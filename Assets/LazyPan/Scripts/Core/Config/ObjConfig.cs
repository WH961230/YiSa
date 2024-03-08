﻿using System;
using System.Collections.Generic;

namespace LazyPan {
    public class ObjConfig {
		public string Sign;
		public string Type;
		public string Name;
		public string CreatureType;
		public string SetUpLocationInformationSign;
		public string SetUpBehaviourSign;

        private static bool isInit;
        private static string content;
        private static string[] lines;
        private static Dictionary<string, ObjConfig> dics = new Dictionary<string, ObjConfig>();

        public ObjConfig(string line) {
            try {
                string[] values = line.Split(',');
				Sign = values[0];
				Type = values[1];
				Name = values[2];
				CreatureType = values[3];
				SetUpLocationInformationSign = values[4];
				SetUpBehaviourSign = values[5];

            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Init() {
            if (isInit) {
                return;
            }
            ReadCSV.Instance.Read("ObjConfig", out content, out lines);
            dics.Clear();
            for (int i = 0; i < lines.Length; i++) {
                if (i > 2) {
                    ObjConfig config = new ObjConfig(lines[i]);
                    dics.Add(config.Sign, config);
                }
            }

            isInit = true;
        }

        public static ObjConfig Get(string sign) {
            if (dics.TryGetValue(sign, out ObjConfig config)) {
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