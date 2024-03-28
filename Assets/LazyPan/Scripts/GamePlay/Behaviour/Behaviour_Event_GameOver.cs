using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_GameOver : Behaviour {
        public Behaviour_Event_GameOver(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(GameOver);
        }

		/*游戏结束*/
		private void GameOver() {
            /*检测游戏是否结束*/
		}

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(GameOver);
        }
    }
}