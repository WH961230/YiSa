namespace LazyPan {
    public class EntityData {
        public BaseRuntimeData BaseRuntimeData;
        public EntityData(ObjConfig config) {
            BaseRuntimeData = new BaseRuntimeData();
            BaseRuntimeData.Sign = config.Sign;
            BaseRuntimeData.Type = config.Type;
            if (config.Type == "Robot") {
                BaseRuntimeData.RobotInfo = new RobotInfo();
                Loader.LoadSetting().TryGetRobotBySign(config.Sign, out RobotSettingInfo info);
                BaseRuntimeData.RobotInfo.HealthPoint = info.MaxHealth;
            } else if (config.Type == "Player") {
                BaseRuntimeData.PlayerInfo = new PlayerInfo();
                BaseRuntimeData.PlayerInfo.HealthPoint = Loader.LoadSetting().PlayerSetting.MaxHealth;
                BaseRuntimeData.PlayerInfo.Experience = 0;
            } else if (config.Type == "Tower") {
                BaseRuntimeData.TowerInfo = new TowerInfo();
                BaseRuntimeData.TowerInfo.Energy = Loader.LoadSetting().TowerSetting.MaxEnergy;
            }
        }

        public void Clear() {
            BaseRuntimeData.RobotInfo = null;
            BaseRuntimeData.PlayerInfo = null;
            BaseRuntimeData.TowerInfo = null;
        }
    }

    //运行数据
    public class BaseRuntimeData {
        /*基础参数*/
        public string Sign;//标识
        public string Type;//类型
        public PlayerInfo PlayerInfo;//玩家数据
        public RobotInfo RobotInfo;//机器人数据
        public TowerInfo TowerInfo;//塔数据
    }
}