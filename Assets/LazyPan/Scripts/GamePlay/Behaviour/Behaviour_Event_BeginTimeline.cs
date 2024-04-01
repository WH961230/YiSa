using UnityEngine.Playables;

namespace LazyPan {
    public class Behaviour_Event_BeginTimeline : Behaviour {
        private PlayableDirector pd;
        public Behaviour_Event_BeginTimeline(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            pd = Cond.Instance.Get<PlayableDirector>(entity, Label.PLAYABLEDIRECTOR);
            pd.stopped += StopPlay;
            StartPlay(pd);
        }

        /*开始播放*/
        private void StartPlay(PlayableDirector pd) {
            pd.Play();
        }

        /*停止播放*/
        private void StopPlay(PlayableDirector pd) {
            Flo.Instance.GetFlow(out Flow_Battle flow);
            flow.Play();
        }

        public override void Clear() {
            base.Clear();
            pd.stopped -= StopPlay;
        }
    }
}