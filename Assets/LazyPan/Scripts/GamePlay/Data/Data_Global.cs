using UnityEngine;

namespace LazyPan {
    public partial class Data {
        //首次游玩
        public bool FirstPlay;
        //根节点
        public Transform UIRoot;
        public Transform ObjRoot;
        //配置
        public Setting Setting;
        //玩家是否可控
        public bool CanControl;
    }
}