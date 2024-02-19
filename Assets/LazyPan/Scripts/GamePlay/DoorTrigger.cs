using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : MonoBehaviour {
    public UnityEvent doorOpen;
    public UnityEvent doorClose;

    void Start() {
    }

    void Update() {
    }

    private void OnTriggerEnter(Collider other) {
        doorOpen?.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        doorClose?.Invoke();
    }
}