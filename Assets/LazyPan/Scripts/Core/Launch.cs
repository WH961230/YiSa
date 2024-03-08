using UnityEngine;

namespace LazyPan {
    public class Launch : MonoBehaviour {
        public static Launch instance;
        private void Start() {
            instance = this;
            Data.Instance.Setting = Loader.LoadSetting();
            Config.Instance.Init();
            Obj.Instance.Init();
            DontDestroyOnLoad(gameObject);

            //配置开始界面
            Game game = Loader.LoadGo("全局", "Global/Global", null, true).GetComponent<Game>();
            game.Init();
        }

        //加载阶段
        public void StageLoad(string sceneName) {
            Data.Instance.OnUpdateEvent.RemoveAllListeners();
            Data.Instance.OnFixedUpdateEvent.RemoveAllListeners();
            Data.Instance.OnLateUpdateEvent.RemoveAllListeners();

            Transform uiRoot = Loader.LoadGo("加载画布", "Global/Global_UIRoot", null, true).transform;
            Stage stage = uiRoot.gameObject.AddComponent<Stage>();
            DontDestroyOnLoad(uiRoot.gameObject);
            stage.Load(sceneName);
        }
    }
}