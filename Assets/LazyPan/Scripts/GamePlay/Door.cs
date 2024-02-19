using UnityEngine;

public class Door : MonoBehaviour {
    public static Door Instance;
    public void Awake() {
        Instance = this;
    }
}