using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_GameOver_Template : Behaviour {
        public Behaviour_Event_GameOver_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*游戏结束*/
		private void GameOver() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}