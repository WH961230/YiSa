using UnityEngine;

public class Lab_Trigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor")) {
            return;
        }

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb == null) {
            other.gameObject.AddComponent<Rigidbody>();
        }
    }
}