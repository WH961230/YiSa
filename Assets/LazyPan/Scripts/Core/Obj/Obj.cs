namespace LazyPan {
    public class Obj : Singleton<Obj> {
        public void Init() {
            
        }

        public void Preload() {
            Data.Instance.ObjRoot = Loader.LoadGo("物体", "Global/Global_Obj_Root", null, true).transform;
        }

        //加载物体
        public Entity LoadEntity(string sign) {
            if (!Data.Instance.TryGetEntityBySign(sign, out Entity instanceEntity)) {
                instanceEntity = new Entity();
                instanceEntity.Init(sign);
            }
            return instanceEntity;
        }

        //销毁实体
        public void UnLoadEntity(Entity entity) {
            if (Data.Instance.HasEntity(entity)) {
                entity.Clear();
            }
        }
    }
}