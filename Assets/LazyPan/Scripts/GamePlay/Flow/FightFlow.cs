using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class FightFlow : Flow {
        private Entity TreatureEntity;
        private Entity PlayerEntity;
        private Entity MonsterACreatorEntity;
        private Entity MonsterBCreatorEntity;
        private Entity MainCameraEntity;
        private Comp fightComp;
        private GameObject FightFlowSoundGo;

        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            if (FightFlowSoundGo == null) {
                FightFlowSoundGo = Sound.Instance.SoundPlay("FightFlowLoop", Vector3.zero, true, -1);
            }

            Data.Instance.ExperiencePrefabs.Clear();
            MessageRegister.Instance.Reg<Entity>(MessageCode.RefreshEntityUI, RefreshEntityUI);
            MessageRegister.Instance.Reg<Entity>(MessageCode.MonsterEntityInit, MonsterEntityInit);
            MessageRegister.Instance.Reg<Entity>(MessageCode.LevelUp, LevelUpEntity);
            MessageRegister.Instance.Reg<Entity>(MessageCode.DeathEntity, DeathEntityEvent);
            MessageRegister.Instance.Reg<Entity>(MessageCode.ClearEntity, ClearEntityEvent);
            fightComp = UI.Instance.Open("UI_Fight");

            //加载角色
            PlayerEntity = Obj.Instance.LoadEntity(Data.Instance.PlayerEntitySign);
            //特性UI
            Comp traitComp = UI.Instance.Open("UI_Trait");
            //特性内容
            traitComp.Get<TextMeshProUGUI>("UI_Trait_Detail").text =
                Loader.LoadAsset<Setting>(AssetType.ASSET, ObjConfig.Get(Data.Instance.PlayerEntitySign).Setting).Detail;
            //注册取消按钮
            ButtonRegister.AddListener(traitComp.Get<Button>("UI_Trait_CloseBtn"), UI.Instance.Close);
            //加载宝藏
            TreatureEntity = Obj.Instance.LoadEntity("Obj_TreasureA");
            //怪物生成器
            MonsterACreatorEntity = Obj.Instance.LoadEntity("Obj_Creator_MonsterA");
            MonsterBCreatorEntity = Obj.Instance.LoadEntity("Obj_Creator_MonsterB");
            //返回主界面按钮
            Button backHomeBtn = fightComp.Get<Button>("UI_Home");
            ButtonRegister.AddListener(backHomeBtn, BackMainInterface);

            if (Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out MainCameraEntity)) {
                BehaviourRegister.Instance.RegisterBehaviour(MainCameraEntity.ID, "Behaviour_FollowTarget");
            }

            Cursor.Instance.SetAimCursor();
            Data.Instance.KillMonsterCount = 0;
            MessageRegister.Instance.Dis(MessageCode.RefreshEntityUI, PlayerEntity);
        }

        private void RefreshEntityUI(Entity entity) {
            if (entity.EntityData.ObjType == ObjType.MainPlayer) {
                fightComp.Get<Slider>("UI_PlayerSlider").value = (float)entity.EntityData.Health / entity.EntityData.HealthMax;
                fightComp.Get<TextMeshProUGUI>("UI_PlayerSliderText").text = string.Concat(entity.EntityData.Health, " / ", entity.EntityData.HealthMax);
                if (Data.Instance.TryGetLevelSetting(entity.EntitySetting, entity.EntityData.Level,
                    out LevelSetting levelSetting)) {
                    int experienceMax = levelSetting.ExperienceMax;
                    fightComp.Get<Slider>("UI_ExpSlider").value = (float) entity.EntityData.Experience / experienceMax;
                    fightComp.Get<TextMeshProUGUI>("UI_ExpSliderText").text = string.Concat("Level ", entity.EntityData.Level, " ( ", entity.EntityData.Experience, " / ", experienceMax, " )");
                }
                fightComp.Get<TextMeshProUGUI>("UI_BulletInfo").text = string.Concat(entity.EntityData.BulletNum, " / ", entity.EntityData.BulletMaxNum);
            } else if (entity.EntityData.ObjType == ObjType.Monster) {
                entity.Comp.Get<Slider>("HealthBar").value = (float) entity.EntityData.Health / entity.EntityData.HealthMax;
            }

            fightComp.Get<TextMeshProUGUI>("UI_KillMonsterCount").text = string.Concat("击杀怪物数量: ", Data.Instance.KillMonsterCount);
        }

        private void MonsterEntityInit(Entity entity) {
            if (entity.EntityData.ObjType == ObjType.Monster) {
            }
        }

        private void LevelUpEntity(Entity entity) {
            if (entity.EntityData.ObjType == ObjType.MainPlayer) {
                
            }
        }

        private void DeathEntityEvent(Entity entity) {
            if (entity.EntityData.ObjType == ObjType.MainPlayer) {
                BaseFlow.ChangeFlow<ClearFlow>();
            } else if (entity.EntityData.ObjType == ObjType.Monster) {
                Data.Instance.KillMonsterCount++;
                if (entity.ObjConfig.IsPreload == 1) {
                    Data.Instance.RecycleGo(entity.ObjConfig.Sign, entity.Prefab);
                } else {
                    Object.Destroy(entity.Prefab);
                }
            }
        }

        private void ClearEntityEvent(Entity tempEntity) {
            Obj.Instance.UnLoadEntity(tempEntity);
        }
        
        private void BackMainInterface() {
            OnClear();
            BaseFlow.ChangeFlow<MainInterfaceFlow>();
        }

        public override void OnClear() {
            base.OnClear();
            Sound.Instance.SoundRecycle(FightFlowSoundGo);
            foreach (GameObject experiencePrefab in Data.Instance.ExperiencePrefabs) {
                Object.Destroy(experiencePrefab);
            }
            Data.Instance.ExperiencePrefabs.Clear();

            if (Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out MainCameraEntity)) {
                BehaviourRegister.Instance.UnRegisterBehaviour(MainCameraEntity.ID, "Behaviour_FollowTarget");
            }

            UI.Instance.Close("UI_Trait");
            UI.Instance.Close("UI_Fight");
            UI.Instance.Close();

            Obj.Instance.UnLoadEntity(MonsterACreatorEntity);
            Obj.Instance.UnLoadEntity(MonsterBCreatorEntity);
            Obj.Instance.UnLoadEntity(TreatureEntity);
            Obj.Instance.UnLoadEntity(PlayerEntity);

            Button backHomeBtn = UI.Instance.Get("UI_Fight").Get<Button>("UI_Home");
            ButtonRegister.RemoveListener(backHomeBtn, BackMainInterface);

            Cursor.Instance.ClearAimCursor();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.RefreshEntityUI, RefreshEntityUI);
            MessageRegister.Instance.UnReg<Entity>(MessageCode.MonsterEntityInit, MonsterEntityInit);
            MessageRegister.Instance.UnReg<Entity>(MessageCode.LevelUp, LevelUpEntity);
            MessageRegister.Instance.UnReg<Entity>(MessageCode.DeathEntity, DeathEntityEvent);
            MessageRegister.Instance.UnReg<Entity>(MessageCode.ClearEntity, ClearEntityEvent);
        }
    }
}