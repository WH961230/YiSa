using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_FollowPlayer : Behaviour {
        private Transform FollowTr;
        public Behaviour_Auto_FollowPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnLateUpdateEvent.AddListener(OnFollowPlayerUpdate);
        }

        private void OnFollowPlayerUpdate() {
            if (FollowTr == null) {
                if (Cond.Instance.GetPlayerEntity() != null) {
                    FollowTr = Cond.Instance.Get<Transform>(Cond.Instance.GetPlayerEntity(), Label.BODY);
                }
            } else {
                entity.Prefab.transform.position = Vector3.Lerp(entity.Prefab.transform.position,
                    new Vector3(FollowTr.position.x, entity.Prefab.transform.position.y, FollowTr.position.z), Time.deltaTime * 2);
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnFollowPlayerUpdate);
        }
    }
}