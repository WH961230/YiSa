// using UnityEngine;
// using UnityEngine.InputSystem;
//
// namespace LazyPan {
//     public class Behaviour_Input_MoveRelativeToWorldCoordinates : Behaviour {
// 	    private Vector2 input;
// 	    public Behaviour_Input_MoveRelativeToWorldCoordinates(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
// 		    InputRegister.Instance.Load(InputRegister.Instance.Motion, GetInput);
// 		    Data.Instance.OnUpdateEvent.AddListener(Movement);
// 		    Data.Instance.GlobalInfo.AllowMovement = true;
// 		    entity.EntityData.BaseRuntimeData.PlayerInfo.MovementDir =
// 			    Cond.Instance.Get<Transform>(entity, Label.BODY).forward;
// 	    }
//
// 		/*获取移动物*/
// 		private CharacterController GetTarget() {
// 			return Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
// 		}
//
// 		/*获取输入*/
// 		private void GetInput(InputAction.CallbackContext obj) {
// 			input = obj.ReadValue<Vector2>();
// 		}
//
// 		/*获取速度*/
// 		private float GetSpeed() {
// 			return Loader.LoadSetting().PlayerSetting.MovementSpeed;
// 		}
//
// 		/*移动*/
// 		private void Movement() {
// 			if (entity.EntityData == null) {
// 				return;
// 			}
// 			if (Data.Instance.GlobalInfo.AllowMovement) {
// 				CharacterController cc = GetTarget();
// 				float speed = GetSpeed();
// 				Vector3 dir = new Vector3(input.x, 0, input.y);
// 				if (dir != Vector3.zero) {
// 					entity.EntityData.BaseRuntimeData.PlayerInfo.MovementDir = dir;
// 				}
// 				cc.Move(speed * Time.deltaTime * dir);
// 			}
// 		}
//
// 		public override void Clear() {
//             base.Clear();
//             Data.Instance.OnUpdateEvent.RemoveListener(Movement);
//             InputRegister.Instance.UnLoad(InputRegister.Instance.Motion, GetInput);
// 		}
//     }
// }