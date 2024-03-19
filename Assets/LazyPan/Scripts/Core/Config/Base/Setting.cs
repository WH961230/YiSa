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

        /*随机获取n个关卡数匹配的关卡机器人配置*/
        public bool TryGetLevelRobotSetting(int levelNum, int needNum, out List<LevelRobotSetting> ret) {
            ret = new List<LevelRobotSetting>();
            foreach (LevelRobotSetting setting in LevelRobotSettings) {
                if (setting.LevelNum == levelNum) {
                    ret.Add(setting);
                }
            }

            if (ret.Count < needNum) {
                ret = default;
                return false;
            }

            int[] indexs = MathUtil.Instance.GetRandNoRepeatIndex(ret.Count, needNum);
            ret.Clear();
            foreach (int index in indexs) {
                ret.Add(LevelRobotSettings[index]);
            }

            return true;
        }

        // /*随机获取n个关卡数匹配的关卡BUFF配置*/
        // public bool TryGetLevelBuffSetting(int levelNum, int needNum) {
        //     
        // }
    }
}