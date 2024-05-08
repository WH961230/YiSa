using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Sniperrifle : Behaviour {
        private int SniperrifleNum = 1;
        private BuffSettingInfo buffSettingInfo;
        private float deploy;
        private float attackIntervalTime;
        private float attackDamage;
        private float attackRange;

        public Behaviour_Auto_Sniperrifle(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(Sniperrifle);
            Loader.LoadSetting().BuffSetting
                .GetSettingBySign(behaviourSign, out buffSettingInfo);
            /*攻击间隔时间*/
            buffSettingInfo.GetParam("AttackIntervalTime", out string attackintervaltime);
            attackIntervalTime = float.Parse(attackintervaltime);
            /*攻击伤害*/
            buffSettingInfo.GetParam("AttackDamage", out string attackdamage);
            attackDamage = float.Parse(attackdamage);
            /*攻击范围*/
            buffSettingInfo.GetParam("AttackRange", out string attackrange);
            attackRange = float.Parse(attackrange);
        }

        private void Sniperrifle() {
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
                        Sound.Instance.SoundPlay("Sniperrifle", Vector3.zero, false, 2);
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
            SniperrifleNum++;
            if (SniperrifleNum > 3) {
                SniperrifleNum = 3;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Sniperrifle);
        }
    }
}