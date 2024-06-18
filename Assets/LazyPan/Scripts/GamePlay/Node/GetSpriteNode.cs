using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using LazyPan;

[System.Serializable, NodeMenuItem("Custom/获取精灵图片节点")]
public class GetSpriteNode : BaseNode {
    [Input(name = "输入精灵图片地址")] public string inputSpritePath;

    [Output(name = "输出精灵图片")] public Sprite output;

    public override string name => "获取精灵图片节点";

    protected override void Process() {
        
    }
}