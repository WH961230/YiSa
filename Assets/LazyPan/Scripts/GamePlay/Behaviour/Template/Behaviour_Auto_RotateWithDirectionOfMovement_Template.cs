using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_RotateWithDirectionOfMovement_Template : Behaviour {
        public Behaviour_Auto_RotateWithDirectionOfMovement_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*获取移动方向*/
		private void GetMovementDir() {
		}

		/*旋转*/
		private void RotateToDir() {
		}

		/*获取旋转速度*/
		private void GetRotateSpeed() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}