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
        private Entity towerEntity;
        private Entity beginTimelineEntity;
        private Entity volumeEntity;
        private Entity lightEntity;
        private Entity robotCreatorEntity;
        private Entity levelSelectEntity;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
#if UNITY_EDITOR
            ConsoleEx.Instance.Content("log", $"=> 进入战斗流程");
#endif
            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
            floorEntity = Obj.Instance.LoadEntity("Obj_Terrain_Cube");

            beginTimelineEntity = Obj.Instance.LoadEntity("Obj_Event_BeginTimeline");
            PlayTimeline();
        }

        public Comp GetUI() {
            return battleComp;
        }

        private void PlayTimeline() {
            //播放完开场演示后生成玩家
            PlayableDirector playableDirector = Cond.Instance.Get<PlayableDirector>(beginTimelineEntity, Label.PLAYABLEDIRECTOR);
            playableDirector.stopped += Play;
            playableDirector.Play();
        }

        private void Play(PlayableDirector pd) {
            /*战斗UI*/
            battleComp = UI.Instance.Open("UI_Battle");
            /*战场玩家*/
            playerSoldierEntity = Obj.Instance.LoadEntity("Obj_Player_BattleSoldier");
            /*玩家升级选择器*/
            //Behaviour_Event_LevelUpSelect
            /*塔*/
            towerEntity = Obj.Instance.LoadEntity("Obj_Building_Tower");
            /*相机实体*/
            cameraEntity = Obj.Instance.LoadEntity("Obj_Camera_BattleCamera");
            /*机器人生成器*/
            robotCreatorEntity = Obj.Instance.LoadEntity("Obj_Event_RobotCreator");
            /*关卡难度选择器*/
            levelSelectEntity = Obj.Instance.LoadEntity("Obj_Event_LevelSelect");
            pd.enabled = false;
        }

        /*结算*/
        public void Settlement() {
            Obj.Instance.UnLoadEntity(robotCreatorEntity);

            SettlementComp = Cond.Instance.Get<Comp>(battleComp, Label.SETTLEMENT);
            SettlementComp.gameObject.SetActive(true);

            Button again = Cond.Instance.Get<Button>(SettlementComp, Label.AGAIN);
            ButtonRegister.AddListener(again, Again);

            Button returnBtn = Cond.Instance.Get<Button>(SettlementComp, Label.RETURN);
            ButtonRegister.AddListener(returnBtn, Return);
        }

        /*回到主菜单*/
        private void Return() {
            Button returnBtn = Cond.Instance.Get<Button>(SettlementComp, Label.RETURN);
            ButtonRegister.RemoveListener(returnBtn, Return);
            Clear();
            Launch.instance.StageLoad("Begin");
        }

        /*再来一局*/
        private void Again() {
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
            Obj.Instance.UnLoadEntity(towerEntity);
            Obj.Instance.UnLoadEntity(beginTimelineEntity);
            Obj.Instance.UnLoadEntity(cameraEntity);
        }
    }
}