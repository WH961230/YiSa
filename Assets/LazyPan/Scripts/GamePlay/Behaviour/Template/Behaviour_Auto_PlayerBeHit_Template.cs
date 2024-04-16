using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_PlayerBeHit_Template : Behaviour {
        public Behaviour_Auto_PlayerBeHit_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*受到伤害*/
		private void BeHit() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}