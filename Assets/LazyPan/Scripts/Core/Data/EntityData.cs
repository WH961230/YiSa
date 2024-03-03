using UnityEngine;

namespace LazyPan {
    public class EntityData {
        public BaseRuntimeData BaseRuntimeData;
        public EntityData(string sign, Setting setting) {
            if (setting.TryGetBaseSetting(sign, out BaseSetting baseSetting)) {
                BaseRuntimeData = new BaseRuntimeData();
                BaseRuntimeData.Sign = sign;
                BaseRuntimeData.DefMotionSpeed = baseSetting.MotionSpeed;
                BaseRuntimeData.CurMotionSpeed = baseSetting.MotionSpeed;
                BaseRuntimeData.DefRotateSpeed = baseSetting.RotateSpeed;
                BaseRuntimeData.CurRotateSpeed = baseSetting.RotateSpeed;
                BaseRuntimeData.Type = ObjConfig.Get(sign).Type;
            }
        }
    }

    //运行数据
    public class BaseRuntimeData {
        /*基础参数*/
        public string Sign;//标识
        public string Type;//类型
        public string CameraType;//相机类型
        /*移动参数*/
        public float DefMotionSpeed;//默认移动速度
        public float CurMotionSpeed;//当前移动速度
        public Vector3 CurMotionDir;//移动方向
        /*旋转参数*/
        public float DefRotateSpeed;//默认旋转速度
        public float CurRotateSpeed;//当前旋转速度
    }
}