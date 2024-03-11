using UnityEngine;

namespace LazyPan {
    public class EntityData {
        public BaseRuntimeData BaseRuntimeData;
        public EntityData(ObjConfig config, Setting setting) {
            BaseRuntimeData = new BaseRuntimeData();
            BaseRuntimeData.Sign = config.Sign;
            BaseRuntimeData.Type = config.Type;
            if (setting.TryGetBaseSetting(config.CreatureType, out BaseSetting baseSetting)) {
                BaseRuntimeData.DefTeleportSpeed = baseSetting.TeleportSpeed;
                BaseRuntimeData.DefTeleportTime = baseSetting.TeleportTime;
                BaseRuntimeData.DefTeleportColdTime = baseSetting.TeleportColdTime;
                BaseRuntimeData.DefMotionSpeed = baseSetting.MotionSpeed;
                BaseRuntimeData.CurMotionSpeed = BaseRuntimeData.DefMotionSpeed;
                BaseRuntimeData.DefRotateSpeed = baseSetting.RotateSpeed;
                BaseRuntimeData.CurRotateSpeed = BaseRuntimeData.DefRotateSpeed;
                BaseRuntimeData.GravitySpeed = baseSetting.GravitySpeed;
            }
        }
    }

    //运行数据
    public class BaseRuntimeData {
        /*基础参数*/
        public string Sign;//标识
        public string Type;//类型
        /*移动参数*/
        public int CurMotionState;//默认 0 禁止 1 移动 2 冲刺
        public float DefMotionSpeed;//默认移动速度
        public float CurMotionSpeed;//当前移动速度
        public Vector3 CurMotionDir;//移动方向
        /*冲刺*/
        public float DefTeleportSpeed;//默认冲刺速度
        public Vector3 CurTeleportDir;//当前冲刺方向
        /*旋转参数*/
        public float DefRotateSpeed;//默认旋转速度
        public float CurRotateSpeed;//当前旋转速度
        public Vector3 CurRotateDir;//旋转方向
        /*重力参数*/
        public Vector3 CurGravityDir;//重力方向
        public float GravitySpeed;//重力速度
        public float DefTeleportTime;//默认传送的持续时间
        public float DefTeleportColdTime;//默认传送冷却时间
    }
}