using UnityEngine.Playables;

namespace LazyPan {
    public class Behaviour_Event_BeginTimeline : Behaviour {
        private PlayableDirector pd;
        public Behaviour_Event_BeginTimeline(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            pd = Cond.Instance.Get<PlayableDirector>(entity, Label.PLAYABLEDIRECTOR);
            pd.stopped += StopPlay;
            /*开始播放*/
            StartPlay(pd);
            /*角色禁止控制*/
            SetPlayerControl(false);
        }

        /*设置角色控制*/
        private void SetPlayerControl(bool control) {
            Data.Instance.CanControl = control;
        }

        /*开始播放*/
        private void StartPlay(PlayableDirector pd) {
            pd.Play();
        }

        /*停止播放*/
        private void StopPlay(PlayableDirector pd) {
            SetPlayerControl(true);
            Flo.Instance.GetFlow(out Flow_Battle flow);
            flow.Play();
        }

        public override void Clear() {
            base.Clear();
            pd.stopped -= StopPlay;
        }
    }
}