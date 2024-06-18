using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using UnityEngine.Events;

[System.Serializable, NodeMenuItem("Custom/设置碰撞体日志节点")]
public class SetTriggerColliderLogNode : BaseNode {
    [Output(name = "碰撞体触发事件")] public UnityAction<Collider> outputUnityAction;

    public override string name => "设置碰撞体日志节点";
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
        outputUnityAction += TriggerColliderLog;
    }

    private void TriggerColliderLog(Collider collider) {
        Debug.LogError(collider.name);
    }
}