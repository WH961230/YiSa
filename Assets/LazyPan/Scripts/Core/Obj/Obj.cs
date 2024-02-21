using UnityEngine;

namespace LazyPan {
    public class Obj : Singleton<Obj> {
        public Transform ObjRoot;
        public Transform CameraRoot;
        public Transform LightRoot;
        public Transform VolumeRoot;
        public Transform TerrainRoot;

        public void Init() {
            ObjRoot = new GameObject("物体").transform;
            CameraRoot = new GameObject("相机").transform;
            LightRoot = new GameObject("灯光").transform;
            VolumeRoot = new GameObject("后处理").transform;
            TerrainRoot = new GameObject("地形").transform;
        }

        //加载物体
        public Entity LoadEntity(string sign) {
            Entity instanceEntity = new Entity();
            instanceEntity.OnInit(sign);
            return instanceEntity;
        }

        //销毁实体
        public void UnLoadEntity(Entity entity) {
            if (Data.Instance.EntityDic.ContainsKey(entity.ID)) {
                Data.Instance.EntityDic.Remove(entity.ID);
                entity.OnClear();
            }
        }

        public Transform GetRoot(string RootSign) {
            if (RootSign == "ObjRoot") { return ObjRoot; }
            if (RootSign == "TerrainRoot") { return TerrainRoot; }
            if (RootSign == "CameraRoot") { return CameraRoot; }
            if (RootSign == "VolumeRoot") { return VolumeRoot; }
            if (RootSign == "LightRoot") { return LightRoot; }
            return null;
        }
    }
}