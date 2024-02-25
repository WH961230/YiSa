using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LazyPan {
    public class Generate : EditorWindow {
#if UNITY_EDITOR
        private string behaviourName;

        [MenuItem("Assets/Create/LazyPan/生成行为")]
        public static void GenerateBehaviour() {
            EditorWindow window = GetWindow(typeof(Generate), true, "快速生成行为配置");
            window.position = new Rect(new Vector2(Screen.width, Screen.height), new Vector2(500, 300));
            window.Show();
            window.Focus();
        }

        private void OnGUI() {
            behaviourName = EditorGUILayout.TextField("输入行为配置名 ：", behaviourName);
            if (GUILayout.Button("生成行为脚本")) {
                CreateBehaviourScript("Assets/LazyPan/Bundles/Configs/Txt/GenerateBehaviourTemplate.txt",
                    "Assets/LazyPan/Scripts/GamePlay/Behaviour/", behaviourName, "Behaviour_", "");
            }
        }

        [MenuItem("Assets/Create/LazyPan/生成配置脚本")]
        public static void GenerateConfig() {
            Object obj = Selection.objects[0];
            GameSetting gameSetting = Loader.LoadSetting();
            ReadCSV.Instance.Read(obj.name, out string content, out string[] lines);
            GenerateScript(obj.name, gameSetting, lines);
        }

        private static void GenerateScript(string className, GameSetting gameSetting, string[] lines) {
            string property = "";
            string readContent = "";
            string[] propertyName = lines[0].Split(',');
            string[] typeName = lines[1].Split(',');

            for (int i = 0; i < propertyName.Length; i++) {
                property += string.Concat("\t\t", "public ", typeName[i], " ", propertyName[i], ";", "\n");
            }

            for (int i = 0; i < propertyName.Length; i++) {
                string tmpTypeName = typeName[i];
                if (tmpTypeName == "string") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " = values[", i, "];", "\n");
                } else if (tmpTypeName == "int") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " = int.Parse(values[", i, "])", ";", "\n");
                } else if (tmpTypeName == "float") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " = float.Parse(values[", i, "])", ";", "\n");
                } else if (tmpTypeName == "bool") {
                    readContent += string.Concat("\t\t\t\t", propertyName[i], " =  int.Parse(values[", i, "]) == 1", ";", "\n");
                }
            }

            string inputPath = string.Concat(Application.dataPath, gameSetting.TxtPath, "GenerateConfigTemplate.txt");
            string outputPath = string.Concat(Application.dataPath, gameSetting.ConfigScriptPath);
            CreateScript(inputPath, outputPath, className, property, readContent, "", "");
        }

        private static bool CreateScript(string inputPath, string outputPath, string className, string property,
            string readContent, string front, string end) {
            if (inputPath.EndsWith(".txt")) {
                var streamReader = new StreamReader(inputPath);
                var log = streamReader.ReadToEnd();
                streamReader.Close();
                log = Regex.Replace(log, "#ClassName#", className);
                log = Regex.Replace(log, "#Property#", property);
                log = Regex.Replace(log, "#ReadContent#", readContent);
                var createPath = $"{outputPath}{front}{className}{end}.cs";
                var streamWriter = new StreamWriter(createPath, false, new UTF8Encoding(true, false));
                streamWriter.Write(log);
                streamWriter.Close();
                AssetDatabase.ImportAsset(createPath);
                AssetDatabase.Refresh();
                return true;
            }

            return false;
        }

        public static bool CreateBehaviourScript(string inputPath, string outputPath, string className, string front, string end) {
            if (inputPath.EndsWith(".txt")) {
                var streamReader = new StreamReader(inputPath);
                var log = streamReader.ReadToEnd();
                streamReader.Close();
                log = Regex.Replace(log, "#ClassName#", className);
                var createPath = $"{outputPath}{front}{className}{end}.cs";
                var streamWriter = new StreamWriter(createPath, false, new UTF8Encoding(true, false));
                streamWriter.Write(log);
                streamWriter.Close();
                AssetDatabase.ImportAsset(createPath);
                return true;
            }

            return false;
        }
#endif
    }
}