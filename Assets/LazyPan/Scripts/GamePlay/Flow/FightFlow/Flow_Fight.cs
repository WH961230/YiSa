using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Fight : Flow {
        private Comp comp;
        private Entity floorEntity;
        private Entity cameraEntity;
        private Entity soldierEntity;
        private Entity shooterEntity;
        private Entity beginTimeline;
        private Entity volumeEntity;
        private Entity lightEntity;
        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            comp = UI.Instance.Open("UI_Fight");
            comp.Get<TextMeshProUGUI>("UI_Fight_FlowTitle").text = "FightFlow";

            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
            floorEntity = Obj.Instance.LoadEntity("Obj_Terrain_Cube");
            soldierEntity = Obj.Instance.LoadEntity("Obj_Player_Soldier");
            soldierEntity = Obj.Instance.LoadEntity("Obj_Robot_Soldier");
            beginTimeline = Obj.Instance.LoadEntity("Obj_Event_BeginTimeline");
            cameraEntity = Obj.Instance.LoadEntity("Obj_Camera_FightCamera");

            ButtonRegister.AddListener(comp.Get<Button>("UI_Fight_NextBtn"), () => {
                Clear();
                Launch.instance.StageLoad("Clear");
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Fight_NextBtn"));
            UI.Instance.Close("UI_Fight");

            Obj.Instance.UnLoadEntity(floorEntity);
            Obj.Instance.UnLoadEntity(volumeEntity);
            Obj.Instance.UnLoadEntity(lightEntity);
            Obj.Instance.UnLoadEntity(soldierEntity);
            Obj.Instance.UnLoadEntity(shooterEntity);
            Obj.Instance.UnLoadEntity(beginTimeline);
            Obj.Instance.UnLoadEntity(cameraEntity);
        }
    }
}