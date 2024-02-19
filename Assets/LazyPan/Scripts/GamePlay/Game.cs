using UnityEngine;

namespace LazyPan {
    public class Game : MonoBehaviour {
        public static MainFlow MainFlow;
        private void Start() {
            Config.Instance.Init();
            UI.Instance.Init();
            Obj.Instance.Init();
            MainFlow = new MainFlow();
            MainFlow.OnInit(null);
        }

        void Update() {
            Data.Instance.OnUpdateEvent?.Invoke();
        }

        private void LateUpdate() {
            Data.Instance.OnLateUpdateEvent?.Invoke();
        }

        private void FixedUpdate() {
            Data.Instance.OnFixedUpdateEvent?.Invoke();
        }
    }
}