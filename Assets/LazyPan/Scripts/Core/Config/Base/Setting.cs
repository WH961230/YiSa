using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {

    //基础配置
    [Serializable]
    public class BaseSetting {
        [Header("基本参数")]
        [Tooltip("标识")] public string Sign;

        [Header("移动参数")]
        [Tooltip("移动速度")] public float MotionSpeed;
        [Tooltip("旋转速度")] public float RotateSpeed;
    }

    //生成点配置
    [Serializable]
    public class BaseCreatorPointsSetting {
        public List<Vector3> CreatorPoints;
    }

    [CreateAssetMenu(menuName = "LazyPan/Setting", fileName = "Setting")]
    public class Setting : ScriptableObject {
        [Header("基础信息配置")] public List<BaseSetting> BaseSettings;
        [Header("基础生成点配置")] public BaseCreatorPointsSetting BaseCreatorPointsSetting;

        public bool TryGetBaseSetting(string sign, out BaseSetting baseSetting) {
            foreach (BaseSetting setting in BaseSettings) {
                if (setting.Sign == sign) {
                    baseSetting = setting;
                    return true;
                }
            }

            baseSetting = default;
            return false;
        }
    }
}