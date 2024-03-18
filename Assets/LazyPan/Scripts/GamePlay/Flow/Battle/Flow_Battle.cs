using System.Collections.Generic;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Battle : Flow {
        private Comp battleComp;
        private Comp SettlementComp;
        private Entity floorEntity;
        private Entity cameraEntity;
        private Entity playerSoldierEntity;
        private Entity robotSoldierEntity;
        private List<Entity> robotSoldierEntities;
        private Entity towerEntity;
        private Entity beginTimeline;
        private Entity volumeEntity;
        private Entity lightEntity;
        private Clock robotCreatorClock;
        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);

            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
            floorEntity = Obj.Instance.LoadEntity("Obj_Terrain_Cube");

            beginTimeline = Obj.Instance.LoadEntity("Obj_Event_BeginTimeline");
            //播放完开场演示后生成玩家
            PlayableDirector playableDirector = Cond.Instance.Get<PlayableDirector>(beginTimeline, Label.PLAYABLEDIRECTOR);
            playableDirector.stopped += Play;
            playableDirector.Play();
        }

        public Comp GetUI() {
            return battleComp;
        }

        public void Play(PlayableDirector pd) {
            battleComp = UI.Instance.Open("UI_Battle");
            playerSoldierEntity = Obj.Instance.LoadEntity("Obj_Player_BattleSoldier");
            towerEntity = Obj.Instance.LoadEntity("Obj_Building_Tower");
            robotSoldierEntities = new List<Entity>();
            AddRobot();
            cameraEntity = Obj.Instance.LoadEntity("Obj_Camera_BattleCamera");
            pd.enabled = false;
        }

        public void AddRobot() {
            robotSoldierEntities.Add(Obj.Instance.LoadEntity("Obj_Robot_Soldier"));
        }

        public void RemoveRobot(Entity robotEntity) {
            robotSoldierEntities.Remove(robotEntity);
            Obj.Instance.UnLoadEntity(robotEntity);
        }

        //结算
        public void Settlement() {
            SettlementComp = Cond.Instance.Get<Comp>(battleComp, Label.SETTLEMENT);
            SettlementComp.gameObject.SetActive(true);

            Button again = Cond.Instance.Get<Button>(SettlementComp, Label.AGAIN);
            ButtonRegister.AddListener(again, Again);

            Button returnBtn = Cond.Instance.Get<Button>(SettlementComp, Label.RETURN);
            ButtonRegister.AddListener(returnBtn, Return);
        }

        //下一步
        public void Return() {
            Button returnBtn = Cond.Instance.Get<Button>(SettlementComp, Label.RETURN);
            ButtonRegister.RemoveListener(returnBtn, Return);
            Clear();
            Launch.instance.StageLoad("Begin");
        }

        //再来一局
        public void Again() {
            Button again = Cond.Instance.Get<Button>(SettlementComp, Label.AGAIN);
            ButtonRegister.RemoveListener(again, Again);
            Clear();
            Data.Instance.OnUpdateEvent.RemoveAllListeners();
            Data.Instance.OnFixedUpdateEvent.RemoveAllListeners();
            Data.Instance.OnLateUpdateEvent.RemoveAllListeners();
            Game.instance.Clear();
            Game.instance.Init();
        }

        public override void Clear() {
            base.Clear();
            UI.Instance.Close("UI_Battle");

            Obj.Instance.UnLoadEntity(floorEntity);
            Obj.Instance.UnLoadEntity(volumeEntity);
            Obj.Instance.UnLoadEntity(lightEntity);
            Obj.Instance.UnLoadEntity(playerSoldierEntity);

            foreach (Entity entity in robotSoldierEntities) {
                Obj.Instance.UnLoadEntity(entity);
            }

            Obj.Instance.UnLoadEntity(towerEntity);
            Obj.Instance.UnLoadEntity(beginTimeline);
            Obj.Instance.UnLoadEntity(cameraEntity);
        }
    }
}