using UnityEngine;

namespace LazyPan {
    public class Behaviour_AutoCloseFight : Behaviour {
        public Behaviour_AutoCloseFight(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
        }

        public override void OnClear() {
            base.OnClear();
        }
    }
}