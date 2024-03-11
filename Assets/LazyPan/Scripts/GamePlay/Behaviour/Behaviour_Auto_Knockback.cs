using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Knockback : Behaviour {
        private CharacterController characterController;
        public Behaviour_Auto_Knockback(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            if (entity.EntityData.BaseRuntimeData.CurKnockbackDeployTime > 0) {
                entity.EntityData.BaseRuntimeData.CurKnockbackDeployTime -= Time.deltaTime;
                characterController.Move(entity.EntityData.BaseRuntimeData.CurKnockbackDir *
                                         Time.deltaTime * entity.EntityData.BaseRuntimeData.DefKnockbackSpeed);
            } else {
                entity.EntityData.BaseRuntimeData.CurKnockbackDeployTime = -1;
                entity.EntityData.BaseRuntimeData.CurKnockbackDir = Vector3.zero;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}