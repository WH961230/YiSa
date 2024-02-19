namespace LazyPan {
    public class MainFlow : Flow {
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            FlowDic.Add(typeof(MainInterfaceFlow), new MainInterfaceFlow());
            FlowDic.Add(typeof(ChoosePlayerFlow), new ChoosePlayerFlow());
            FlowDic.Add(typeof(FightFlow), new FightFlow());
            FlowDic.Add(typeof(ClearFlow), new ClearFlow());
            ChangeFlow<MainInterfaceFlow>();
            Cursor.Instance.SetCursor();
        }

        public override void OnClear() {
            base.OnClear();
            Cursor.Instance.ClearCursor();
        }
    }
}