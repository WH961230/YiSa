using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace LazyPan {
    public partial class Cond : Singleton<Cond> {
        #region 全局
        public Entity GetCameraEntity() { if (Data.Instance.TryGetEntityByType("Camera", out Entity entity)) { return entity; } else { return null; } }
        public Entity GetPlayerEntity() { if (Data.Instance.TryGetEntityByType("Player", out Entity entity)) { return entity; } else { return null; } }
        #endregion

        #region 根据标签获取组件|事件
        public T Get<T>(Entity entity, string label) where T : Object {
            if (entity == null) { return default; }

            if (entity.Comp == null) {
                Debug.LogErrorFormat("请检查 entity:{0} 没有挂 Comp 组件!", entity.ObjConfig.Sign);
                EditorApplication.isPaused = true;
            }
            return entity.Comp.Get<T>(label);
        }
        public T Get<T>(Comp comp, string label) where T : Object {
            if (comp == null) { return default; }
            return comp.Get<T>(label);
        }
        public UnityEvent Get(Entity entity, string label) {
            return entity.Comp.GetEvent(label);
        }
        public UnityEvent Get(Comp comp, string label) {
            return comp.GetEvent(label);
        }
        #endregion
    }
}