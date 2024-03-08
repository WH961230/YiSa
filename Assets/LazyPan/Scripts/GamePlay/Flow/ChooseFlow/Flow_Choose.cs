using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Choose : Flow {
        private Entity chooseEntity;
        private Comp comp;
        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            comp = UI.Instance.Open("UI_Choose");
            comp.Get<TextMeshProUGUI>("UI_Choose_FlowTitle").text = "ChooseFlow";

            chooseEntity = Obj.Instance.LoadEntity("Obj_Camera_ChooseCamera");

            ButtonRegister.AddListener(comp.Get<Button>("UI_Choose_NextBtn"), () => {
                Clear();
                Launch.instance.StageLoad("Fight");
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Choose_NextBtn"));
            UI.Instance.Close("UI_Choose");

            Obj.Instance.UnLoadEntity(chooseEntity);
        }
    }
}