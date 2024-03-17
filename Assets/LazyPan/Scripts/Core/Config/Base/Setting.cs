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
        [Tooltip("血量")] public float Health;
        [Tooltip("血量上限")] public float HealthMax;
        [Tooltip("血量恢复速度")] public float HealthSpeed;

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
        [Tooltip("攻击力")] public int Attack;
        [Tooltip("攻击间隔时间")] public float AttackIntervalTime;

        [Header("击退")]
        [Tooltip("击退速度")] public float KnockbackSpeed;
        [Tooltip("击退时间")] public float KnockbackTime;

        [Header("检测")]
        [Tooltip("检测距离")] public float DetectDistance;
        [Tooltip("检测频率")] public float DetectFrequency;

        [Header("充能")]
        [Tooltip("默认能量最大值")] public int EnergyMax;
        [Tooltip("默认能量值")] public int Energy;
        [Tooltip("默认每秒补充能量速度")] public int ChargeEnergySpeed;
        [Tooltip("默认每秒能量掉落速度")] public int EnergyDownSpeed;

        [Header("范围")]
        [Tooltip("默认范围旋转角度")] public float RangeRotateAngle;

        [Header("经验值")]
        [Tooltip("经验值上限")] public int ExpMax;
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