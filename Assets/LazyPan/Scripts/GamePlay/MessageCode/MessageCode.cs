namespace LazyPan {
    public class MessageCode : Singleton<MessageCode> {
        public static int DamageEntity = 1;//伤害实体
        public static int RefreshEntityUI = 2;//刷新实体UI
        public static int DeathEntity = 3;//实体死亡
        public static int LevelUp = 4;//升级
        public static int MonsterEntityInit = 5;//怪兽参数初始化
        public static int GainExperience = 6;//获取经验值
        public static int ClearEntity = 7;//清除实体
    }
}