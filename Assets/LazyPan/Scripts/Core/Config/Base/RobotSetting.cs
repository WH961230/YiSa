using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LazyPan {
    [CreateAssetMenu(menuName = "LazyPan/RobotSetting", fileName = "RobotSetting")]
    public class RobotSetting : ScriptableObject {
        public List<RobotSettingInfo> RobotSettingInfo;
    }

    [Serializable]
    public class RobotSettingInfo {
        [Tooltip("标识")] public string Sign;
        [Tooltip("图标")] public Sprite Icon;
        [Tooltip("描述")] public string Description;
        [Tooltip("攻击力")] public int Attack;
        [Tooltip("移动速度")] public float MovementSpeed;
        [Tooltip("最大血量")] public int MaxHealth;
        [Tooltip("被击退速度")] public float KnockbackSpeed;
        [Tooltip("被击退持续时间")] public float KnockbackDuraing;
    }
}