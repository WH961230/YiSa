using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using LazyPan;
using NodeGraphProcessor.Examples;
using UnityEngine.Events;

[System.Serializable, NodeMenuItem("Custom/设置触发器事件节点")]
public class SetTriggerEnterEventNode : ConditionalNode {
    [Input(name = "输入可触碰游戏物体")] public GameObject inputTriggerGameObject;
    [Input(name = "输入触碰事件")] public UnityAction<Collider> inputUnityAction;
    public override string name => "设置触发器事件节点";
    private bool isInit;

    protected override void Enable() {
        base.Enable();
        isInit = false;
    }

    protected override void Process() {
        if (!isInit) {
            ProcessStart();
            isInit = true;
        }
    }

    private void ProcessStart() {
        Comp triggerComp = inputTriggerGameObject.GetComponent<Comp>();
        triggerComp.OnTriggerEnterEvent.AddListener(InputTriggerEnter);
    }

    private void InputTriggerEnter(Collider collider) {
        inputUnityAction?.Invoke(collider);
    }

    public override IEnumerable<ConditionalNode> GetExecutedNodes() {
        throw new System.NotImplementedException();
    }
}