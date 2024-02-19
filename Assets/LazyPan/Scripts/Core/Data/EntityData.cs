using System;
using UnityEngine;

namespace LazyPan {
    public class EntityData {
        public ObjType ObjType;//物品类型
        public int Health;//血量
        public int HealthMax;//血量上限
        public float MovementSpeed;//移动速度
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
    }

    [Serializable]
    public enum ObjType {
        None,
        MainCamera,
        MainPlayer,
        Monster,
    }
}