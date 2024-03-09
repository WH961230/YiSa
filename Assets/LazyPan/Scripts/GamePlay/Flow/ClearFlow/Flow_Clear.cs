using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Clear : Flow {
        private Entity cameraEntity;
        private Entity volumeEntity;
        private Comp comp;
        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            comp = UI.Instance.Open("UI_Clear");
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.TITLE).text = "ClearFlow";

            cameraEntity = Obj.Instance.LoadEntity("Obj_Camera_ClearCamera");
            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.HOME), () => {
                Clear();
                Launch.instance.StageLoad("Begin");
            });

            ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.AGAIN), () => {
                Clear();
                Launch.instance.StageLoad("Choose");
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(Cond.Instance.Get<Button>(comp, Label.HOME));
            ButtonRegister.RemoveAllListener(Cond.Instance.Get<Button>(comp, Label.AGAIN));
            UI.Instance.Close("UI_Clear");
            Obj.Instance.UnLoadEntity(cameraEntity);
            Obj.Instance.UnLoadEntity(volumeEntity);
        }
    }
}