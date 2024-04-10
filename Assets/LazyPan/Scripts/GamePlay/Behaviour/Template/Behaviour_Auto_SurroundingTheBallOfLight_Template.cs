using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_SurroundingTheBallOfLight_Template : Behaviour {
        public Behaviour_Auto_SurroundingTheBallOfLight_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*环绕*/
		private void Surround() {
		}

		/*环绕碰撞*/
		private void SurroundTrigger() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}