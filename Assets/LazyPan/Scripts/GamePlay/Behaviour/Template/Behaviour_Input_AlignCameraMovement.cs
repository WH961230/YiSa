using UnityEngine;
using UnityEngine.InputSystem;


namespace LazyPan {
    public class Behaviour_Input_AlignCameraMovement : Behaviour {
        public Behaviour_Input_AlignCameraMovement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*移动输入检测*/
		public void MotionEvent(InputAction.CallbackContext obj) {
		}

		/*角色移动更新*/
		public void OnMotionUpdate() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}