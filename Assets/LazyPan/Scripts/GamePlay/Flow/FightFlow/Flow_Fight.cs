using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Fight : Flow {
        private Comp comp;
        private Entity floorEntity;
        private Entity cameraEntity;
        private Entity playerSoldierEntity;
        private Entity robotSoldierEntity;
        private Entity beginTimeline;
        private Entity volumeEntity;
        private Entity lightEntity;
        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            comp = UI.Instance.Open("UI_Fight");
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.TITLE).text = "FightFlow";

            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
            floorEntity = Obj.Instance.LoadEntity("Obj_Terrain_Cube");

            beginTimeline = Obj.Instance.LoadEntity("Obj_Event_BeginTimeline");
            //播放完开场演示后生成玩家
            Cond.Instance.Get<PlayableDirector>(beginTimeline, Label.PLAYABLEDIRECTOR).stopped += director => {
                playerSoldierEntity = Obj.Instance.LoadEntity("Obj_Player_Soldier");
                robotSoldierEntity = Obj.Instance.LoadEntity("Obj_Robot_Soldier");
                cameraEntity = Obj.Instance.LoadEntity("Obj_Camera_FightCamera");
            };

            ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.NEXT), () => {
                Clear();
                Launch.instance.StageLoad("Clear");
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(Cond.Instance.Get<Button>(comp, Label.NEXT));
            UI.Instance.Close("UI_Fight");

            Obj.Instance.UnLoadEntity(floorEntity);
            Obj.Instance.UnLoadEntity(volumeEntity);
            Obj.Instance.UnLoadEntity(lightEntity);
            Obj.Instance.UnLoadEntity(playerSoldierEntity);
            Obj.Instance.UnLoadEntity(robotSoldierEntity);
            Obj.Instance.UnLoadEntity(beginTimeline);
            Obj.Instance.UnLoadEntity(cameraEntity);
        }
    }
}