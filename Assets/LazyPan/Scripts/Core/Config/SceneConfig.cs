using System;
using System.Collections.Generic;

namespace LazyPan {
    public class SceneConfig {
		public string Sign;
		public string Description;
		public string DirPath;

        private static bool isInit;
        private static string content;
        private static string[] lines;
        private static Dictionary<string, SceneConfig> dics = new Dictionary<string, SceneConfig>();

        public SceneConfig(string line) {
            try {
                string[] values = line.Split(',');
				Sign = values[0];
				Description = values[1];
				DirPath = values[2];

            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Init() {
            if (isInit) {
                return;
            }
            ReadCSV.Instance.Read("SceneConfig", out content, out lines);
            dics.Clear();
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    SceneConfig config = new SceneConfig(lines[i]);
                    dics.Add(config.Sign, config);
                }
            }

            isInit = true;
        }

        public static SceneConfig Get(string sign) {
            if (dics.TryGetValue(sign, out SceneConfig config)) {
                return config;
            }

            return null;
        }

        public static List<string> GetKeys() {
              if (!isInit) {
                   Init();
              }
              var keys = new List<string>();
              keys.AddRange(dics.Keys);
              return keys;
        }
    }
}