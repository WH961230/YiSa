using TMPro;
using UnityEngine.InputSystem;

namespace LazyPan {
    public class ConsoleEx : Singleton<ConsoleEx> {
        private Comp comp;
        private bool firstSendCode;

        /*初始化*/
        public void Init(bool initOpen) {
            InputRegister.Instance.Dispose(InputRegister.Instance.Console);
            InputRegister.Instance.Load(InputRegister.Instance.Console, ConsoleEvent);
            comp =
                Loader.LoadComp("控制台界面", "UI/UI_Console", Data.Instance.UIDontDestroyRoot, initOpen);
            ContentClear();
        }

        /*控制台事件*/
        private void ConsoleEvent(InputAction.CallbackContext obj) {
            if (obj.performed) {
                bool hasDebug = comp.gameObject.activeSelf;
                if (hasDebug) {
                    comp.gameObject.SetActive(false);
                } else {
                    comp.gameObject.SetActive(true);
                    firstSendCode = true;
                    //绑定按键
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).text = "";
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).ActivateInputField();
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).onEndEdit.RemoveAllListeners();
                    Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).onEndEdit.AddListener(SendCode);
                }
            }
        }

        /*发送命令*/
        private void SendCode(string content) {
            if (Keyboard.current.enterKey.isPressed) {
                if (!string.IsNullOrEmpty(content)) {
                    Content("you", content);
                    CodeAction(content);
                }
                Cond.Instance.Get<TMP_InputField>(comp, Label.CODE).ActivateInputField();
            }
        }

        /*命令的触发表现*/
        private bool CodeAction(string code) {
            if (code == "help") {
                Content("computer", "code: help[帮助] clear[清空控制台] ");
                return true;
            }

            if (code == "clear") {
                ContentClear();
                return true;
            }

            return false;
        }

        /*新增内容*/
        public void Content(string who, string content) {
            string originalText = Cond.Instance.Get<TextMeshProUGUI>(comp, Label.CONTENT).text;
            SetText(string.Concat(who, " : ", content, "\n", originalText));
        }

        /*清空内容*/
        private void ContentClear() {
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.CONTENT).text = null;
        }

        /*设置输入内容*/
        private void SetText(string content) {
            Cond.Instance.Get<TextMeshProUGUI>(comp, Label.CONTENT).text = content;
        }
    }
}