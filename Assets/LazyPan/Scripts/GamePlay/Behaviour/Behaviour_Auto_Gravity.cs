using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Gravity : Behaviour {
        private CharacterController characterController;
        public Behaviour_Auto_Gravity(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            Data.Instance.OnLateUpdateEvent.AddListener(OnGravityLateUpdate);
        }

        private void OnGravityLateUpdate() {
            if (entity.EntityData.BaseRuntimeData.CurMotionState == 0) {
                entity.EntityData.BaseRuntimeData.CurGravityDir = Vector3.zero;
                entity.EntityData.BaseRuntimeData.CurGravityDir += Vector3.down;
                characterController.Move(entity.EntityData.BaseRuntimeData.CurGravityDir * Time.deltaTime *
                                         entity.EntityData.BaseRuntimeData.GravitySpeed);
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnGravityLateUpdate);
        }
    }
}