using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LazyPan {
    public class Behaviour_Input_CloseFight : Behaviour {
        private TimelineAsset asset;
        private PlayableDirector playableDirector;
        private TrailRenderer trailRenderer;
        private bool isPlay;

        public Behaviour_Input_CloseFight(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            asset = Cond.Instance.Get<TimelineAsset>(entity,  Label.Assemble(Label.CLOSEFIGHT, Label.TIMELINEASSET));
            playableDirector = Cond.Instance.Get<PlayableDirector>(entity, Label.PLAYABLEDIRECTOR);
            trailRenderer = Cond.Instance.Get<TrailRenderer>(entity, Label.TRAILRENDERER);
            playableDirector.played += playableDirector => { isPlay = true; trailRenderer.Clear();};
            playableDirector.stopped += director => { isPlay = false; };
            InputRegister.Instance.Load(InputRegister.Instance.Space, CloseFight);
        }

        private void CloseFight(InputAction.CallbackContext obj) {
            if (isPlay) {
                return;
            }
            playableDirector.playableAsset = asset;
            playableDirector.Play();
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Space, CloseFight);
        }
    }
}