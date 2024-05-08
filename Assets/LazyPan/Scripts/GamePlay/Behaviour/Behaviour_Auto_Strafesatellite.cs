using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Strafesatellite : Behaviour {
        /*旋转雇佣时间*/
        private float deploy;
        /*发射弹药雇佣时间*/
        private float shotDeploy;
        /*卫星配置参数*/
        private int StrafesatelliteNum = 1;//卫星等级 卫星数量
        private float StrafesatelliteRoundSpeed;//卫星环绕速度
        private float StrafesatelliteBulletShootInterval;//卫星子弹发射频率
        private int StrafesatelliteBulletAttackDamage;//卫星子弹发射伤害
        /*不同等级的扫射卫星*/
        private Comp Level1;
        private Comp Level2;
        private Comp Level3;
        /*卫星主体*/
        private GameObject strafesatellite;
        /*配置*/
        private BuffSettingInfo buffSettingInfo;
        public Behaviour_Auto_Strafesatellite(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            /*扫射*/
            Data.Instance.OnUpdateEvent.AddListener(Strafesatellite);
            /*获取配置*/
            Loader.LoadSetting().BuffSetting.GetSettingBySign(behaviourSign, out buffSettingInfo);
            /*初始化弹药*/
            InitBullet();
            /*获取旋转速度*/
            buffSettingInfo.GetParam("RoundSpeed", out string speed);
            StrafesatelliteRoundSpeed = float.Parse(speed);
            /*获取发射间隔时间*/
            buffSettingInfo.GetParam("ShotInterval", out string shotInterval);
            StrafesatelliteBulletShootInterval = float.Parse(shotInterval);
            /*卫星发射伤害*/
            buffSettingInfo.GetParam("AttackDamage", out string attackDamage);
            StrafesatelliteBulletAttackDamage = int.Parse(attackDamage);
        }

        /*初始化弹药*/
        private void InitBullet() {
            buffSettingInfo.GetParam("Bullet", out string bullet);
            strafesatellite = Loader.LoadGo("弹药", string.Concat("Common/", bullet), Data.Instance.ObjRoot, true);
            strafesatellite.transform.position = Cond.Instance.Get<Transform>(entity, Label.MUZZLE).position;
            Level1 = Cond.Instance.Get<Comp>(strafesatellite.GetComponent<Comp>(), Label.Assemble(Label.LEVEL, "1"));
            Level2 = Cond.Instance.Get<Comp>(strafesatellite.GetComponent<Comp>(), Label.Assemble(Label.LEVEL, "2"));
            Level3 = Cond.Instance.Get<Comp>(strafesatellite.GetComponent<Comp>(), Label.Assemble(Label.LEVEL, "3"));
        }

        /*显示卫星*/
        private void DisplayStrafesatellite() {
            Level1.gameObject.SetActive(StrafesatelliteNum == 1);
            Level2.gameObject.SetActive(StrafesatelliteNum == 2);
            Level3.gameObject.SetActive(StrafesatelliteNum == 3);
        }

        /*隐藏所有的卫星*/
        private void HideAllStrafesatellite() {
            Level1.gameObject.SetActive(false);
            Level2.gameObject.SetActive(false);
            Level3.gameObject.SetActive(false);
        }

        /*发射扫射卫星的子弹*/
        private void ShotStrafesatelliteBullet(Comp level) {
            if (level.gameObject.activeSelf) {
                /*在每个发射口 按照间隔时间发射弹药*/
                if (shotDeploy > 0) {
                    shotDeploy -= Time.deltaTime;
                } else {
                    Sound.Instance.SoundPlay("Strafesatellite", Vector3.zero, false, 1, 2);
                    foreach (Comp.TransformData data in level.Transforms) {
                        buffSettingInfo.GetParam("ShotBullet", out string bullet);
                        GameObject shotStrafesatellite = Loader.LoadGo("弹药", string.Concat("Common/", bullet),
                            Data.Instance.ObjRoot, true);
                        shotStrafesatellite.transform.position = data.Tran.position;
                        shotStrafesatellite.transform.forward = data.Tran.forward;

                        Comp templateComp = strafesatellite.GetComponent<Comp>();
                        templateComp.OnParticleCollisionEvent.RemoveAllListeners();
                        templateComp.OnParticleCollisionEvent.AddListener(BulletTriggerHit);
                        templateComp.OnParticleCollisionEvent.AddListener((aaa) => {
                            Object.Destroy(shotStrafesatellite);
                        });
                    }

                    shotDeploy = StrafesatelliteBulletShootInterval;
                }
            }
        }

        /*扫射卫星*/
        private void Strafesatellite() {
            if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
                DisplayStrafesatellite();
                ShotStrafesatelliteBullet(Level1);
                ShotStrafesatelliteBullet(Level2);
                ShotStrafesatelliteBullet(Level3);
                Cond.Instance.Get<Transform>(strafesatellite.GetComponent<Comp>(), Label.BODY).transform
                    .Rotate(Vector3.up * Time.deltaTime * StrafesatelliteRoundSpeed);
            } else {
                HideAllStrafesatellite();
            }
        }

        /*弹药触发*/
        private void BulletTriggerHit(GameObject go) {
            if (go.layer != LayerMask.NameToLayer("EnemySideBeHit")) {
                return;
            }
            if (Data.Instance.TryGetEntityByBodyPrefabID(go.GetInstanceID(), out Entity tmpEntity)) {
                if (tmpEntity.EntityData.BaseRuntimeData.Type != "Robot") {
                    return;
                }
                if (tmpEntity.EntityData.BaseRuntimeData.RobotInfo.HealthPoint > 0) {
                    tmpEntity.EntityData.BaseRuntimeData.RobotInfo.BeAttackType = 1;
                    MessageRegister.Instance.Dis(MessageCode.BeInjuried, tmpEntity, StrafesatelliteBulletAttackDamage);
                }
            }
        }

        public override void Upgrade() {
            base.Upgrade();
            StrafesatelliteNum++;
            if (StrafesatelliteNum > 3) {
                StrafesatelliteNum = 3;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Strafesatellite);
        }
    }
}