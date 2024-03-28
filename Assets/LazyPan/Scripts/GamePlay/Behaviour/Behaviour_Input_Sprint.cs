using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Sprint : Behaviour {
        private TrailRenderer trailRenderer;
        private CharacterController characterController;

        public Behaviour_Input_Sprint(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            trailRenderer = Cond.Instance.Get<TrailRenderer>(entity, Label.Assemble(Label.BODY, Label.TRAILRENDERER));

            InputRegister.Instance.Load(InputRegister.Instance.Space, GetInput);
            Data.Instance.OnUpdateEvent.AddListener(Sprinting);
        }

        /*获取输入*/
        private void GetInput(InputAction.CallbackContext obj) {
            if (obj.performed) {
                Sprint();
            }
        }

        /*冲刺*/
        private void Sprint() {
            if (entity.EntityData.BaseRuntimeData.CurMotionState != 2 && entity.EntityData.BaseRuntimeData.CurTeleportColdDeployTime == 0) {
                trailRenderer.gameObject.SetActive(true);
                entity.EntityData.BaseRuntimeData.CurTeleportDeployTime = entity.EntityData.BaseRuntimeData.DefTeleportTime;
                entity.EntityData.BaseRuntimeData.CurTeleportColdDeployTime =
                    entity.EntityData.BaseRuntimeData.DefTeleportColdTime;
                entity.EntityData.BaseRuntimeData.CurMotionState = 2;
                SetCanControl(false);
            }
        }

        /*设置是否可控*/
        private void SetCanControl(bool canControl) {
            Data.Instance.CanControl = canControl;
        }

        /*冲刺中*/
        private void Sprinting() {
            if (entity.EntityData.BaseRuntimeData.CurMotionState == 2) {
                if (entity.EntityData.BaseRuntimeData.CurTeleportDeployTime > 0) {
                    entity.EntityData.BaseRuntimeData.CurTeleportDeployTime -= Time.deltaTime;
                    entity.EntityData.BaseRuntimeData.CurTeleportDir = Vector3.zero;
                    entity.EntityData.BaseRuntimeData.CurTeleportDir = Cond.Instance.Get<Transform>(entity, Label.BODY).forward;
                    characterController.Move(entity.EntityData.BaseRuntimeData.CurTeleportDir *
                                             Time.deltaTime * entity.EntityData.BaseRuntimeData.DefTeleportSpeed);
                } else {
                    entity.EntityData.BaseRuntimeData.CurTeleportDeployTime = -1;
                    trailRenderer.gameObject.SetActive(false);
                    entity.EntityData.BaseRuntimeData.CurTeleportDir = Vector3.zero;
                    entity.EntityData.BaseRuntimeData.CurMotionState = 0;
                    SetCanControl(true);
                }
            }

            //冲刺冷却
            if (entity.EntityData.BaseRuntimeData.CurTeleportColdDeployTime > 0) {
                entity.EntityData.BaseRuntimeData.CurTeleportColdDeployTime -= Time.deltaTime;
            } else {
                entity.EntityData.BaseRuntimeData.CurTeleportColdDeployTime = 0;
            }
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Space, GetInput);
            Data.Instance.OnUpdateEvent.RemoveListener(Sprinting);
        }
    }
}