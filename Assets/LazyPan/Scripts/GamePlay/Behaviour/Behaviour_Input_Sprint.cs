using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Sprint : Behaviour {
        private TrailRenderer trailRenderer;
        private CharacterController characterController;
        private float deploy;

        public Behaviour_Input_Sprint(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            trailRenderer = Cond.Instance.Get<TrailRenderer>(entity, Label.Assemble(Label.BODY, Label.TRAILRENDERER));

            InputRegister.Instance.Load(InputRegister.Instance.Space, GetInput);
            Data.Instance.OnUpdateEvent.AddListener(Sprinting);
            deploy = -1;
        }

        /*获取输入*/
        private void GetInput(InputAction.CallbackContext obj) {
            if (obj.performed) {
                Sprint();
            }
        }

        /*获取冲刺速度*/
        private float GetSprintSpeed() {
            return Loader.LoadSetting().PlayerSetting.SprintSpeed;
        }

        /*冲刺*/
        private void Sprint() {
            if (deploy == -1) {
                deploy = Loader.LoadSetting().PlayerSetting.SprintTime;
            }
        }

        /*设置是否可控*/
        private void SetCanControl(bool canControl) {
            Data.Instance.GlobalInfo.AllowMovement = canControl;
        }

        /*冲刺中*/
        private void Sprinting() {
            if (deploy > 0) {
                deploy -= Time.deltaTime;
                characterController.Move(Cond.Instance.Get<Transform>(entity, Label.BODY).forward * Time.deltaTime * GetSprintSpeed());
            } else {
                if (deploy != -1) {
                    deploy = -1;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Space, GetInput);
            Data.Instance.OnUpdateEvent.RemoveListener(Sprinting);
        }
    }
}