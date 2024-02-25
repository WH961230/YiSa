namespace LazyPan {
    public class MainFlow : Flow {
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            FlowDic.Add(typeof(BeginFlow), new BeginFlow());
            ChangeFlow<BeginFlow>();
        }

        public override void OnClear() {
            base.OnClear();
        }

        public override void ChangeFlow<T>() {
            base.ChangeFlow<T>();
        }

        public override void EndFlow() {
            base.EndFlow();
        }

        public override T GetFlow<T>() {
            return base.GetFlow<T>();
        }
    }
}