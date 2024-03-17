using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public partial class Cond {
        public bool GetTowerEntity(out Entity entity) { return Data.Instance.TryGetEntityByType("Building", out entity); }
        public bool GetRandEntityByType(string type, out Entity entity) { return Data.Instance.TryGetRandEntityByType(type, out entity); }

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
        public bool GetEntitiesByTypeWithinDistance(string type, Vector3 fromPoint, float distance, out List<Entity> entities) {
            return Data.Instance.TryGetEntitiesWithinDistance(type, fromPoint, distance, out entities);
        }
    }
}