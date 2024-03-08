using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LazyPan {
    public partial class Data : Singleton<Data> {
        public UnityEvent OnUpdateEvent = new UnityEvent();
        public UnityEvent OnFixedUpdateEvent = new UnityEvent();
        public UnityEvent OnLateUpdateEvent = new UnityEvent();

        public int EntityID;//为实体分配的ID
        public Dictionary<int, Entity> EntityDic = new Dictionary<int, Entity>();
        public Dictionary<int, List<Behaviour>> BehaviourDic = new Dictionary<int, List<Behaviour>>();

        public bool AddEntity(int id, Entity entity) {
            if (EntityDic.TryAdd(id, entity)) {
                Debug.LogFormat("ID:{0} 注册实体:{1}", id, entity.ObjConfig.Sign);
                return true;
            }

            return false;
        }

        public void RemoveEntity(int id) {
            if (EntityDic.ContainsKey(id)) {
                Debug.LogFormat("ID:{0} 移除实体:{1}", id, EntityDic[id].ObjConfig.Sign);
                EntityDic.Remove(id);
            }
        }

        //通过 标识 查找实体
        public bool TryGetEntityBySign(string objSign, out Entity entity) {
            foreach (Entity tmpEntity in EntityDic.Values) {
                if (objSign == tmpEntity.ObjConfig.Sign) {
                    entity = tmpEntity;
                    return true;
                }
            }

            entity = default;
            return false;
        }

        //通过 ID 查找实体
        public bool TryGetEntityByID(int id, out Entity entity) {
            if (EntityDic.TryGetValue(id, out entity)) {
                return true;
            }

            return false;
        }

        //通过 类型 查找实体
        public bool TryGetEntityByType(string type, out Entity entity) {
            foreach (Entity tmpEntity in EntityDic.Values) {
                if (tmpEntity.EntityData.BaseRuntimeData != null && type == tmpEntity.EntityData.BaseRuntimeData.Type) {
                    entity = tmpEntity;
                    return true;
                }
            }

            entity = default;
            return false;
        }

        //通过组件查找实体
        public bool TryGetEntityByComp(Comp comp, out Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                if (tempEntity.Comp == comp) {
                    entity = tempEntity;
                    return true;
                }
            }

            entity = null;
            return false;
        }

        //通过实体查找实体
        public bool HasEntity(Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                if (tempEntity == entity) {
                    return true;
                }
            }

            return false;
        }
    }
}