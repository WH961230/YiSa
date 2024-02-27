using UnityEngine;

namespace LazyPan {
    public class BeginFlow : Flow {
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            LoadObj();
            LoadUI();
        }

        private void LoadObj() {
            Data.Instance.Setting = Loader.LoadSetting();

            Obj.Instance.LoadEntity("Obj_Floor");
            Obj.Instance.LoadEntity("Obj_Volume");
            Obj.Instance.LoadEntity("Obj_DirectionalLight");
            Obj.Instance.LoadEntity("Obj_Soldier");
            Obj.Instance.LoadEntity("Obj_BeginTimeline");
            Obj.Instance.LoadEntity("Obj_Camera");

            CreateMarkRoot();
            CreateMarkRoot();
            CreateMarkRoot();
        }

        private void LoadUI() {
            UI.Instance.Open("UI_Lab");
        }

        private void CreateMarkRoot() {
            GameObject markRoot = Loader.LoadGo("敌人标记点", "Obj/Obj_MarkRobot", Data.Instance.ObjRoot, true);
            markRoot.transform.position = Data.Instance.Setting.CreatorSetting
                .CreatorPoints[Random.Range(0, Data.Instance.Setting.CreatorSetting.CreatorPoints.Count)];
        }

        public override void OnClear() {
            base.OnClear();
        }
    }
}