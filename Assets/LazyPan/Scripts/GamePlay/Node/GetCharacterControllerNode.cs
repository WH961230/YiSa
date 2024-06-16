using System.Collections.Generic;
using GraphProcessor;
using NodeGraphProcessor.Examples;
using UnityEngine;

[System.Serializable, NodeMenuItem("Custom/获取角色控制器节点")]
public class GetCharacterControllerNode : ConditionalNode {
    [Input(name = "输入游戏物体")] public GameObject gameObject;
    [Output(name = "输出角色控制器")] public CharacterController characterController;
    public override string name => "获取角色控制器节点";

    public override IEnumerable<ConditionalNode> GetExecutedNodes() {
        throw new System.NotImplementedException();
    }

    protected override void Process() {
        if (characterController == null) {
            characterController = gameObject.GetComponent<CharacterController>();
        }
    }
}