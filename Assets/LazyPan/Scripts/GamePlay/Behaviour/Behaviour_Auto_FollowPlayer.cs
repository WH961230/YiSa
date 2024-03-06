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
                if (Data.Instance.TryGetEntityByType("Player", out Entity playerEntity)) {
                    FollowTr = playerEntity.Comp.Get<Transform>("Body");
                    Offset = entity.Prefab.transform.position - FollowTr.position;
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