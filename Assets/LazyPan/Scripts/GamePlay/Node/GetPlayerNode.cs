using GraphProcessor;
using LazyPan;
using UnityEngine;

[System.Serializable, NodeMenuItem("Custom/获取玩家节点")]
public class GetPlayerNode : BaseNode {
    [Output(name = "玩家游戏物体")] public GameObject outputPlayer;
    public override string name => "获取玩家节点";

    protected override void Process() {
        Entity playerEntity = Cond.Instance.GetPlayerEntity();
        if (playerEntity != null) {
            outputPlayer = Cond.Instance.Get<GameObject>(playerEntity, Label.BODY);
        }
    }
}