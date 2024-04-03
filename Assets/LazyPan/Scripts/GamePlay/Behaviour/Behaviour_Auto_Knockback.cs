using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Knockback : Behaviour {
        private CharacterController characterController;
        private Vector3 knockbackDir;
        private float knockbackSpeed;
        private float deploy;

        public Behaviour_Auto_Knockback(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
            MessageRegister.Instance.Reg<Entity, Entity>(MessageCode.BeHit, BeHit);
        }

        private void BeHit(Entity arg1, Entity arg2) {
            if (entity.ID == arg2.ID) {
                knockbackDir = (Cond.Instance.Get<Transform>(entity, Label.BODY).position -
                                Cond.Instance.Get<Transform>(arg1, Label.BODY).position).normalized;
                bool getSetting = Loader.LoadSetting()
                    .TryGetRobotBySign(entity.ObjConfig.Sign, out RobotSettingInfo info);
                if (getSetting) {
                    knockbackSpeed = info.KnockbackSpeed;
                    deploy = info.KnockbackDuraing;
                }
            }
        }

        private void OnUpdate() {
            if (deploy > 0) {
                deploy -= Time.deltaTime;
                characterController.Move(knockbackDir * Time.deltaTime * knockbackSpeed);
            } else {
                deploy = -1;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
            MessageRegister.Instance.UnReg<Entity, Entity>(MessageCode.BeHit, BeHit);
        }
    }
}