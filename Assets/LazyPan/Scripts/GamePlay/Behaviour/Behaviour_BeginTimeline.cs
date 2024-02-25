using UnityEngine.Playables;

namespace LazyPan {
    public class Behaviour_BeginTimeline : Behaviour {
        private PlayableDirector pd;
        public Behaviour_BeginTimeline(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            pd = entity.Comp.Get<PlayableDirector>("BeginPlayableDirector");
            pd.Play();
            pd.stopped += CheckStartControl;
        }

        private void CheckStartControl(PlayableDirector pd) {
            Data.Instance.CanControl = true;
        }

        public override void OnClear() {
            base.OnClear();
            pd.stopped -= CheckStartControl;
        }
    }
}