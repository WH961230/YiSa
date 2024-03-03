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
        }
    }
}