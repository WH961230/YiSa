using UnityEngine;

namespace LazyPan {
    public class Behaviour_Gravity : Behaviour {
        private CharacterController characterController;
        public bool IsGround = false;
        private Vector3 currentGravityDir; //重力方向
        private Vector3 controllerMoveDir;
        private Collider[] colliders;

        public Behaviour_Gravity(Entity entity, string sign) : base(entity, sign) {
            //获取角色控制器
            characterController = entity.Comp.Get<CharacterController>("CharacterController");

            Data.Instance.OnUpdateEvent.AddListener(OnGravityUpdate);
        }

        private void OnGravityUpdate() {
            Transform ObjectTr = entity.Prefab.transform;

            Vector3 bottom = ObjectTr.position + ObjectTr.up * characterController.radius + ObjectTr.up * entity.EntitySetting.OverlapCapsuleOffset;
            Vector3 top = ObjectTr.position + ObjectTr.up * characterController.height - ObjectTr.up * characterController.radius;
            colliders = Physics.OverlapCapsule(bottom, top, characterController.radius, entity.EntitySetting.GravityDetectMaskLayer);
            IsGround = colliders.Length > 0;
            currentGravityDir = IsGround ? Vector3.zero : Vector3.down;

            controllerMoveDir = Vector3.zero;
            controllerMoveDir += currentGravityDir * entity.EntitySetting.GravitySpeed;

            if (controllerMoveDir.magnitude > 0) {
                characterController.Move(controllerMoveDir * Time.deltaTime);
            }
        }

        public override void OnClear() {
            base.OnClear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnGravityUpdate);
        }
    }
}