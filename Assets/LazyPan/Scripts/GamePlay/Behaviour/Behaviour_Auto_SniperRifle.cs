using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_SniperRifle : Behaviour {
        public Behaviour_Auto_SniperRifle(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*发射*/
		private void Shoot() {
		}

		/*碰撞*/
		private void ShootTrigger() {
		}

		public override void Upgrade() {
			base.Upgrade();
		}

		public override void Clear() {
            base.Clear();
        }
    }
}