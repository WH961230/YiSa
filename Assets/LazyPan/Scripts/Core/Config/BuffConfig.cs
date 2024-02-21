using System;
using System.Collections.Generic;

namespace LazyPan {
    public class BuffConfig {
		public string Sign;
		public string Description;

        private static bool isInit;
        private static string content;
        private static string[] lines;
        private static Dictionary<string, BuffConfig> dics = new Dictionary<string, BuffConfig>();

        public BuffConfig(string line) {
            try {
                string[] values = line.Split(',');
				Sign = values[0];
				Description = values[1];

            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Init() {
            if (isInit) {
                return;
            }
            ReadCSV.Instance.Read("BuffConfig", out content, out lines);
            dics.Clear();
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    BuffConfig config = new BuffConfig(lines[i]);
                    dics.Add(config.Sign, config);
                }
            }

            isInit = true;
        }

        public static void Clear() {
            isInit = false;
            dics.Clear();
            lines = null;
        }

        public static BuffConfig Get(string sign) {
            if (dics.TryGetValue(sign, out BuffConfig config)) {
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