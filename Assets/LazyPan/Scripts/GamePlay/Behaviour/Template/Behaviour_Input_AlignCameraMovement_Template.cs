using UnityEngine;

namespace LazyPan {
    public class Behaviour_Input_AlignCameraMovement_Template : Behaviour {
        public Behaviour_Input_AlignCameraMovement_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*获取输入*/
		private void GetInput() {
		}

		/*获取移动物*/
		private void GetTarget() {
		}

		/*获取相机*/
		private void GetCamera() {
		}

		/*获取速度*/
		private void GetSpeed() {
		}

		/*获取输入对齐相机方向*/
		private void GetInputAlignCameraDir() {
		}

		/*设置角色控制*/
		private void SetPlayerControl() {
		}

		/*获取可以移动*/
		private void GetPlayerControl() {
		}

		/*获取位移方向*/
		private void GetMovementDir() {
		}

		/*设置位移方向*/
		private void SetMovementDir() {
		}

		/*移动*/
		private void Movement() {
		}

		/*获取动画控制器*/
		private void GetAnimator() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}