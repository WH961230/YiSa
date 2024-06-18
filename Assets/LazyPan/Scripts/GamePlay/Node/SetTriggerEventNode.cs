using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using LazyPan;
using NodeGraphProcessor.Examples;
using UnityEngine.Events;

[System.Serializable, NodeMenuItem("Custom/设置触发器事件节点")]
public class SetTriggerEventNode : LinearConditionalNode {
    [Input(name = "输入可触碰游戏物体")] public GameObject inputTriggerGameObject;
    [Input(name = "输入触碰进入事件")] public UnityAction<Collider> inputTriggerEnterUnityAction;
    [Input(name = "输入触碰离开事件")] public UnityAction<Collider> inputTriggerExitUnityAction;
    [Output(name = "输出触碰进入事件")] public bool outputTriggerEnter;
    [Output(name = "输出触碰离开事件")] public bool outputTriggerExit;
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
        triggerComp.OnTriggerExitEvent.AddListener(InputTriggerExit);
    }

    private void InputTriggerEnter(Collider collider) {
        inputTriggerEnterUnityAction?.Invoke(collider);
        outputTriggerEnter = true;
    }

    private void InputTriggerExit(Collider collider) {
        inputTriggerExitUnityAction?.Invoke(collider);
        outputTriggerExit = true;
    }

    public override IEnumerable<ConditionalNode> GetExecutedNodes() {
        throw new System.NotImplementedException();
    }
}