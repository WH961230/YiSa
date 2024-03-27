using UnityEngine;


namespace LazyPan {
    public class Behaviour_Trigger_ChargeStartGame_Template : Behaviour {
        public Behaviour_Trigger_ChargeStartGame_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*进入充能*/
		private void ChargeIn(Collider arg0) {
		}

		/*离开充能*/
		private void ChargeOut(Collider arg0) {
		}

		/*充能刷新*/
		private void Charge() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}