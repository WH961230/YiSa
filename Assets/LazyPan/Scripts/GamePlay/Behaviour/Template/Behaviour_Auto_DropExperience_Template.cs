using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_DropExperience_Template : Behaviour {
        public Behaviour_Auto_DropExperience_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*死亡掉落*/
		private void DeathDrop() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}