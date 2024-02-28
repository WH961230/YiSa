using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Start : Flow {
        private Comp comp;
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            Data.Instance.Setting = Loader.LoadSetting();

            if (!Data.Instance.TryGetEntityByObjType(ObjType.MainVolume, out Entity volumeEntity)) {
                Obj.Instance.LoadEntity("Obj_Volume");
            }

            if (!Data.Instance.TryGetEntityByObjType(ObjType.MainLight, out Entity lightEntity)) {
                Obj.Instance.LoadEntity("Obj_DirectionalLight");
            }

            if (!Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out Entity cameraEntity)) {
                Obj.Instance.LoadEntity("Obj_Camera");
            }

            comp = UI.Instance.Open("UI_Start");
            comp.Get<TextMeshProUGUI>("UI_FlowTitle").text = "StartFlow";
            ButtonRegister.AddListener(comp.Get<Button>("UI_NextBtn"), () => {
                BaseFlow.ChangeFlow<Flow_Choose>();
            });
        }
        
        public override void OnClear() {
            base.OnClear();
            ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_NextBtn"));
            UI.Instance.Close("UI_Start");
        }
    }
}