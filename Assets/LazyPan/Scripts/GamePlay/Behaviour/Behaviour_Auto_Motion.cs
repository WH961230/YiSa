// using UnityEngine;
//
// namespace LazyPan {
//     public class Behaviour_Auto_Motion : Behaviour {
//         private CharacterController characterController;
//         public Behaviour_Auto_Motion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
//             characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
//             Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
//         }
//
//         private void OnUpdate() {
//             MotionToPlayer();
//         }
//
//         /*向玩家移动*/
//         private void MotionToPlayer() {
//             Vector3 dir = (Cond.Instance.Get<Transform>(Cond.Instance.GetPlayerEntity(), Label.BODY).position -
//                            Cond.Instance.Get<Transform>(entity, Label.BODY).position).normalized;
//             bool getSetting = Loader.LoadSetting().TryGetRobotBySign(entity.ObjConfig.Sign, out RobotSettingInfo info);
//             float movementSpeed = info.MovementSpeed;
//             if (getSetting) {
//                 if (entity.EntityData.BaseRuntimeData.RobotInfo.SlowTime > 0) {
//                     entity.EntityData.BaseRuntimeData.RobotInfo.SlowTime -= Time.deltaTime;
//                     movementSpeed *= 0.7f;
//                 } else {
//                     entity.EntityData.BaseRuntimeData.RobotInfo.SlowTime = 0;
//                 }
//                 characterController.Move(dir * Time.deltaTime * movementSpeed);
//             }
//         }
//
//         public override void Clear() {
//             base.Clear();
//             Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
//         }
//     }
// }