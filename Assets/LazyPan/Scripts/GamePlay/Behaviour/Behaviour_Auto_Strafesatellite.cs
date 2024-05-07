using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Strafesatellite : Behaviour {
        private float deploy;
        private int LaserNum;
        public Behaviour_Auto_Strafesatellite(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            /*扫射*/
            Data.Instance.OnUpdateEvent.AddListener(Strafesatellite);
        }

        /*扫射*/
        private void Strafesatellite() {
            
        }

        public override void Upgrade() {
            base.Upgrade();
            LaserNum++;
            if (LaserNum > 3) {
                LaserNum = 3;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(Strafesatellite);
        }
    }
}