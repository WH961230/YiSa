using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Flow_Begin : Flow {
        private Entity volumeEntity;
        private Entity lightEntity;
        private Entity beginCameraEntity;
        private Comp comp;
        private Comp announcementComp;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);

            comp = UI.Instance.Open("UI_Begin");
            volumeEntity = Obj.Instance.LoadEntity("Obj_Volume_Volume");
            lightEntity = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
            beginCameraEntity = Obj.Instance.LoadEntity("Obj_Camera_BeginCamera");

            announcementComp = Cond.Instance.Get<Comp>(comp, Label.ANNOUNCEMENT);
            announcementComp.gameObject.SetActive(false);
            if (!Data.Instance.FirstPlay) {
                announcementComp.gameObject.SetActive(true);
                ButtonRegister.AddListener(Cond.Instance.Get<Button>(announcementComp, Label.BACK), () => {
                    announcementComp.gameObject.SetActive(false);
                });
                Data.Instance.FirstPlay = true;
            }

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