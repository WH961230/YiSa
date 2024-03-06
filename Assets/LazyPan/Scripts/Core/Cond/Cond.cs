using UnityEngine;

namespace LazyPan {
    public class Cond : Singleton<Cond> {
        #region Camera

        public Entity GetCameraEntity() {
            if (Data.Instance.TryGetEntityByType("MainCamera", out Entity cameraEntity)) { return cameraEntity; }
            return null;
        }

        public Camera GetCamera() {
            return GetCameraEntity()?.Comp.Get<Camera>("Camera");
        }

        #endregion

        #region Player
        
        public Entity GetPlayerEntity() {
            if (Data.Instance.TryGetEntityByType("Player", out Entity entity)) { return entity; }
            return null;
        }

        #endregion
    }
}