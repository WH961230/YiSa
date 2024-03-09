using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Gravity : Behaviour {
        private CharacterController characterController;
        public Behaviour_Auto_Gravity(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            Data.Instance.OnLateUpdateEvent.AddListener(OnGravityLateUpdate);
        }

        private void OnGravityLateUpdate() {
            entity.EntityData.BaseRuntimeData.CurMotionDir = Vector3.zero;
            entity.EntityData.BaseRuntimeData.CurMotionDir += Vector3.down;
            characterController.Move(entity.EntityData.BaseRuntimeData.CurMotionDir * Time.deltaTime *
                                     entity.EntityData.BaseRuntimeData.GravitySpeed);
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnGravityLateUpdate);
        }
    }
}