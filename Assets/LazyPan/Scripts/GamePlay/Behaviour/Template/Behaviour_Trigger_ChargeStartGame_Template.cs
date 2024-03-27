using UnityEngine;

namespace LazyPan {
    public class Behaviour_Trigger_ChargeStartGame_Template : Behaviour {
        public Behaviour_Trigger_ChargeStartGame_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*进入充能*/
		private void ChargeIn() {
		}

		/*离开充能*/
		private void ChargeOut() {
		}

		/*充能刷新*/
		private void Charge() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}