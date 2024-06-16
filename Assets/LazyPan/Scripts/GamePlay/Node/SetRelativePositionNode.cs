using GraphProcessor;
using UnityEngine;

[System.Serializable, NodeMenuItem("Custom/相对位置节点")]
public class SetRelativePositionNode : BaseNode {
    [Input(name = "输入相对坐标")] public Vector3 inputRelativePosition;
    [Input(name = "输入被跟踪游戏物体")] public GameObject inputBeFollowGameObject;
    [Input(name = "输入跟踪游戏物体")] public GameObject inputFollowGameObject;
    public bool IsLerp;
    public float LerpSpeed;
    public override string name => "相对位置节点";

    protected override void Process() {
        if (inputFollowGameObject != null && inputBeFollowGameObject != null) {
            if (IsLerp) {
                inputFollowGameObject.transform.position = Vector3.Lerp(inputFollowGameObject.transform.position,
                    inputBeFollowGameObject.transform.position + inputRelativePosition, Time.deltaTime * LerpSpeed);
            } else {
                inputFollowGameObject.transform.position =
                    inputBeFollowGameObject.transform.position + inputRelativePosition;
            }
        }
    }
}