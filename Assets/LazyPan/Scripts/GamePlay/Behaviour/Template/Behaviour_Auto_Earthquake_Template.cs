using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Earthquake_Template : Behaviour {
        public Behaviour_Auto_Earthquake_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*地震*/
		private void Earthquake() {
		}

		/*地震碰撞*/
		private void EarthquakeTrigger() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}