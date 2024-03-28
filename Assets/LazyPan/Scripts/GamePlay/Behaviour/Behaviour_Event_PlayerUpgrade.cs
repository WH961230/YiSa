using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_PlayerUpgrade : Behaviour {
        public Behaviour_Event_PlayerUpgrade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*BUFF三选一*/
		private void SelectOneOutOfThree() {
		}

        public override void Clear() {
            base.Clear();
        }
    }
}