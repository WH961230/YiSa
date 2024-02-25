using System;
using System.Collections.Generic;
using MilkShake;
using UnityEngine;

namespace LazyPan {

    //基础配置
    [Serializable]
    public class BaseSetting {
        [Header("标识")] public string Sign;
        [Header("类型")] public ObjType ObjType;
        [Header("描述")] public string Detail;
    }

    //基础配置
    [Serializable]
    public class MotionRotateSetting {
        [Header("标识")] public string Sign;
        [Header("初始移动速度")] public float MovementSpeed;
        [Header("初始转身速度")] public float RotateSpeed;
    }

    //震动配置
    [Serializable]
    public class ShakerSetting {
        [Header("相机震动标识")] public string CameraShakeSign;
        [Header("相机震动配置")] public ShakePreset CameraShakePreset;
    }

    //生成器配置
    [Serializable]
    public class CreatorSetting {
        [Header("生成器物体标识")] public string CreatorObjSign;
        [Header("生成器物体延时时间")] public float CreatorObjDelayTime;
        [Header("生成器物体间隔")] public Vector2 CreatorObjIntervalTime;
        [Header("生成器物体距离")] public float CreatorObjDistance;
    }

    //等级配置
    [Serializable]
    public class LevelSetting {
        [Header("等级")] public int Level;
        [Header("经验值")] public int ExperienceMax;
    }

    //霸服配置
    [Serializable]
    public class BuffSetting {
        [Header("标识")] public string Sign;
        [Header("图标")] public Sprite Icon;
        [Header("描述")] public string Detail;
        [Header("参数")] public List<string> ParamStrs;
    }

    //掉落物配置
    [Serializable]
    public class DropSetting {
        [Header("标识")] public string Sign;
        [Header("掉落物预制体")] public GameObject DropPrefab;
        [Header("掉落力")] public float DropForce;
    }

    //攻击配置
    [Serializable]
    public class AttackSetting {
        [Header("基础攻击力")] public int AttackBase;
        [Header("基础攻击力倍率")] public float AttackRatio;
        [Header("暴击倍率")] public float AttackExtraRatio;
    }

    [CreateAssetMenu(menuName = "LazyPan/Setting", fileName = "Setting")]
    public class Setting : ScriptableObject {
        [Header("基础信息配置")] public List<BaseSetting> BaseSettings;
        [Header("移动与旋转配置")] public List<MotionRotateSetting> MotionRotateSettings;
        [Header("初始血量上限")] public int HealthMax;
        [Header("初始攻击间隔")] public float AttackInterval;
        [Header("初始换弹间隔")] public float ReloadInterval;
        [Header("初始等级")] public int Level;//等级
        [Header("初始经验值")] public int Experience;//经验值
        [Header("攻击配置")] public AttackSetting AttackSetting;//攻击配置
        [Header("经验值设置")] public List<LevelSetting> LevelSettings;
        [Header("初始弹药数量")] public int BulletNum;
        [Header("初始弹药数量上限")] public int BulletMaxNum;
        [Header("弹药特效预制体")] public GameObject BulletFx;
        [Header("相机震动设置配置")] public List<ShakerSetting> ShakerSettings;
        [Header("相机跟随偏移")] public Vector3 CameraFollowOffset;
        [Header("相机位置跟随速度")] public float CameraFollowPositionSpeed;
        [Header("相机角度跟随速度")] public float CameraFollowRotationSpeed;
        [Header("伤害特效")] public GameObject InjuredHealthFloatingGo;
        [Header("恢复特效")] public GameObject RecoverHealthFloatingGo;
        [Header("重力偏移角度")] public float OverlapCapsuleOffset;
        [Header("重力速度")] public float GravitySpeed;
        [Header("重力检测层级")] public LayerMask GravityDetectMaskLayer;
        [Header("脚步特效")] public GameObject FootStepFx;
        [Header("武器类型")] public int WeaponType;
        [Header("箭头标记图标")] public Sprite ArrowMarkSprite;
        [Header("机器人攻击最小距离")] public float RobotAttackDistance;
        [Header("怪物移动速度")] public Vector2 RobotMovementSpeed;
        [Header("怪物移动转向")] public Vector2 RobotRotateSpeed;
        [Header("怪物死亡布娃娃力")] public float RobotDeadRagdollForce;
        [Header("血数值飙出特效")] public GameObject BloodFloatingGo;
        [Header("生成器配置")] public CreatorSetting CreatorSetting;
        [Header("霸服配置")] public List<BuffSetting> BuffSettings;
        [Header("掉落物配置")] public List<DropSetting> DropSettings;
        [Header("升级特效")] public GameObject LevelUpGo;
    }
}