using LazyPan;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game instance;

    public void Init() {
        instance = this;
        Obj.Instance.Preload();
        UI.Instance.Preload();
        Flo.Instance.Preload();
    }

    public void Clear() {
        Obj.Instance.UnPreload();
        UI.Instance.UnPreload();
        Flo.Instance.UnPreload();
    }

    private void Update() { Data.Instance.OnUpdateEvent.Invoke(); }

    private void FixedUpdate() { Data.Instance.OnFixedUpdateEvent.Invoke(); }

    private void LateUpdate() { Data.Instance.OnLateUpdateEvent.Invoke(); }
}