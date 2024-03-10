using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LazyPan {
    public class Behaviour_Input_Teleport : Behaviour {
        private TimelineAsset asset;
        private PlayableDirector playableDirector;
        private TrailRenderer trailRenderer;
        private bool isPlay;

        public Behaviour_Input_Teleport(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            asset = Cond.Instance.Get<TimelineAsset>(entity,  Label.Assemble(Label.TELEPORT, Label.TIMELINEASSET));

            playableDirector = Cond.Instance.Get<PlayableDirector>(entity, Label.PLAYABLEDIRECTOR);
            trailRenderer = Cond.Instance.Get<TrailRenderer>(entity, Label.Assemble(Label.BODY, Label.TRAILRENDERER));
            playableDirector.played += playableDirector => { 
                isPlay = true;
                Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER).enabled = false;
                trailRenderer.Clear();
            };

            playableDirector.stopped += director => {
                isPlay = false;
                Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER).enabled = true;
            };

            InputRegister.Instance.Load(InputRegister.Instance.Space, Teleport);
        }

        private void Teleport(InputAction.CallbackContext obj) {
            if (isPlay) {
                return;
            }
            playableDirector.playableAsset = asset;
            playableDirector.Play();
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.Space, Teleport);
        }
    }
}