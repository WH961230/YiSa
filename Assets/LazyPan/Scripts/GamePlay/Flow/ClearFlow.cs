using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class ClearFlow : Flow {
        private Comp ClearComp;

        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            Sound.Instance.SoundPlay("ClearFlow", Vector3.zero, false, 10f);
            ClearComp = UI.Instance.Open("UI_Clear");
            Button quitBtn = ClearComp.Get<Button>("UI_Clear_BackToHomeBtn");
            ButtonRegister.AddListener(quitBtn, BackToHomeButton);
            Button restartGameBtn = ClearComp.Get<Button>("UI_Clear_RestartGameBtn");
            ButtonRegister.AddListener(restartGameBtn, RestartGameButton);
        }

        private void BackToHomeButton() {
            BaseFlow.ChangeFlow<MainInterfaceFlow>();
        }

        private void RestartGameButton() {
            BaseFlow.ChangeFlow<FightFlow>();
        }

        public override void ChangeFlow<T>() {
            base.ChangeFlow<T>();
        }

        public override void EndFlow() {
            base.EndFlow();
        }

        public override T GetFlow<T>() {
            return base.GetFlow<T>();
        }

        public override void OnClear() {
            base.OnClear();
            Button quitBtn = ClearComp.Get<Button>("UI_Clear_BackToHomeBtn");
            ButtonRegister.RemoveListener(quitBtn, BackToHomeButton);
            Button restartGameBtn = ClearComp.Get<Button>("UI_Clear_RestartGameBtn");
            ButtonRegister.RemoveListener(restartGameBtn, RestartGameButton);
            UI.Instance.Close();
        }
    }
}