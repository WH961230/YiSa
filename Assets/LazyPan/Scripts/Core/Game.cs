using LazyPan;
using UnityEngine;

public class Game : MonoBehaviour {
    [HideInInspector] public GameSetting GameSetting;
    public static Game instance;

    private void Awake() {
        instance = this;
        GameSetting = Loader.LoadGameSetting();
    }

    private void Start() {
        Config.Instance.Init();
        Obj.Instance.Init();
        UI.Instance.Init();
        new Flow_Main().OnInit(null);
    }

    private void Update() { Data.Instance.OnUpdateEvent.Invoke(); }

    private void FixedUpdate() { Data.Instance.OnFixedUpdateEvent.Invoke(); }

    private void LateUpdate() { Data.Instance.OnLateUpdateEvent.Invoke(); }
}