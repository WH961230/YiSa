using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Ring : Behaviour {
        private int RingNum = 1;
        private float deploy;
        private float attackIntervalTime;
        private float attackRange;
        private BuffSettingInfo buffSettingInfo;
        public Behaviour_Auto_Ring(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(Ring);
            /*获取配置*/
            Loader.LoadSetting().BuffSetting.GetSettingBySign(behaviourSign, out buffSettingInfo);
            /*攻击范围*/
            buffSettingInfo.GetParam("AttackRange", out string attackrange);
            attackRange = float.Parse(attackrange);
            /*攻击间隔*/
            buffSettingInfo.GetParam("AttackIntervalTime", out string attackintervaltime);
            attackIntervalTime = float.Parse(attackintervaltime);

            buffSettingInfo.GetParam("Bullet", out string bullet);
            GameObject template = Loader.LoadGo("圆环弹药", string.Concat("Common/", bullet), Data.Instance.ObjRoot, true);
            Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
            template.transform.position = bulletMuzzle.position;
            Comp templateComp = template.GetComponent<Comp>();
            templateComp.OnParticleCollisionEvent.RemoveAllListeners();
            templateComp.OnParticleCollisionEvent.AddListener(OnTriggerEnter);
        }

        /*圆环*/
        private void Ring() {
            if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
                if (deploy > 0) {
                    deploy -= Time.deltaTime;
                } else {
                    // Sound.Instance.SoundPlay("Shotgun", Vector3.zero, false, 1, 2);


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
                }
            }
        }

        public override void Upgrade() {
            base.Upgrade();
            RingNum++;
            if (RingNum > buffSettingInfo.UpgradeLimit) {
                RingNum = buffSettingInfo.UpgradeLimit;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Ring);
        }
    }
}