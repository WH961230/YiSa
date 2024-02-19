using System;
using UnityEngine;

public class PropBox : MonoBehaviour {
    void Start() {
    }

    void Update() {
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}