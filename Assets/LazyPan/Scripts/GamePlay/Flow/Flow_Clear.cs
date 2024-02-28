using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Clear : Flow {
        private Comp comp;
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            comp = UI.Instance.Open("UI_Clear");
            comp.Get<TextMeshProUGUI>("UI_FlowTitle").text = "ClearFlow";
            ButtonRegister.AddListener(comp.Get<Button>("UI_BackHomeBtn"), () => {
                BaseFlow.ChangeFlow<Flow_Start>();
            });

            ButtonRegister.AddListener(comp.Get<Button>("UI_PlayAgainBtn"), () => {
                BaseFlow.ChangeFlow<Flow_Choose>();
            });
        }

        public override void OnClear() {
            base.OnClear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_BackHomeBtn"));
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_PlayAgainBtn"));
            UI.Instance.Close("UI_Clear");
        }
    }
}