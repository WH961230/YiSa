using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LazyPan {
    public class Behaviour_InputCloseFight : Behaviour {
        private TimelineAsset asset;
        private PlayableDirector playableDirector;
        private bool isPlay;
        public Behaviour_InputCloseFight(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            asset = entity.Comp.Get<TimelineAsset>("CloseFightTimelineAsset");
            playableDirector = entity.Comp.Get<PlayableDirector>("CloseFightPlayableDirector");
            playableDirector.played += playableDirector => { isPlay = true; };  
            playableDirector.stopped += director => { isPlay = false; };  
            InputRegister.Instance.Load(InputRegister.Instance.LeftClick, CloseFight);
        }

        private void CloseFight(InputAction.CallbackContext obj) {
            if (isPlay) {
                return;
            }
            playableDirector.playableAsset = asset;
            playableDirector.Play();
        }

        public override void OnClear() {
            base.OnClear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.LeftClick, CloseFight);
        }
    }
}