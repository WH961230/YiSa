using UnityEngine;
using GraphProcessor;

[System.Serializable, NodeMenuItem("Custom/角色控制器移动节点")]
public class SetCharacterControllerMoveNode : BaseNode {
    [Input(name = "输入速度")] public float inputSpeed;
    [Input(name = "输入方位")] public Vector3 inputDirection;
    [Input(name = "角色控制器")] public CharacterController inputCharacterController;
    [Input(name = "输入移动方向对齐游戏物体")] public GameObject inputAlignGameObject;
    public 角色移动方向类型 角色移动方向类型;
    public override string name => "角色控制器移动节点";

    protected override void Process() {
        if (inputCharacterController != null) {
            Vector3 dir = Vector3.zero;
            switch (角色移动方向类型) {
                case 角色移动方向类型.世界绝对位置:
                    dir = Vector3.forward * inputDirection.z +
                          Vector3.right * inputDirection.x +
                          Vector3.up * inputDirection.y;
                    break;
                case 角色移动方向类型.物体相对位置:
                    if (inputAlignGameObject != null) {
                        dir = inputAlignGameObject.transform.forward * inputDirection.z +
                              inputAlignGameObject.transform.right * inputDirection.x +
                              inputAlignGameObject.transform.up * inputDirection.y;
                    }
                    break;
            }

            inputCharacterController.Move(dir * inputSpeed * Time.deltaTime);
        }
    }
}

public enum 角色移动方向类型 {
    世界绝对位置,
    物体相对位置,
}