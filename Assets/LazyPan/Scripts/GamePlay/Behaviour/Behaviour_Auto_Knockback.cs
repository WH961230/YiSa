// using UnityEngine;
//
// namespace LazyPan {
//     public class Behaviour_Auto_Knockback : Behaviour {
//         private CharacterController characterController;
//         private Vector3 knockbackDir;
//         private float knockbackSpeed = 50;
//         private float deploy = -1;
//
//         public Behaviour_Auto_Knockback(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
//             characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
//             Data.Instance.OnUpdateEvent.AddListener(Knockback);
//             MessageRegister.Instance.Reg<Entity, Entity>(MessageCode.BeHit, BeHit);
//         }
//
//         /*被击中*/
//         private void BeHit(Entity arg1, Entity arg2) {
//             if (entity.ID == arg2.ID) {
//                 knockbackDir = (Cond.Instance.Get<Transform>(entity, Label.BODY).position -
//                                 Cond.Instance.Get<Transform>(arg1, Label.BODY).position).normalized;
//                 deploy = 0.01f;
//             }
//         }
//
//         /*击退*/
//         private void Knockback() {
//             if (deploy > 0) {
//                 deploy -= Time.deltaTime;
//                 characterController.Move(knockbackDir * Time.deltaTime * knockbackSpeed);
//             } else {
//                 deploy = -1;
//             }
//         }
//
//         public override void Clear() {
//             base.Clear();
//             Data.Instance.OnUpdateEvent.RemoveListener(Knockback);
//             MessageRegister.Instance.UnReg<Entity, Entity>(MessageCode.BeHit, BeHit);
//         }
//     }
// }