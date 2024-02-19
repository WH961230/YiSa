using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour {
    public List<Transform> transforms;
    public Transform followTran;
    public Vector3 offVec;

    private void Update() {
        if (followTran == null) {
            return;
        }

        foreach (Transform tran in transforms) {
            Vector3 position = Camera.main.WorldToScreenPoint(followTran.position) + offVec;
            position.z = 0;
            tran.localPosition = position;
        }
    }
}