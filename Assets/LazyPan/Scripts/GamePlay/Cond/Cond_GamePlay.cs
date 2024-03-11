namespace LazyPan {
    public partial class Cond {
        public Entity GetTypeRandEntity(string type) { if (Data.Instance.TryGetRandEntityByEntities(type, out Entity entity)) { return entity; } else { return null; } }
    }
}