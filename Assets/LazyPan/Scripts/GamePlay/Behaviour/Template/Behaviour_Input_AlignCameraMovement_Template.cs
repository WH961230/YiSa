using UnityEngine;
using UnityEngine.InputSystem;


namespace LazyPan {
    public class Behaviour_Input_AlignCameraMovement_Template : Behaviour {
        public Behaviour_Input_AlignCameraMovement_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*移动输入检测*/
		private void MotionEvent(InputAction.CallbackContext obj) {
		}

		/*角色移动更新*/
		private void OnMotionUpdate() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}