using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Begin : Flow {
        private Entity floorEntity;
        private Entity volumeEntity;
        private Entity lightEntity;
        private Entity beginCameraEntity;
        private Entity playerSoldierEntity;
        private Comp comp;
        private Comp announcementComp;
        private Entity startGameEntity;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
#if UNITY_EDITOR
            ConsoleEx.Instance.Content("log", $"=> 进入开始流程");
#endif
            comp = UI.Instance.Open("UI_Begin");
            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
            beginCameraEntity = Obj.Instance.LoadEntity("Obj_Camera_BeginCamera");

            playerSoldierEntity = Obj.Instance.LoadEntity("Obj_Player_BeginSoldier");
            Data.Instance.CanControl = true;

            floorEntity = Obj.Instance.LoadEntity("Obj_Terrain_Begin");

            announcementComp = Cond.Instance.Get<Comp>(comp, Label.ANNOUNCEMENT);
            announcementComp.gameObject.SetActive(false);

            if (!Data.Instance.FirstPlay) {
                announcementComp.gameObject.SetActive(true);
                ButtonRegister.AddListener(Cond.Instance.Get<Button>(announcementComp, Label.BACK), () => {
                    announcementComp.gameObject.SetActive(false);
                });
                Data.Instance.FirstPlay = true;
            }

            startGameEntity = Obj.Instance.LoadEntity("Obj_Event_BeginStartGame");
            startGameEntity.EntityData.BaseRuntimeData.CurMaxEnergy = 3;
            startGameEntity.EntityData.BaseRuntimeData.CurEnergy = 0;
            startGameEntity.EntityData.BaseRuntimeData.CurChargeEnergySpeed = 1;
            startGameEntity.EntityData.BaseRuntimeData.DefEnergyDownSpeed = 1;

            ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.QUIT), () => {
                Clear();
                Launch.instance.QuitGame();
            });
        }

        public void Next() {
            Clear();
            Launch.instance.StageLoad("Battle");
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(Cond.Instance.Get<Button>(comp, Label.NEXT));
            UI.Instance.Close("UI_Begin");

            Obj.Instance.UnLoadEntity(volumeEntity);
            Obj.Instance.UnLoadEntity(lightEntity);
            Obj.Instance.UnLoadEntity(playerSoldierEntity);
            Obj.Instance.UnLoadEntity(floorEntity);
            Obj.Instance.UnLoadEntity(beginCameraEntity);
            Obj.Instance.UnLoadEntity(startGameEntity);
        }
    }
}