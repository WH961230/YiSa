using UnityEngine;


namespace LazyPan {
    public class Behaviour_Trigger_ChargeStartGame : Behaviour {
        public Behaviour_Trigger_ChargeStartGame(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
	        entity.EntityData.BaseRuntimeData.CurMaxEnergy = 3;
	        entity.EntityData.BaseRuntimeData.CurEnergy = 0;
	        entity.EntityData.BaseRuntimeData.CurChargeEnergySpeed = 1;
	        entity.EntityData.BaseRuntimeData.DefEnergyDownSpeed = 1;
        }

		/*进入充能*/
		public void ChargeIn(Collider arg0) {
		}

		/*离开充能*/
		public void ChargeOut(Collider arg0) {
		}

		/*充能刷新*/
		public void Charge() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}