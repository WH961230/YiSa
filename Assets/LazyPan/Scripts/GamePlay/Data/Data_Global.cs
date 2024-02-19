using System;

namespace LazyPan {
    public partial class Data {
        public GameState GameState;
        public string SceneName;
    }

    [Serializable]
    public enum GameState {
        BEGIN,//开始流程
        FIGHT,//战斗流程
        CLEAR,//结束流程
    }
}