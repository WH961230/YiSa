using UnityEditor;
using UnityEngine;

namespace LazyPan {
    [CustomEditor(typeof(CollectCreatorRobotPoints))]
    public class CollectCreatorRobotPointsEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            CollectCreatorRobotPoints myScript = (CollectCreatorRobotPoints) target;
            if (GUILayout.Button("保存机器人出生点位")) {
                Setting setting = Loader.LoadAsset<Setting>(AssetType.ASSET, "Setting/Setting");
                setting.BaseCreatorPointsSetting.CreatorPoints.Clear();
                foreach (Transform tr in myScript.Points) {
                    setting.BaseCreatorPointsSetting.CreatorPoints.Add(tr.position);
                }
                Debug.Log("保存机器人出生点位成功！");
            }
        }
    }
}