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
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.TITLE).text = "BeginFlow";
            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
            beginCameraEntity = Obj.Instance.LoadEntity("Obj_Camera_BeginCamera");

            ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.NEXT), () => {
                Clear();
                Launch.instance.StageLoad("Choose");
            });

            ButtonRegister.AddListener(Cond.Instance.Get<Button>(comp, Label.QUIT), () => {
                Clear();
                Launch.instance.QuitGame();
            });
        }

        public override void Clear() {
            base.Clear();
            ButtonRegister.RemoveAllListener(Cond.Instance.Get<Button>(comp, Label.NEXT));
            UI.Instance.Close("UI_Begin");

            Obj.Instance.UnLoadEntity(volumeEntity);
            Obj.Instance.UnLoadEntity(lightEntity);
            Obj.Instance.UnLoadEntity(beginCameraEntity);
        }
    }
}