using System.Collections.Generic;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Fight : Flow {
        private Comp comp;
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
            playableDirector.stopped += director => {
                if (playableDirector.enabled) {
                    playerSoldierEntity = Obj.Instance.LoadEntity("Obj_Player_Soldier");
                    towerEntity = Obj.Instance.LoadEntity("Obj_Building_Tower");

                    robotSoldierEntities = new List<Entity>();
 
                    AddRobot();
                    AddRobot();
                    AddRobot();
                    AddRobot();
                    AddRobot();
                    AddRobot();
                    AddRobot();
                    AddRobot();
                    AddRobot();
                    AddRobot();

                    cameraEntity = Obj.Instance.LoadEntity("Obj_Camera_FightCamera");
                    comp = UI.Instance.Open("UI_Fight");
                    Cond.Instance.Get<TextMeshProUGUI>(comp, Label.TITLE).text = "FightFlow";

                    ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.NEXT), Next);
                }
            };
        }

        public void AddRobot() {
            robotSoldierEntities.Add(Obj.Instance.LoadEntity("Obj_Robot_Soldier"));
        }

        public void RemoveRobot(Entity robotEntity) {
            robotSoldierEntities.Remove(robotEntity);
            Obj.Instance.UnLoadEntity(robotEntity);
        }

        public void Next() {
            Clear();
            Launch.instance.StageLoad("Clear");
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(Cond.Instance.Get<Button>(comp, Label.NEXT));
            UI.Instance.Close("UI_Fight");

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