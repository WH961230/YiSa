namespace LazyPan {
    public class Label {

        //全局
        public static string CAMERA = "Camera";//相机
        public static string ANIMATOR = "Animator";//动画播放器
        public static string CHARACTERCONTROLLER = "CharacterController";//角色控制器
        public static string PLAYABLEDIRECTOR = "PlayableDirector";//时间轴播放器
        public static string TIMELINEASSET = "TimelineAsset";//时间轴资源
        public static string TRAILRENDERER = "TrailRenderer";//拖尾
        public static string EVENT = "Event";//事件

        //角色
        public static string BODY = "Body";//身体位置
        public static string AIMOFFSETPOINT = "Point";//修复射击偏移点位配置
        public static string MUZZLE = "Muzzle";//枪口

        //UI
        public static string CURSOR = "Cursor";//光标
        public static string QUIT = "Quit";//退出
        public static string BACK = "Back";//返回
        public static string NEXT = "Next";//下一步
        public static string TITLE = "Title";//标题
        public static string HOME = "Home";//标题
        public static string AGAIN = "Again";//再来

        //事件
        public static string SHOOTEVENT = "ShootEvent";//射击事件
        public static string MOTIONEVENT = "MotionEvent";//移动事件

        //移动类型
        public static string MOTION = "Motion";

        //攻击方式
        public static string CLOSEFIGHT = "CloseFight";//近战
        public static string SHOOT = "Shoot";
        public static string ANNOUNCEMENT = "Announcement";//公告

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
