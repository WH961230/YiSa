﻿using UnityEngine;

namespace LazyPan {
    public class EntityData {
        public BaseRuntimeData BaseRuntimeData;
        public EntityData(ObjConfig config, Setting setting) {
            BaseRuntimeData = new BaseRuntimeData();
            BaseRuntimeData.Sign = config.Sign;
            BaseRuntimeData.Type = config.Type;
            if (setting.TryGetBaseSetting(config.CreatureType, out BaseSetting baseSetting)) {
                /*基参*/
                BaseRuntimeData.DefHealth = baseSetting.Health;
                BaseRuntimeData.CurHealth = BaseRuntimeData.DefHealth;
                BaseRuntimeData.CurAttack = baseSetting.Attack;
                BaseRuntimeData.DefAttackIntervalTime = baseSetting.AttackIntervalTime;
                BaseRuntimeData.CurAttackIntervalDeployTime = BaseRuntimeData.DefAttackIntervalTime;
                BaseRuntimeData.DefTeleportSpeed = baseSetting.TeleportSpeed;
                BaseRuntimeData.DefTeleportTime = baseSetting.TeleportTime;
                BaseRuntimeData.DefTeleportColdTime = baseSetting.TeleportColdTime;
                BaseRuntimeData.DefKnockbackSpeed = baseSetting.KnockbackSpeed;
                BaseRuntimeData.DefKnockbackTime = baseSetting.KnockbackTime;
                BaseRuntimeData.DefMotionSpeed = baseSetting.MotionSpeed;
                BaseRuntimeData.CurMotionSpeed = BaseRuntimeData.DefMotionSpeed;
                BaseRuntimeData.DefRotateSpeed = baseSetting.RotateSpeed;
                BaseRuntimeData.CurRotateSpeed = BaseRuntimeData.DefRotateSpeed;
                BaseRuntimeData.DefDetectDistance = baseSetting.DetectDistance;
                BaseRuntimeData.CurDetectDistance = BaseRuntimeData.DefDetectDistance;
                BaseRuntimeData.DefDetectFrequency = baseSetting.DetectFrequency;
                BaseRuntimeData.GravitySpeed = baseSetting.GravitySpeed;
            }
        }
    }

    //运行数据
    public class BaseRuntimeData {
        /*基础参数*/
        public string Sign;//标识
        public string Type;//类型
        public int DefHealth;//默认血量
        public int CurHealth;//当前血量
        public int DefEnergy;//默认能量
        public int DefMaxEnergy;//默认最大能量
        public float DefChargeEnergySpeed;//默认能量补充速度
        public int CurEnergy;//当前能量
        public int CurMaxEnergy;//当前最大能量
        public int CurChargeEnergySpeed;//当前能量补充速度
        /*移动参数*/
        public int CurMotionState;//默认 0 禁止 1 移动 2 冲刺
        public float DefMotionSpeed;//默认移动速度
        public float CurMotionSpeed;//当前移动速度
        public Vector3 CurMotionDir;//移动方向
        /*冲刺*/
        public float DefTeleportSpeed;//默认冲刺速度
        public Vector3 CurTeleportDir;//当前冲刺方向
        /*击退*/
        public float DefKnockbackSpeed;//默认击退速度
        public float DefKnockbackTime;//默认击退的持续时间
        public float CurKnockbackDeployTime;//默认击退的持续雇佣时间
        public Vector3 CurKnockbackDir;//当前击退方向
        /*旋转参数*/
        public float DefRotateSpeed;//默认旋转速度
        public float CurRotateSpeed;//当前旋转速度
        public Vector3 CurRotateDir;//旋转方向
        /*重力参数*/
        public Vector3 CurGravityDir;//重力方向
        public float GravitySpeed;//重力速度
        public float DefTeleportTime;//默认传送的持续时间
        public float CurTeleportDeployTime;//传送的雇佣时间
        public float DefTeleportColdTime;//默认传送冷却时间
        public float CurTeleportColdDeployTime;//传送的冷却雇佣时间
        /*攻击参数*/
        public int CurAttack;//当前伤害
        public float DefAttackIntervalTime;//默认攻击间隔时间
        public float CurAttackIntervalDeployTime;//当前攻击间隔时间
        public float DefDetectDistance;//默认检测距离
        public float CurDetectDistance;//当前检测距离
        public float DefDetectFrequency;//默认检频测率
        public float CurDetectFrequencyDeployTime;//当前检测频率
        public int CurAttackTargetEntityID;//当前攻击的实体ID
    }
}