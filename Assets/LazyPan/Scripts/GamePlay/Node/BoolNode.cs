using UnityEngine;
using GraphProcessor;
using NodeGraphProcessor.Examples;

[System.Serializable, NodeMenuItem("Custom/Bool节点")]
public class BoolNode : LinearConditionalNode {
	[Input(name = "In"), SerializeField] public bool input;
	[Output(name = "Out")] public bool output;
	public override string name => "Bool节点";

	protected override void Process() {
		output = input;
	}
}