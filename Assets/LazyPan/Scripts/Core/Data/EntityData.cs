using System;
using UnityEngine;

namespace LazyPan {
    public class EntityData {
        public ObjType ObjType;//物品类型
        public int Health;//血量
        public int HealthMax;//血量上限
        public float MovementSpeed;//移动速度
        public float RotateSpeed;//旋转速度
        public int Level;//等级
        public int Experience;//经验值
        public int AttackBase;//攻击力
        public float AttackRatio;//攻击力倍率
        public float AttackExtraRatio;//暴击倍率
        public float AttackInterval;//攻击间隔
        public float ReloadInterval;//换弹间隔时间
        public GameObject BulletPrefab;
        public int BulletNum;
        public int BulletMaxNum;
        public GameObject InjuredHealthFloatingGo;
        public GameObject RecoverHealthFloatingGo;
        public GameObject FootStepFx;
        public bool IsDeathRagdoll;

        public EntityData(string sign, Setting setting) {
            BaseSetting baseSetting = GetBaseSetting(sign, setting);
            ObjType = baseSetting == null ? ObjType.None : baseSetting.ObjType;
            
            MotionRotateSetting motionRotateSetting = GetMotionRotateSetting(sign, setting);
            MovementSpeed = motionRotateSetting == null ? 0 : motionRotateSetting.MovementSpeed;//移动速度
            RotateSpeed = motionRotateSetting == null ? 0 : motionRotateSetting.RotateSpeed;//旋转速度

            Health = setting.HealthMax;//血
            HealthMax = setting.HealthMax;
            AttackBase = setting.AttackSetting.AttackBase;//攻击力
            AttackRatio = setting.AttackSetting.AttackRatio;//攻击力倍率
            AttackExtraRatio = setting.AttackSetting.AttackExtraRatio;//暴击倍率
            Experience = setting.Experience;//经验值
            Level = setting.Level;//等级
            AttackInterval = setting.AttackInterval;//攻击间隔
            ReloadInterval = setting.ReloadInterval;//换弹时间
            BulletNum = setting.BulletNum;//子弹
            BulletMaxNum = setting.BulletMaxNum;
            BulletPrefab = setting.BulletFx;
            
            InjuredHealthFloatingGo = setting.InjuredHealthFloatingGo;//受伤血特效
            RecoverHealthFloatingGo = setting.RecoverHealthFloatingGo;//恢复血特效
            FootStepFx = setting.FootStepFx;//脚步特效
        }

        public BaseSetting GetBaseSetting(string sign, Setting setting) {
            foreach (BaseSetting tmpSetting in setting.BaseSettings) {
                if (tmpSetting.Sign == sign) {
                    return tmpSetting;
                }
            }

            return null;
        }

        public MotionRotateSetting GetMotionRotateSetting(string sign, Setting setting) {
            foreach (MotionRotateSetting tmpSetting in setting.MotionRotateSettings) {
                if (tmpSetting.Sign == sign) {
                    return tmpSetting;
                }
            }

            return null;
        }
    }

    [Serializable]
    public enum ObjType {
        None,//无
        MainCamera,//相机
        MainPlayer,//玩家
        Robot,//怪物
    }
}