using UnityEngine;
using GraphProcessor;

[System.Serializable, NodeMenuItem("Custom/Vector3节点")]
public class Vector3Node : BaseNode {
    [Output(name = "Out")] public Vector3 output;
    [Input(name = "In"), SerializeField] public Vector3 input;
    public override string name => "Vector3节点";

    protected override void Process() {
        output = input;
    }
}