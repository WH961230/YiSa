namespace LazyPan {
    public class Label {
        public static string CAMERA = "Camera";//相机
        public static string ANIMATOR = "Animator";//动画播放器
        public static string CHARACTERCONTROLLER = "CharacterController";//角色控制器
        public static string PLAYABLEDIRECTOR = "PlayableDirector";//时间轴播放器
        public static string TIMELINEASSET = "TimelineAsset";//时间轴资源
        public static string TRAILRENDERER = "TrailRenderer";//拖尾
        public static string EVENT = "Event";//事件
        public static string TRIGGER = "Trigger";//触发器
        public static string COLLIDER = "Collider";//碰撞体
        public static string MATERIAL = "Material";//材质
        public static string RENDERER = "Renderer";//渲染
        public static string COMP = "Comp";//组件
        public static string PLAYER = "Player";//玩家
        public static string ROBOT = "Robot";//机器人
        public static string BODY = "Body";//身体位置
        public static string FOOT = "Foot";//脚
        public static string AIMOFFSETPOINT = "Point";//修复射击偏移点位配置
        public static string MUZZLE = "Muzzle";//枪口
        public static string BARREL = "Barrel";
        public static string HIT = "Hit";//
        public static string SQUIRT = "Squirt";//喷血口
        public static string ENERGY = "Energy";//能量
        public static string HEALTH = "Health";//血量
        public static string RANGE = "Range";//范围
        public static string EXP = "Exp";//经验值
        public static string CURSOR = "Cursor";//光标
        public static string QUIT = "Quit";//退出
        public static string BACK = "Back";//返回
        public static string NEXT = "Next";//下一步
        public static string TITLE = "Title";//标题
        public static string HOME = "Home";//标题
        public static string AGAIN = "Again";//再来
        public static string ANNOUNCEMENT = "Announcement";//公告
        public static string INFO = "Info";//信息
        public static string SETTLEMENT = "Settlement";//结算
        public static string RETURN = "Return";//返回
        public static string LEVEL = "Level";//关卡
        public static string LEVELUP = "LevelUp";//等级
        public static string SELECT = "Select";//选择
        public static string MOTION = "Motion";
        public static string TELEPORT = "Teleport";//传送
        public static string CLOSEFIGHT = "CloseFight";//近战
        public static string SHOOT = "Shoot";//射击
        public static string SWORD = "Sword";//剑
        public static string GUN = "Gun";//枪
        public static string CODE = "Code";//命令
        public static string CONTENT = "Content";//内容
        public static string A = "A";
        public static string B = "B";
        public static string C = "C";
        public static string SYSTEM = "System";
        public static string OWN = "Own";
        public static string LINE = "Line";

        //组合A+B
        public static string Assemble(string labelA, string labelB) {
            return string.Concat(labelA, labelB);
        }

        //组合A+B+C
        public static string Assemble(string labelA, string labelB, string labelC) {
            return string.Concat(labelA, labelB, labelC);
        }
    }
}
