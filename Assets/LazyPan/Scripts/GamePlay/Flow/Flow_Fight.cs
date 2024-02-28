using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Fight : Flow {
        private Comp comp;
        private List<GameObject> markRootList = new List<GameObject>();
        private List<Entity> robotEntity = new List<Entity>();
        private Entity floorEntity;
        private Entity soldierEntity;
        private Entity beginTimeline;
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            comp = UI.Instance.Open("UI_Fight");
            comp.Get<TextMeshProUGUI>("UI_FlowTitle").text = "FightFlow";
            ButtonRegister.AddListener(comp.Get<Button>("UI_NextBtn"), () => {
                BaseFlow.ChangeFlow<Flow_Clear>();
            });

            floorEntity = Obj.Instance.LoadEntity("Obj_Floor");
            soldierEntity = Obj.Instance.LoadEntity("Obj_Soldier");
            beginTimeline = Obj.Instance.LoadEntity("Obj_BeginTimeline");

            markRootList.Clear();
            markRootList.Add(CreateMarkRoot());
            markRootList.Add(CreateMarkRoot());
            markRootList.Add(CreateMarkRoot());
        }

        private GameObject CreateMarkRoot() {
            GameObject markRoot = Loader.LoadGo("敌人标记点", "Obj/Obj_MarkRobot", Data.Instance.ObjRoot, true);
            markRoot.transform.position = Data.Instance.Setting.CreatorSetting
                .CreatorPoints[Random.Range(0, Data.Instance.Setting.CreatorSetting.CreatorPoints.Count)];
            return markRoot;
        }

        public override void OnClear() {
            base.OnClear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_NextBtn"));
            UI.Instance.Close("UI_Fight");
            Obj.Instance.UnLoadEntity(floorEntity);
            Obj.Instance.UnLoadEntity(soldierEntity);
            Obj.Instance.UnLoadEntity(beginTimeline);
            foreach (GameObject markRoot in markRootList) {
                Object.Destroy(markRoot);
            }
        }
    }
}