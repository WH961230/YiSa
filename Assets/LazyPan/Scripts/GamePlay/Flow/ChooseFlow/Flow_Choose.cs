using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Choose : Flow {
        private Entity chooseEntity;
        private Entity volumeEntity;
        private Comp comp;
        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            comp = UI.Instance.Open("UI_Choose");
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.TITLE).text = "ChooseFlow";
            chooseEntity = Obj.Instance.LoadEntity("Obj_Camera_ChooseCamera");
            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.NEXT), () => {
                Clear();
                Launch.instance.StageLoad("Fight");
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(Cond.Instance.Get<Button>(comp, Label.NEXT));
            UI.Instance.Close("UI_Choose");
            Obj.Instance.UnLoadEntity(chooseEntity);
            Obj.Instance.UnLoadEntity(volumeEntity);
        }
    }
}