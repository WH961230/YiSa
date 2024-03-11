using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Shoot : Behaviour {
        public Behaviour_Auto_Shoot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(OnShootUpdate);
        }

        private void OnShootUpdate() {
            if (entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime > 0) {
                entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime -= Time.deltaTime;
            } else {
                Entity robotEntity = Cond.Instance.GetTypeRandEntity(Label.ROBOT);
                if (robotEntity != null) {
                    Debug.LogFormat("射击检测 " + robotEntity.ObjConfig.Sign);
                }
                entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime =
                    entity.EntityData.BaseRuntimeData.DefAttackIntervalTime;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnShootUpdate);
        }
    }
}