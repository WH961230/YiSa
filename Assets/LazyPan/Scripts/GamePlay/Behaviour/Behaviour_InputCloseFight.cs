using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LazyPan {
    public class Behaviour_InputCloseFight : Behaviour {
        private TimelineAsset asset;
        private PlayableDirector playableDirector;
        private TrailRenderer swordTrail;
        private bool isPlay;
        public Behaviour_InputCloseFight(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            asset = entity.Comp.Get<TimelineAsset>("CloseFightTimelineAsset");
            playableDirector = entity.Comp.Get<PlayableDirector>("CloseFightPlayableDirector");
            swordTrail = entity.Comp.Get<TrailRenderer>("SwordTrail");
            swordTrail.enabled = false;
            playableDirector.played += playableDirector => {
                isPlay = true;
                swordTrail.enabled = true;
            };  
            playableDirector.stopped += director => {
                swordTrail.enabled = false;
                isPlay = false;
            };  
            //近战动画状态机
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