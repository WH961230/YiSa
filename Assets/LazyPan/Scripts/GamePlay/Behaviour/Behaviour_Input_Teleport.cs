using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Teleport : Behaviour {
        private TrailRenderer trailRenderer;
        private CharacterController characterController;

        public Behaviour_Input_Teleport(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            trailRenderer = Cond.Instance.Get<TrailRenderer>(entity, Label.Assemble(Label.BODY, Label.TRAILRENDERER));

            InputRegister.Instance.Load(InputRegister.Instance.Space, Teleport);
            Data.Instance.OnUpdateEvent.AddListener(OnTeleportUpdate);
        }

        private void Teleport(InputAction.CallbackContext obj) {
            if (entity.EntityData.BaseRuntimeData.CurMotionState != 2 && entity.EntityData.BaseRuntimeData.DefTeleportColdTime == 0) {
                trailRenderer.gameObject.SetActive(true);
                entity.EntityData.BaseRuntimeData.CurTeleportDeployTime = entity.EntityData.BaseRuntimeData.DefTeleportTime;
                entity.EntityData.BaseRuntimeData.CurTeleportColdDeployTime =
                    entity.EntityData.BaseRuntimeData.DefTeleportColdTime;
                entity.EntityData.BaseRuntimeData.CurMotionState = 2;
            }
        }

        private void OnTeleportUpdate() {
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
            InputRegister.Instance.UnLoad(InputRegister.Instance.Space, Teleport);
            Data.Instance.OnUpdateEvent.RemoveListener(OnTeleportUpdate);
        }
    }
}