using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class MainInterfaceFlow : Flow {
        public GameObject DefaultPlayerModel;
        private Comp MainComp;
        private GameObject MainInterfaceFlowLoopSoundGo;
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            MainComp = UI.Instance.Open("UI_Main");
            if (MainInterfaceFlowLoopSoundGo == null) {
                MainInterfaceFlowLoopSoundGo = Sound.Instance.SoundPlay("MainInterfaceFlowLoop", Vector3.zero, true, -1);
            }

            if (DefaultPlayerModel == null) {
                DefaultPlayerModel = GameObject.Find("Obj_BeginPlayerModel");
            }
            DefaultPlayerModel.SetActive(true);

            //开始战斗按钮
            ButtonRegister.AddListener(MainComp.Get<Button>("UI_Begin_StartBtn"), () => {
                BaseFlow.ChangeFlow<ChoosePlayerFlow>();
            });

            //退出游戏按钮
            ButtonRegister.AddListener(MainComp.Get<Button>("UI_Begin_QuitBtn"), () => {
                QuitGame();
            });

            //主相机
            if (!Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out Entity entity)) {
                entity = Obj.Instance.LoadEntity("Obj_MainCamera");
            }

            entity.Prefab.transform.position = GameObject.Find("MainCameraPosition_0").transform.position;
            entity.Prefab.transform.rotation = GameObject.Find("MainCameraPosition_0").transform.rotation;
        }

        private void QuitGame() {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
            OnClear();
        }

        public override void OnClear() {
            base.OnClear();
            Comp BeginComp = UI.Instance.Get("UI_Main");
            ButtonRegister.RemoveAllListener(BeginComp.Get<Button>("UI_Begin_StartBtn"));
            ButtonRegister.RemoveAllListener(BeginComp.Get<Button>("UI_Begin_QuitBtn"));
            UI.Instance.Close("UI_Main");
            Sound.Instance.SoundRecycle(MainInterfaceFlowLoopSoundGo);
            MainInterfaceFlowLoopSoundGo = null;
        }
    }
}