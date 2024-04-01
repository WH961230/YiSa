using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Gravity : Behaviour {
        public Behaviour_Auto_Gravity(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnLateUpdateEvent.AddListener(Gravity);
        }

        /*重力*/
        private void Gravity() {
            Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER).Move(
                Vector3.down * Time.deltaTime * GetGravitySpeed());
        }

        /*获取重力速度*/
        private float GetGravitySpeed() {
            return Loader.LoadSetting().PlayerSetting.GravitySpeed;
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnLateUpdateEvent.RemoveListener(Gravity);
        }
    }
}