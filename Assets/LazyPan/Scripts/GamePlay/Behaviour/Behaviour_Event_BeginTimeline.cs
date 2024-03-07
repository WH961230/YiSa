using UnityEngine.Playables;

namespace LazyPan {
    public class Behaviour_Event_BeginTimeline : Behaviour {
        private PlayableDirector pd;
        public Behaviour_Event_BeginTimeline(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            pd = Cond.Instance.Get<PlayableDirector>(entity, Label.PLAYABLEDIRECTOR);
            pd.Play();
            pd.stopped += CheckStartControl;
        }

        private void CheckStartControl(PlayableDirector pd) {
            Data.Instance.CanControl = true;
        }

        public override void Clear() {
            base.Clear();
            pd.stopped -= CheckStartControl;
        }
    }
}