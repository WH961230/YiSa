using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LazyPan {
#if UNITY_EDITOR
    public class ToolSet : EditorWindow {
        [MenuItem("[LP] 工具组/[LP] 点位工具组/[LP] 收集点位组")]
        public static void CollectPointsToSetting() {
            if (Selection.gameObjects.Length == 0 || Selection.gameObjects.Length > 1) {
                EditorUtility.DisplayDialog("错误", "请选择有且仅有一个物体作为父物体!", "了解");
                return;
            }
            GameObject selectGameObject = Selection.gameObjects[0];
            if (selectGameObject.transform.childCount == 0) {
                EditorUtility.DisplayDialog("错误", "父物体没有子物体,请检查选中物体!", "了解");
                return;
            }
            /*创建点位配置*/
            string settingPath = string.Concat("Assets", Loader.LoadGameSetting().LocationInformationSettingPath, selectGameObject.name, ".asset");
            LocationInformationSetting setting = AssetDatabase.LoadAssetAtPath(settingPath, typeof(LocationInformationSetting)) as LocationInformationSetting;
            if (setting == null) {
                setting = CreateInstance<LocationInformationSetting>();
                AssetDatabase.CreateAsset(setting, settingPath);
            }

            setting.locationInformationDatas = new List<LocationInformationData>();
            for (int i = 0; i < selectGameObject.transform.childCount; i++) {
                setting.name = selectGameObject.name;
                Transform childTran = selectGameObject.transform.GetChild(i);
                if (childTran.name == "TerrainIgnore") {
                    continue;
                }
                LocationInformationData locationInformationData = new LocationInformationData();
                locationInformationData.Position = childTran.position;
                locationInformationData.Rotation = childTran.rotation.eulerAngles;
                setting.locationInformationDatas.Add(locationInformationData);
            }

            AssetDatabase.SaveAssets();
        }

        [MenuItem("[LP] 工具组/[LP] 点位工具组/[LP] 创建点位组")]
        public static void CreatePointsToSetting() {
            if (Selection.activeObject == null) {
                EditorUtility.DisplayDialog("错误", "请选择一个点位配置!", "了解");
                return;
            }
            LocationInformationSetting locationInformationSetting = Selection.activeObject as LocationInformationSetting;
            if (locationInformationSetting == null) {
                EditorUtility.DisplayDialog("错误", "请选择一个点位配置文件!", "了解");
                return;
            }
            GameObject parentGameObject = new GameObject(locationInformationSetting.name);
            if (locationInformationSetting.TerrainGo != null) {
                GameObject terrain = Instantiate(locationInformationSetting.TerrainGo, parentGameObject.transform);
                terrain.name = "TerrainIgnore";
            }
            for (int i = 0; i < locationInformationSetting.locationInformationDatas.Count; i++) {
                LocationInformationData data = locationInformationSetting.locationInformationDatas[i];
                string markSign = "Obj_MarkLocationInformation";
                GameObject childGameObject = Loader.LoadGo(string.Concat(markSign, "_", i),
                    string.Concat("Tool/", markSign), parentGameObject.transform,
                    true);
                childGameObject.transform.position = data.Position;
                childGameObject.transform.rotation = Quaternion.Euler(data.Rotation);
            }
        }

        [MenuItem("[LP] 工具组/[LP] 点位工具组/[LP] 说明")]
        public static void Explain() {
            string title = "点位工具组 说明";
            string content = "收集点位组：\n收集选中的父物体之下的所有子物体的位置和方向数据录入点位配置，配置名为父物体的名字\n\n";
            content += "创建点位组：\n创建选中的配置数据的游戏物体，父物体为配置名，根据点位配置赋值子物体的位置和方向数据";
            EditorUtility.DisplayDialog(title, content, "了解");
        }
    }
#endif
}