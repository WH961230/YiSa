using UnityEngine;
using UnityEngine.Playables;


namespace LazyPan {
    public class Behaviour_Event_BeginTimeline_Template : Behaviour {
        public Behaviour_Event_BeginTimeline_Template(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

		/*检测角色控制*/
		private void CheckStartControl(PlayableDirector pd) {
		}



        public override void Clear() {
            base.Clear();
        }
    }
}