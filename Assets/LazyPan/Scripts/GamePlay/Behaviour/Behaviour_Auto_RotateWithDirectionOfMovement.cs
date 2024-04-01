using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_RotateWithDirectionOfMovement : Behaviour {
        public Behaviour_Auto_RotateWithDirectionOfMovement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        Data.Instance.OnLateUpdateEvent.AddListener(RotateToDir);
        }

		/*获取移动方向*/
		private Vector3 GetMovementDir() {
			CharacterController cc = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
			return cc.velocity;
		}

		/*获取移动速度*/
		private float GetRotateSpeed() {
			return Loader.LoadSetting().PlayerSetting.RotateSpeed;
		}

		/*旋转*/
		private void RotateToDir() {
			Transform body = Cond.Instance.Get<Transform>(entity, Label.BODY);
			Vector3 targetDir = GetMovementDir();
			if (targetDir == Vector3.zero) {
				return;
			}
			Quaternion toRotation = Quaternion.LookRotation(targetDir, Vector3.up);
			body.rotation = Quaternion.RotateTowards(body.rotation, toRotation, GetRotateSpeed() * Time.deltaTime);
		}

		public override void Clear() {
            base.Clear();
            Data.Instance.OnLateUpdateEvent.RemoveListener(RotateToDir);
        }
    }
}