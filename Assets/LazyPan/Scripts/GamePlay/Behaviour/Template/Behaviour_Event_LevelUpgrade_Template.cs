using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_LevelUpgrade_Template : Behaviour {
        public Behaviour_Event_LevelUpgrade_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*BUFF三选一*/
		private void SelectOneOutOfThree() {
		}

		/*设置升级*/
		private void LevelUpgrade() {
		}

		/*选择机器人配置*/
		private void SelectRobotSetting() {
		}

		/*设置是否可控*/
		private void SetCanControl() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}