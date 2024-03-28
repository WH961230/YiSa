using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_PlayerUpgrade_Template : Behaviour {
        public Behaviour_Event_PlayerUpgrade_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*BUFF三选一*/
		private void SelectOneOutOfThree() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}