using System;
using System.Collections.Generic;
using UnityEngine;

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

    //关卡机器人配置
    [Serializable]
    public class LevelRobotSetting {
        [Tooltip("关卡数")] public int LevelNum;
        [Tooltip("描述")] public string Description;
        [Tooltip("图标")] public Sprite Icon;
        [Tooltip("怪物数量")] public int robotNum;
        [Tooltip("怪物标识")] public string robotSign;
        [Tooltip("怪物难度")] public int robotDifficulty;
    }

    //关卡BUFF配置
    [Serializable]
    public class LevelBuffSetting {
        [Tooltip("关卡数")] public int LevelNum;
        [Tooltip("描述")] public string Description;
        [Tooltip("图标")] public Sprite Icon;
        [Tooltip("行为标识")] public string BehaviourSign;
    }

    [CreateAssetMenu(menuName = "LazyPan/Setting", fileName = "Setting")]
    public class Setting : ScriptableObject {
        [Header("关卡配置")] public LevelSetting LevelSetting;
        [Header("玩家配置")] public PlayerSetting PlayerSetting;
        [Header("机器人配置")] public RobotSetting RobotSetting;
        [Header("Buff配置")] public BuffSetting BuffSetting;
        [Header("塔配置")] public TowerSetting TowerSetting;
        [Header("可激活Buff配置")] public ActivableSetting ActivableSetting;

        [Header("基础信息配置")] public List<BaseSetting> BaseSettings;
        [Header("关卡机器人配置")] public List<LevelRobotSetting> LevelRobotSettings;
        [Header("关卡BUFF配置")] public List<LevelBuffSetting> LevelBuffSettings;

        /*获取标识的基础信息配置*/
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

        /*获取n个不重复的机器人配置*/
        public bool TryGetRobotByCount(int num, out List<RobotSettingInfo> robotSettingInfo) {
            int[] index = MathUtil.Instance.GetRandNoRepeatIndex(RobotSetting.RobotSettingInfo.Count, 3);
            if (index != null) {
                robotSettingInfo = new List<RobotSettingInfo>();
                foreach (int i in index) {
                    robotSettingInfo.Add(RobotSetting.RobotSettingInfo[i]);
                }

                return true;
            }

            robotSettingInfo = default;
            return false;
        }

        /*获取机器人配置*/
        public bool TryGetRobotBySign(string sign, out RobotSettingInfo robotSettingInfo) {
            foreach (RobotSettingInfo info in RobotSetting.RobotSettingInfo) {
                if (info.Sign == sign) {
                    robotSettingInfo = info;
                    return true;
                }
            }

            robotSettingInfo = default;
            return false;
        }

        /*获取n个不重复的 BuFF 配置*/
        public bool TryGetBuffByCount(int num, out List<BuffSettingInfo> buffSettingInfo) {
            int[] index = MathUtil.Instance.GetRandNoRepeatIndex(BuffSetting.BuffSettingInfo.Count, 3);
            if (index != null) {
                buffSettingInfo = new List<BuffSettingInfo>();
                foreach (int i in index) {
                    buffSettingInfo.Add(BuffSetting.BuffSettingInfo[i]);
                }

                return true;
            }

            buffSettingInfo = default;
            return false;
        }
    }
}