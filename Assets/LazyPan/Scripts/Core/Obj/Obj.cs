namespace LazyPan {
    public class Obj : Singleton<Obj> {
        public void Init() {
            Data.Instance.ObjRoot = Loader.LoadGo("物体", "Global/Global_Obj_Root", null, true).transform;
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
    }
}