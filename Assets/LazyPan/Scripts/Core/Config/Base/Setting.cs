using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LazyPan {

    //基础配置
    [Serializable]
    public class BaseSetting {
        [FormerlySerializedAs("Sign")]
        [Header("基本参数")]
        [Tooltip("标识")] public string CreatureType;

        [Header("移动参数")]
        [Tooltip("移动速度")] public float MotionSpeed;
        [Tooltip("旋转速度")] public float RotateSpeed;
        [Tooltip("重力速度")] public float GravitySpeed;
    }

    [CreateAssetMenu(menuName = "LazyPan/Setting", fileName = "Setting")]
    public class Setting : ScriptableObject {
        [Header("基础信息配置")] public List<BaseSetting> BaseSettings;

        public bool TryGetBaseSetting(string sign, out BaseSetting baseSetting) {
            foreach (BaseSetting setting in BaseSettings) {
                if (setting.CreatureType == sign) {
                    baseSetting = setting;
                    return true;
                }
            }

            baseSetting = default;
            return false;
        }
    }
}