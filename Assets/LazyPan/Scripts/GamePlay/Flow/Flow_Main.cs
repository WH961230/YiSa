namespace LazyPan {
    public class Flow_Main : Flow {
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            FlowDic.Add(typeof(Flow_Start), new Flow_Start());
            FlowDic.Add(typeof(Flow_Choose), new Flow_Choose());
            FlowDic.Add(typeof(Flow_Fight), new Flow_Fight());
            FlowDic.Add(typeof(Flow_Clear), new Flow_Clear());
            ChangeFlow<Flow_Start>();
        }
    }
}