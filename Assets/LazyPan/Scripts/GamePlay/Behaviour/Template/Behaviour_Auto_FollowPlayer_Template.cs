using UnityEngine;


namespace LazyPan {
    public class Behaviour_Auto_FollowPlayer_Template : Behaviour {
        public Behaviour_Auto_FollowPlayer_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*跟随*/
		private void Follow() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}