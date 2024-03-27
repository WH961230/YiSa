using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_BeginTimeline_Template : Behaviour {
        public Behaviour_Event_BeginTimeline_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*设置角色控制*/
		private void SetPlayerControl() {
		}

		/*获取角色控制*/
		private void GetPlayerControl() {
		}

		/*开始播放*/
		private void StartPlay() {
		}

		/*停止播放*/
		private void StopPlay() {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}