using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Clear : Flow {
        private Comp comp;
        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            comp = UI.Instance.Open("UI_Clear");
            comp.Get<TextMeshProUGUI>("UI_Clear_FlowTitle").text = "ClearFlow";

            ButtonRegister.AddListener(comp.Get<Button>("UI_Clear_BackHomeBtn"), () => {
                Launch.instance.StageLoad("Begin");
            });

            ButtonRegister.AddListener(comp.Get<Button>("UI_Clear_PlayAgainBtn"), () => {
                Launch.instance.StageLoad("Choose");
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Clear_BackHomeBtn"));
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Clear_PlayAgainBtn"));
            UI.Instance.Close("UI_Clear");
        }
    }
}