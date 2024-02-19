using UnityEngine;

namespace LazyPan {
    public class Behaviour_FollowTarget : Behaviour {
        private Entity playerEntity;
        private Transform playerFollowTran;
        private Quaternion cameraDefaultQua;
        private Transform cameraShakeRootTran;
        public Behaviour_FollowTarget(Entity entity, string sign) : base(entity, sign) {
            if (Data.Instance.TryGetEntityByObjType(ObjType.MainPlayer, out playerEntity)) {
                playerFollowTran = playerEntity.Prefab.transform;
            }

            cameraShakeRootTran = GameObject.FindWithTag("CameraShakeRoot").transform;
            Data.Instance.OnLateUpdateEvent.AddListener(OnFollowTargetLateUpdate);
            entity.Prefab.transform.rotation = Quaternion.Euler(new Vector3(60, -180, 0));
            cameraDefaultQua = entity.Prefab.transform.rotation;
        }

        private void OnFollowTargetLateUpdate() {
            if (playerFollowTran == null) {
                return;
            }

            entity.Prefab.transform.position = Vector3.Lerp(entity.Prefab.transform.position, cameraShakeRootTran.position + playerFollowTran.position + entity.EntitySetting.CameraFollowOffset, Time.deltaTime * entity.EntitySetting.CameraFollowPositionSpeed);
            entity.Prefab.transform.rotation = Quaternion.Lerp(entity.Prefab.transform.rotation, cameraShakeRootTran.rotation * cameraDefaultQua, Time.deltaTime * entity.EntitySetting.CameraFollowRotationSpeed);
        }

        public override void OnClear() {
            base.OnClear();
            playerEntity = null;
            playerFollowTran = null;
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnFollowTargetLateUpdate);
        }
    }
}