using GraphProcessor;
using LazyPan;
using UnityEngine;

[System.Serializable, NodeMenuItem("Custom/获取相机节点")]
public class GetCameraNode : BaseNode {
    [Output(name = "相机游戏物体")] public GameObject outputCamera;
    public override string name => "获取相机节点";

    protected override void Process() {
        Entity cameraEntity = Cond.Instance.GetCameraEntity();
        if (cameraEntity != null) {
            outputCamera = Cond.Instance.Get<GameObject>(cameraEntity, Label.BODY);
        }
    }
}