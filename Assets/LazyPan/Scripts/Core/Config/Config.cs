namespace LazyPan {
    public class Config : Singleton<Config> {
        public void Init() {
            SceneConfig.Init();
            ObjConfig.Init();
            BehaviourConfig.Init();
            BuffConfig.Init();
        }
    }
}