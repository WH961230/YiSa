using UnityEngine;

namespace LazyPan {
    public partial class Data {
        public GlobalInfo GlobalInfo;//全局
        public LevelInfo LevelInfo;//关卡
    }

    /*全局*/
    public class GlobalInfo {
        public bool IsGameStart;//是否游戏开始
        public bool IsGamePause;//是否游戏暂停
        public bool IsGameOver;//是否游戏结束
    }

    /*关卡*/
    public class LevelInfo {
        public int Level;//关卡数
    }

    /*玩家*/
    public class PlayerInfo {
        public int HealthPoint;//血量
        //经验
        //可移动
        public bool AllowMovement;//允许移动
    }

    /*机器人*/
    public class RobotInfo {
        //种类
        public int HealthPoint;//血量
        public int DeathDropType;//死亡掉落类型 0 无 1 经验值 2 可激活
        public int DeathType;//死亡类型 0 敌方攻击 1 自杀
    }

    /*塔*/
    public class TowerInfo {
        //武器
    }

    /*BUFF*/
    public class BuffInfo {
        
    }

    /*激活*/
    public class ActivableInfo {
        
    }
}