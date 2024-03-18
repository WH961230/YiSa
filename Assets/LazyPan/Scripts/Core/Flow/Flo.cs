using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace LazyPan {
    public class Flo : Singleton<Flo> {
        Dictionary<Type, Flow> flows = new Dictionary<Type, Flow>();
        public void Preload() {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneConfig sceneConfig = SceneConfig.Get(sceneName);
            Type type = Assembly.Load("Assembly-CSharp").GetType(string.Concat("LazyPan.", sceneConfig.Flow));
            Flow flow = (Flow) Activator.CreateInstance(type);
            flow.Init(null);
            flows.Clear();
            flows.Add(type, flow);
        }

        public void UnPreload() {
            foreach (Flow flow in flows.Values) {
                flow.Clear();
            }
            flows.Clear();
        }

        public bool GetFlow<T>(out T flow) where T : Flow {
            if (flows.ContainsKey(typeof(T))) {
                flow = (T)flows[typeof(T)];
                return true;
            }

            flow = default;
            return false;
        }
    }
}