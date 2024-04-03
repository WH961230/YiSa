using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace LazyPan {
    public partial class Cond : Singleton<Cond> {
        #region 全局

        public Entity GetCameraEntity() {
            if (Data.Instance.TryGetEntityByType("Camera", out Entity entity)) {
                return entity;
            } else {
                return null;
            }
        }

        public Entity GetPlayerEntity() {
            if (Data.Instance.TryGetEntityByType("Player", out Entity entity)) {
                return entity;
            } else {
                return null;
            }
        }

        public bool GetTowerEntity(out Entity entity) {
            return Data.Instance.TryGetEntityByType("Tower", out entity);
        }

        public bool GetRandEntityByType(string type, out Entity entity) {
            return Data.Instance.TryGetRandEntityByType(type, out entity);
        }

        public bool GetEntityByID(int id, out Entity entity) {
            return Data.Instance.TryGetEntityByID(id, out entity);
        }

        #endregion

        #region 根据标签获取组件|事件

        public T Get<T>(Entity entity, string label) where T : Object {
            if (entity == null) {
                return default;
            }
#if UNITY_EDITOR
            if (entity.Comp == null) {
                Debug.LogErrorFormat("请检查 entity:{0} 没有挂 Comp 组件!", entity.ObjConfig.Sign);
                EditorApplication.isPaused = true;
            }
#endif
            return entity.Comp.Get<T>(label);
        }

        public T Get<T>(Comp comp, string label) where T : Object {
            if (comp == null) {
                return default;
            }

            return comp.Get<T>(label);
        }

        public UnityEvent Get(Entity entity, string label) {
            return entity.Comp.GetEvent(label);
        }

        public UnityEvent Get(Comp comp, string label) {
            return comp.GetEvent(label);
        }

        #endregion

        #region 根据距离获取实体

        //根据类型获取距离内的随机一个实体
        public bool GetRandEntityByTypeWithinDistance(string type, Vector3 fromPoint, float distance,
            out Entity entity) {
            if (Data.Instance.TryGetEntitiesWithinDistance(type, fromPoint, distance, out List<Entity> entities)) {
                entity = Data.Instance.GetRandEntity(entities);
                return true;
            }

            entity = default;
            return false;
        }

        //根据类型获取距离内的所有实体
        public bool GetEntitiesByTypeWithinDistance(string type, Vector3 fromPoint, float distance,
            out List<Entity> entities) {
            return Data.Instance.TryGetEntitiesWithinDistance(type, fromPoint, distance, out entities);
        }

        #endregion
    }
}