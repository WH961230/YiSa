using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Choose : Flow {
        private Comp comp;
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            comp = UI.Instance.Open("UI_Choose");
            comp.Get<TextMeshProUGUI>("UI_FlowTitle").text = "ChooseFlow";
            ButtonRegister.AddListener(comp.Get<Button>("UI_NextBtn"), () => {
                BaseFlow.ChangeFlow<Flow_Fight>();
            });
        }

        public override void OnClear() {
            base.OnClear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_NextBtn"));
            UI.Instance.Close("UI_Choose");
        }
    }
}