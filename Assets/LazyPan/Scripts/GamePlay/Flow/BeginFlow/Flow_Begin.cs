using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Begin : Flow {
        private Entity volumeEntity;
        private Entity lightEntity;
        private Entity beginCameraEntity;
        private Comp comp;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);

            comp = UI.Instance.Open("UI_Begin");
            comp.Get<TextMeshProUGUI>("UI_Begin_FlowTitle").text = "BeginFlow";

            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_DirectionalLight");
            beginCameraEntity = Obj.Instance.LoadEntity("Obj_BeginCamera");

            ButtonRegister.AddListener(comp.Get<Button>("UI_Begin_NextBtn"), () => {
                Clear();
                Launch.instance.StageLoad("Choose");
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Begin_NextBtn"));
            UI.Instance.Close("UI_Begin");

            Obj.Instance.UnLoadEntity(volumeEntity);
            Obj.Instance.UnLoadEntity(lightEntity);
            Obj.Instance.UnLoadEntity(beginCameraEntity);
        }
    }
}