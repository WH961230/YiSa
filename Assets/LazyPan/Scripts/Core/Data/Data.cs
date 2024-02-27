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

        public bool TryGetEntityByID(int id, out Entity entity) {
            if (EntityDic.TryGetValue(id, out entity)) {
                return true;
            }

            return false;
        }

        public bool TryGetEntityByObjType(ObjType type, out Entity entity) {
            foreach (Entity tempEntity in EntityDic.Values) {
                if (tempEntity.EntityData.ObjType == type) {
                    entity = tempEntity;
                    return true;
                }
            }

            entity = null;
            return false;
        }

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

        public bool TryGetDropSetting(Setting setting, string dropSign, out DropSetting dropSetting) {
            List<DropSetting> dropSettings = setting.DropSettings;

            foreach (DropSetting tempDropSetting in dropSettings) {
                if (tempDropSetting.Sign == dropSign) {
                    dropSetting = tempDropSetting;
                    return true;
                }
            }

            dropSetting = default;
            return false;
        }

        public bool TryGetLevelSetting(Setting setting, int level, out LevelSetting levelSetting) {
            List<LevelSetting> levelSettings = setting.LevelSettings;
            foreach (LevelSetting tempLevelSetting in levelSettings) {
                if (tempLevelSetting.Level == level) {
                    levelSetting = tempLevelSetting;
                    return true;
                }
            }

            levelSetting = default;
            return false;
        }

        public bool TryGetShakerSetting(Setting setting, string sign, out ShakerSetting shakerSetting) {
            List<ShakerSetting> settings = setting.ShakerSettings;
            foreach (ShakerSetting tempSetting in settings) {
                if (tempSetting.CameraShakeSign == sign) {
                    shakerSetting = tempSetting;
                    return true;
                }
            }

            shakerSetting = default;
            return false;
        }
    }
}