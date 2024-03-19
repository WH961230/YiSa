using System.Collections.Generic;

namespace LazyPan {
    public partial class Data {
        public bool CanControl;//玩家是否可控
        public bool GameOver;//游戏结束
        public int LevelNum = 1;//关卡数量 1-n
        public bool SelectLevel;//选择关卡
        public bool StartNextLevel;//开始下一关卡
        public List<string> SelectRobots;//每局生成的机器人标识
        public bool SelectLevelUp;//选择角色升级

        public void Default() {
            LevelNum = 1;
            GameOver = false;
            SelectRobots = null;
            SelectLevel = false;
            StartNextLevel = false;
            CanControl = false;
        }
    }
}