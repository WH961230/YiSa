// using System;
// using UnityEngine;
// using UnityEngine.InputSystem;
//
// namespace LazyPan {
//     public class Behaviour_Input_Sprint : Behaviour {
//         private CharacterController characterController;
//         private float deploy;
//
//         public Behaviour_Input_Sprint(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
//             characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
//             InputRegister.Instance.Load(InputRegister.Instance.Space, GetInput);
//             Data.Instance.OnUpdateEvent.AddListener(Sprinting);
//             deploy = -1;
//         }
//
//         /*获取输入*/
//         private void GetInput(InputAction.CallbackContext obj) {
//             if (obj.performed) {
//                 Sprint();
//             }
//         }
//
//         /*获取冲刺速度*/
//         private float GetSprintSpeed() {
//             return Loader.LoadSetting().PlayerSetting.SprintSpeed;
//         }
//
//         /*冲刺*/
//         private void Sprint() {
//             if (Math.Abs(deploy - (-1)) < 0.01f) {
//                 deploy = Loader.LoadSetting().PlayerSetting.SprintTime;
//                 Data.Instance.GlobalInfo.AllowMovement = false;
//             }
//         }
//
//         /*冲刺中*/
//         private void Sprinting() {
//             if (Math.Abs(deploy - (-1)) < 0.01f) {
//                 return;
//             }
//
//             if (deploy > 0) {
//                 deploy -= Time.deltaTime;
//                 characterController.Move(entity.EntityData.BaseRuntimeData.PlayerInfo.MovementDir * Time.deltaTime * GetSprintSpeed());
//             } else {
//                 deploy = -1;
//                 Data.Instance.GlobalInfo.AllowMovement = true;
//             }
//         }
//
//         public override void Clear() {
//             base.Clear();
//             InputRegister.Instance.UnLoad(InputRegister.Instance.Space, GetInput);
//             Data.Instance.OnUpdateEvent.RemoveListener(Sprinting);
//         }
//     }
// }