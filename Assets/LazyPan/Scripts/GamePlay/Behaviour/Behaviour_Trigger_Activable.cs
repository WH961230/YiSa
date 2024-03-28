using UnityEngine;

namespace LazyPan {
    public class Behaviour_Trigger_Activable : Behaviour {
        public Behaviour_Trigger_Activable(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*激活*/
		private void Active() {
		}

        public override void Clear() {
            base.Clear();
        }
    }
}