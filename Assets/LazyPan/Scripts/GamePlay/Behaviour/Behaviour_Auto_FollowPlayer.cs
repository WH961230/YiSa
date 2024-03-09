using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_FollowPlayer : Behaviour {
        private Vector3 Offset;
        private Transform FollowTr;
        public Behaviour_Auto_FollowPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnLateUpdateEvent.AddListener(OnFollowPlayerUpdate);
        }

        private void OnFollowPlayerUpdate() {
            if (FollowTr == null) {
                if (Cond.Instance.GetPlayerEntity() != null) {
                    Offset = entity.Prefab.transform.position;
                    FollowTr = Cond.Instance.Get<Transform>(Cond.Instance.GetPlayerEntity(), Label.BODY);
                    entity.Prefab.transform.position = FollowTr.position + Offset;
                }
            } else {
                if (Offset != Vector3.zero) {
                    entity.Prefab.transform.position = Vector3.Lerp(entity.Prefab.transform.position,
                        FollowTr.position + Offset, Time.deltaTime * 5);
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnFollowPlayerUpdate);
        }
    }
}