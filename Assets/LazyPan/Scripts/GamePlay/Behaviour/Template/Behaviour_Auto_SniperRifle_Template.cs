using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_SniperRifle_Template : Behaviour {
        public Behaviour_Auto_SniperRifle_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*发射*/
		private void Shoot() {
		}

		/*碰撞*/
		private void ShootTrigger() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}