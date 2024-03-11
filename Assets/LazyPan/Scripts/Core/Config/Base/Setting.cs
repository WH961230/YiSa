using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LazyPan {

    //基础配置
    [Serializable]
    public class BaseSetting {
        [Header("基本参数")]
        [Tooltip("标识")] public string CreatureType;
        [Tooltip("血量")] public int Health;

        [Header("移动参数")]
        [Tooltip("移动速度")] public float MotionSpeed;
        
        [Header("旋转参数")]
        [Tooltip("旋转速度")] public float RotateSpeed;
        
        [Header("冲刺参数")]
        [Tooltip("冲刺速度")] public float TeleportSpeed;
        [Tooltip("冲刺时间")] public float TeleportTime;
        [Tooltip("冲刺冷却时间")] public float TeleportColdTime;
        
        [Header("重力参数")]
        [Tooltip("重力速度")] public float GravitySpeed;

        [Header("攻击参数")]
        [Tooltip("攻击间隔时间")] public float AttackIntervalTime;
        [Tooltip("击退速度")] public float KnockbackSpeed;
        [Tooltip("击退时间")] public float KnockbackTime;
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