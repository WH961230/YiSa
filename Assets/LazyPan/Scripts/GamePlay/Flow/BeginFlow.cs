namespace LazyPan {
    public class BeginFlow : Flow {
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            LoadObj();
            LoadUI();
        }

        private void LoadObj() {
            Obj.Instance.LoadEntity("Obj_Floor");
            Obj.Instance.LoadEntity("Obj_Volume");
            Obj.Instance.LoadEntity("Obj_DirectionalLight");
            Obj.Instance.LoadEntity("Obj_Player");
            Obj.Instance.LoadEntity("Obj_BeginTimeline");
            Obj.Instance.LoadEntity("Obj_Camera");
        }

        private void LoadUI() {
            UI.Instance.Open("UI_Lab");
        }

        public override void OnClear() {
            base.OnClear();
        }
    }
}