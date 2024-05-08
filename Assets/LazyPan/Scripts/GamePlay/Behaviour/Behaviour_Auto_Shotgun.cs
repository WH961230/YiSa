using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Shotgun : Behaviour {
        private int ShotgunNum = 1;
        private float deploy;
        private float attackIntervalTime;
        private float attackRange;
        private float attackDamage;
        private BuffSettingInfo buffSettingInfo;
        public Behaviour_Auto_Shotgun(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(Shotgun);
            /*获取配置*/
            Loader.LoadSetting().BuffSetting.GetSettingBySign(behaviourSign, out buffSettingInfo);
            /*攻击伤害*/
            buffSettingInfo.GetParam("AttackDamage", out string attackdamage);
            attackDamage = float.Parse(attackdamage);
            /*攻击范围*/
            buffSettingInfo.GetParam("AttackRange", out string attackrange);
            attackRange = float.Parse(attackrange);
            /*攻击间隔*/
            buffSettingInfo.GetParam("AttackIntervalTime", out string attackintervaltime);
            attackIntervalTime = float.Parse(attackintervaltime);
        }

        /*霰弹枪*/
        private void Shotgun() {
            if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
                if (deploy > 0) {
                    deploy -= Time.deltaTime;
                } else {
                    bool findRobotEntity = Cond.Instance.GetRandEntityByTypeWithinDistance(Label.ROBOT,
                        Cond.Instance.Get<Transform>(entity, Label.BODY).position,
                        attackRange, out Entity robotEntity);
                    if (findRobotEntity && robotEntity.EntityData.BaseRuntimeData.RobotInfo.HealthPoint > 0) {
                        buffSettingInfo.GetParam("Bullet", out string bullet);
                        GameObject template = Loader.LoadGo("弹药", string.Concat("Common/", bullet), Data.Instance.ObjRoot, true);
                        Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
                        template.transform.position = bulletMuzzle.position;
                        template.transform.forward = (Cond.Instance.Get<Transform>(robotEntity, Label.HIT).position - bulletMuzzle.position).normalized;
                        Comp templateComp = template.GetComponent<Comp>();
                        templateComp.OnParticleCollisionEvent.RemoveAllListeners();
                        templateComp.OnParticleCollisionEvent.AddListener(OnTriggerEnter);
                        Sound.Instance.SoundPlay("Shotgun", Vector3.zero, false, 2);
                    }
                    deploy = attackIntervalTime;
                }
            }
        }

        private void OnTriggerEnter(GameObject go) {
            if (go.layer != LayerMask.NameToLayer("EnemySideBeHit")) {
                return;
            }
            if (Data.Instance.TryGetEntityByBodyPrefabID(go.GetInstanceID(), out Entity tmpEntity)) {
                if (tmpEntity.EntityData.BaseRuntimeData.Type != "Robot") {
                    return;
                }
                if (tmpEntity.EntityData.BaseRuntimeData.RobotInfo.HealthPoint > 0) {
                    tmpEntity.EntityData.BaseRuntimeData.RobotInfo.BeAttackType = 1;
                    MessageRegister.Instance.Dis(MessageCode.BeInjuried, tmpEntity, attackDamage);
                }
            }
        }

        public override void Upgrade() {
            base.Upgrade();
            ShotgunNum++;
            if (ShotgunNum > buffSettingInfo.UpgradeLimit) {
                ShotgunNum = buffSettingInfo.UpgradeLimit;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Shotgun);
        }
    }
}