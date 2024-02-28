using UnityEngine;

namespace LazyPan {
    public class Behaviour_FollowPlayer : Behaviour {
        private Vector3 Offset;
        private Transform FollowTr;
        public Behaviour_FollowPlayer(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnLateUpdateEvent.AddListener(OnFollowPlayerUpdate);
        }

        private void OnFollowPlayerUpdate() {
            if (FollowTr == null) {
                if (Data.Instance.TryGetEntityByObjType(ObjType.MainPlayer, out Entity playerEntity)) {
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

        public override void OnClear() {
            base.OnClear();
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnFollowPlayerUpdate);
        }
    }
}