using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Motion : Behaviour {
        private CharacterController characterController;
        public Behaviour_Auto_Motion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            MotionToPlayer();
        }

        /*向玩家移动*/
        private void MotionToPlayer() {
            Vector3 dir = (Cond.Instance.Get<Transform>(Cond.Instance.GetPlayerEntity(), Label.BODY).position -
                           Cond.Instance.Get<Transform>(entity, Label.BODY).position).normalized;
            bool getSetting = Loader.LoadSetting().TryGetRobotBySign(entity.ObjConfig.Sign, out RobotSettingInfo info);
            if (getSetting && GetControl()) {
                characterController.Move(dir * Time.deltaTime * info.MovementSpeed);
            }
        }

        /*获取可以移动*/
        private bool GetControl() {
            return Data.Instance.GlobalInfo.AllowMovement;
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}