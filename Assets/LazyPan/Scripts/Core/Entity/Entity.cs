using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LazyPan {
    [Serializable]
    public class Entity {
        public int ID;//身份ID
        public GameObject Prefab;//物体
        public Comp Comp;//组件
        public ObjConfig ObjConfig;//配置
        public EntityData EntityData;//实体数据
        public Setting EntitySetting;//实体配置

        public void OnInit(string sign) {
            //设置配置
            ObjConfig objConfig = ObjConfig.Get(sign);
            ObjConfig = objConfig;
            //实体配置
            EntitySetting = Loader.LoadAsset<Setting>(AssetType.ASSET, objConfig.Setting);
            //获取对象池的物体 如数量不足 对象池预加载
            if (objConfig.IsPreload == 1) {
                Data.Instance.GetPreloadGo(sign, out Prefab);
            } else {
                Prefab = Data.Instance.GetGo(sign);
            }

            //设置Comp
            Comp = Prefab.GetComponent<Comp>();
            //设置ID
            ID = ++Data.Instance.EntityID;
            //物体名赋值
            Prefab.name = string.Concat(objConfig.Name, "_", ID);
            //创建实体数据
            EntityData = new EntityData();
            EntityData.ObjType = EntitySetting.ObjType;
            EntityData.Health = EntitySetting.HealthMax;//血
            EntityData.HealthMax = EntitySetting.HealthMax;
            EntityData.AttackBase = EntitySetting.AttackSetting.AttackBase;//攻击力
            EntityData.AttackRatio = EntitySetting.AttackSetting.AttackRatio;//攻击力倍率
            EntityData.AttackExtraRatio = EntitySetting.AttackSetting.AttackExtraRatio;//暴击倍率
            EntityData.Experience = EntitySetting.Experience;//经验值
            EntityData.Level = EntitySetting.Level;//等级
            EntityData.AttackInterval = EntitySetting.AttackInterval;//攻击间隔
            EntityData.ReloadInterval = EntitySetting.ReloadInterval;//换弹时间
            EntityData.BulletNum = EntitySetting.BulletNum;//子弹
            EntityData.BulletMaxNum = EntitySetting.BulletMaxNum;
            EntityData.BulletPrefab = EntitySetting.BulletFx;
            EntityData.MovementSpeed = EntitySetting.MovementSpeed;//移动速度
            EntityData.InjuredHealthFloatingGo = EntitySetting.InjuredHealthFloatingGo;//受伤血特效
            EntityData.RecoverHealthFloatingGo = EntitySetting.RecoverHealthFloatingGo;//恢复血特效
            EntityData.FootStepFx = EntitySetting.FootStepFx;//脚步特效
            //注册实体
            Data.Instance.AddEntity(ID, this);
            //注册配置行为
            Data.Instance.BehaviourDic.TryAdd(ID, new List<Behaviour>());
            if (!string.IsNullOrEmpty(objConfig.Behaviour)) {
                string[] behaviourArray = objConfig.Behaviour.Split("|");
                for (int i = 0; i < behaviourArray.Length; i++) {
                    BehaviourRegister.Instance.RegisterBehaviour(ID, behaviourArray[i]);
                }
            }
        }

        public void OnClear() {
            //注销行为
            if (Data.Instance.BehaviourDic.TryGetValue(ID, out List<Behaviour> behaviours)) {
                if (behaviours != null && behaviours.Count > 0) {
                    int index = behaviours.Count - 1;
                    for (int i = index; i >= 0; i--) {
                        BehaviourRegister.Instance.UnRegisterBehaviour(ID, behaviours[i].BehaviourSign);
                    }
                }
            }
            //注销实体
            Data.Instance.RemoveEntity(ID);
            //物体销毁
            if (ObjConfig.IsPreload == 1) {
                Data.Instance.RecycleGo(ObjConfig.Sign, Prefab);
            } else {
                Object.Destroy(Prefab);
            }
            Prefab = null;
            //销毁配置
            ObjConfig = null;
            //ID重置
            ID = 0;
            //数据销毁
            EntityData = null;
        }
    }
}