namespace LazyPan {
    public interface IFlow {
        public void OnInit(Flow flow);//初始化
        public void OnClear();//清除
        public void ChangeFlow<T>();//改变状态
        public void EndFlow();//结束流程
    }
}