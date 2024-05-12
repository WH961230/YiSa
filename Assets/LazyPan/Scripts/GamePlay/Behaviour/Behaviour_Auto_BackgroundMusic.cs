using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_BackgroundMusic : Behaviour {
        private GameObject soundGo;
        public Behaviour_Auto_BackgroundMusic(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            PlayBackgroundMusic();
        }

        /*播放背景音乐*/
        private void PlayBackgroundMusic() {
            soundGo = Sound.Instance.SoundPlay("BackgroundMusic", Vector3.zero, true, 1, -1);
        }

        public override void Clear() {
            base.Clear();
            Sound.Instance.SoundRecycle(soundGo);
        }
    }
}