using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Gravity_Template : Behaviour {
        public Behaviour_Auto_Gravity_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*重力*/
		private void Gravity() {
		}

		/*获取重力速度*/
		private void GetGravitySpeed() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}