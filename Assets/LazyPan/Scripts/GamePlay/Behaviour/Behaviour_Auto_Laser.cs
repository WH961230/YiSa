using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Laser : Behaviour {
        public Behaviour_Auto_Laser(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*发射激光*/
		private void Laser() {
		}

		/*激光碰撞*/
		private void LaserTrigger() {
		}

		public override void Upgrade() {
			base.Upgrade();
		}

		public override void Clear() {
            base.Clear();
        }
    }
}