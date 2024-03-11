using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_Teleport : Behaviour {
        private TrailRenderer trailRenderer;
        private CharacterController characterController;
        private float teleportColdDeployTime;
        private float teleportDeployTime;

        public Behaviour_Input_Teleport(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            trailRenderer = Cond.Instance.Get<TrailRenderer>(entity, Label.Assemble(Label.BODY, Label.TRAILRENDERER));

            InputRegister.Instance.Load(InputRegister.Instance.Space, Teleport);
            Data.Instance.OnUpdateEvent.AddListener(OnTeleportUpdate);
        }

        private void Teleport(InputAction.CallbackContext obj) {
            if (entity.EntityData.BaseRuntimeData.CurMotionState != 2 && teleportColdDeployTime == 0) {
                trailRenderer.gameObject.SetActive(true);
                teleportDeployTime = entity.EntityData.BaseRuntimeData.DefTeleportTime;
                teleportColdDeployTime = entity.EntityData.BaseRuntimeData.DefTeleportColdTime;
                entity.EntityData.BaseRuntimeData.CurMotionState = 2;
            }
        }

        private void OnTeleportUpdate() {
            if (entity.EntityData.BaseRuntimeData.CurMotionState == 2) {
                if (teleportDeployTime > 0) {
                    teleportDeployTime -= Time.deltaTime;
                    entity.EntityData.BaseRuntimeData.CurTeleportDir = Vector3.zero;
                    entity.EntityData.BaseRuntimeData.CurTeleportDir = Cond.Instance.Get<Transform>(entity, Label.BODY).forward;
                    characterController.Move(entity.EntityData.BaseRuntimeData.CurTeleportDir *
                                             Time.deltaTime * entity.EntityData.BaseRuntimeData.DefTeleportSpeed);
                } else {
                    teleportDeployTime = -1;
                    trailRenderer.gameObject.SetActive(false);
                    entity.EntityData.BaseRuntimeData.CurTeleportDir = Vector3.zero;
                    entity.EntityData.BaseRuntimeData.CurMotionState = 0;
                }
            }

            //冲刺冷却
            if (teleportColdDeployTime > 0) {
                teleportColdDeployTime -= Time.deltaTime;
            } else {
                teleportColdDeployTime = 0;
            }
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Space, Teleport);
            Data.Instance.OnUpdateEvent.RemoveListener(OnTeleportUpdate);
        }
    }
}