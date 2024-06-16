using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("Custom/朝向目标节点")]
public class SetLookAtTargetNode : BaseNode {
    [Input(name = "输入被朝向游戏物体")] public GameObject inputBeLookAtGameObject;
    [Input(name = "输入注视游戏物体")] public GameObject inputLookAtGameObject;
    public float LerpSpeed;
    public override string name => "朝向目标节点";

    protected override void Process() {
        if (inputBeLookAtGameObject != null && inputLookAtGameObject != null) {
            Vector3 lookDir = (inputBeLookAtGameObject.transform.position - inputLookAtGameObject.transform.position)
                .normalized;
            inputLookAtGameObject.transform.rotation =
                Quaternion.Slerp(inputLookAtGameObject.transform.rotation, Quaternion.LookRotation(lookDir),
                    Time.deltaTime * LerpSpeed);
        }
    }
}