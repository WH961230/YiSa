using System;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using LazyPan;
using NodeGraphProcessor.Examples;
using UnityEngine.InputSystem;

[System.Serializable, NodeMenuItem("Custom/获取输入节点")]
public class GetInputNode : ConditionalNode {
    [Input(name = "输入按键映射")] public string inputKeyMap;
    [Output(name = "输出方位")] public Vector3 outputDirection;
    public override string name => "获取输入节点";
    public override IEnumerable<ConditionalNode> GetExecutedNodes() => throw new Exception();

    protected override void Process() {
        InputRegister.Instance.Load(inputKeyMap, InputEvent);
    }

    private void InputEvent(InputAction.CallbackContext callback) {
        Vector2 inputVec = callback.ReadValue<Vector2>();
        outputDirection = new Vector3(inputVec.x, 0, inputVec.y);
    }
}