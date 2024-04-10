using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Laser_Template : Behaviour {
        public Behaviour_Auto_Laser_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*发射激光*/
		private void Laser() {
		}

		/*激光碰撞*/
		private void LaserTrigger() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}