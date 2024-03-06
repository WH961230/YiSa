using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public void Init(string sign) {
            //设置ID
            ID = ++Data.Instance.EntityID;
            //设置配置
            ObjConfig objConfig = ObjConfig.Get(sign);
            ObjConfig = objConfig;
            //实体配置
            EntitySetting = Data.Instance.Setting;
            //获取对象池的物体 如数量不足 对象池预加载
            Prefab = Loader.LoadGo(null,
                string.Concat(SceneConfig.Get(SceneManager.GetActiveScene().name).DirPath, objConfig.Sign),
                Data.Instance.ObjRoot, true);
            //设置Comp
            Comp = Prefab.GetComponent<Comp>();
            //物体名赋值
            Prefab.name = string.Concat("[", ID, "]", objConfig.Name);
            //创建实体数据
            EntityData = new EntityData(objConfig.Sign, EntitySetting);
            //注册实体
            Data.Instance.AddEntity(ID, this);
            //注册配置行为
            Data.Instance.BehaviourDic.TryAdd(ID, new List<Behaviour>());
            if (!string.IsNullOrEmpty(objConfig.SetUpBehaviourSign)) {
                string[] behaviourArray = objConfig.SetUpBehaviourSign.Split("|");
                for (int i = 0; i < behaviourArray.Length; i++) {
                    BehaviourRegister.Instance.RegisterBehaviour(ID, behaviourArray[i]);
                }
            }
        }

        public void Clear() {
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
            Object.Destroy(Prefab);
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