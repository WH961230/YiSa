using System.Collections.Generic;

namespace LazyPan {
    public partial class Data {
        public GlobalInfo GlobalInfo = new GlobalInfo();//全局
        public List<BuffInfo> BuffInfo = new List<BuffInfo>();//Buff
    }

    /*全局*/
    public class GlobalInfo {
        public bool IsGameStart;//是否游戏开始
        public bool IsGamePause;//是否游戏暂停
        public bool IsGameOver;//是否游戏结束
        public bool AllowMovement;//允许移动//可移动
        public int Level = 1;//关卡数量
        public int RobotLevel = 1;//怪物难度
        public int OwnLevel = 1;//己方等级
    }

    /*玩家*/
    public class PlayerInfo {
        public float HealthPoint;//血量
        /*经验值*/
        public float Experience;//经验值
    }

    /*机器人*/
    public class RobotInfo {
        //种类
        public float HealthPoint;//血量
        public bool IsDead => HealthPoint == 0;//死亡
        public float SlowTime;//被减速时间
        public int DeathDropType;//死亡掉落类型 0 无 1 经验值 2 可激活
        public int BeAttackType;//死亡类型 1 敌方 2 自杀
    }

    /*塔*/
    public class TowerInfo {
        //武器
        public float Energy;//当前能量
    }

    /*BUFF*/
    public class BuffInfo {
        public bool Disable;//禁用
        public int Level;//当前升级 无法升级的 0 可以升级的 等级数
        public BuffSettingInfo Setting;
    }

    /*激活*/
    public class ActivableInfo {
        
    }
}