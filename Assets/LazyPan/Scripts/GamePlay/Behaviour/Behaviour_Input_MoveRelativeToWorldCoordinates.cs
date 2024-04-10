using UnityEngine;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class Behaviour_Input_MoveRelativeToWorldCoordinates : Behaviour {
	    private Vector2 input;
	    public Behaviour_Input_MoveRelativeToWorldCoordinates(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
		    InputRegister.Instance.Load(InputRegister.Instance.Motion, GetInput);
		    Data.Instance.OnUpdateEvent.AddListener(Movement);
		    SetPlayerControl(true);
	    }

		/*获取移动物*/
		private CharacterController GetTarget() {
			return Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
		}

		/*获取输入*/
		private void GetInput(InputAction.CallbackContext obj) {
			input = obj.ReadValue<Vector2>();
		}

		/*获取速度*/
		private float GetSpeed() {
			return Loader.LoadSetting().PlayerSetting.MovementSpeed;
		}

		/*设置角色控制*/
		private void SetPlayerControl(bool canControl) {
			Data.Instance.GlobalInfo.AllowMovement = canControl;
		}

		/*获取可以移动*/
		private bool GetPlayerControl() {
			return Data.Instance.GlobalInfo.AllowMovement;
		}

		/*移动*/
		private void Movement() {
			if (entity.EntityData == null) {
				return;
			}
			if (GetPlayerControl()) {
				CharacterController cc = GetTarget();
				float speed = GetSpeed();
				Vector3 dir = new Vector3(input.x, 0, input.y);
				cc.Move(speed * Time.deltaTime * dir);
			}
		}

		public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Movement);
            InputRegister.Instance.UnLoad(InputRegister.Instance.Motion, GetInput);
		}
    }
}