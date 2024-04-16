using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_SurroundingTheBallOfLight : Behaviour {
        public Behaviour_Auto_SurroundingTheBallOfLight(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        Debug.Log("环绕球体注册");
        }

		/*环绕*/
		private void Surround() {
		}

		/*环绕碰撞*/
		private void SurroundTrigger() {
		}

		public override void Upgrade() {
			base.Upgrade();
			Debug.Log("环绕球体升级");
		}

		public override void Clear() {
            base.Clear();
        }
    }
}