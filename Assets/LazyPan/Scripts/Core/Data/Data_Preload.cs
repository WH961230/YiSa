using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LazyPan {
    public partial class Data {
        public Dictionary<string, Queue<GameObject>> PreloadGoDic = new Dictionary<string, Queue<GameObject>>();
        //获取预加载物体 标识
        public void GetPreloadGo(string sign, out GameObject preloadGo) {
            //预加载的 Key 存在
            if (PreloadGoDic.ContainsKey(sign)) {
                Queue<GameObject> preloadQueueGo = PreloadGoDic[sign];
                //预加载队列不为空
                if (preloadQueueGo.Count > 0) {
                    preloadGo = preloadQueueGo.Dequeue();
                    preloadGo.SetActive(true);
                    return;
                }

                PreloadGoDic[sign] = PreloadGo(sign, 20);
                preloadGo = PreloadGoDic[sign].Dequeue();
                preloadGo.SetActive(true);
                return;
            }

            PreloadGoDic.Add(sign, PreloadGo(sign, 20));
            preloadGo = PreloadGoDic[sign].Dequeue();
            preloadGo.SetActive(true);
        }

        private Queue<GameObject> PreloadGo(string sign, int count) {
            Queue<GameObject> preloadGo = new Queue<GameObject>();
            while (count > 0) {
                GameObject go = GetGo(sign);
                go.name = string.Concat("预加载物体_", sign, "_", go.GetInstanceID());
                go.SetActive(false);
                preloadGo.Enqueue(go);
                count--;
            }

            return preloadGo;
        }

        public GameObject GetGo(string sign) {
            return Loader.LoadGo("", string.Concat(SceneConfig.Get(SceneManager.GetActiveScene().name).DirPath, sign),
                Obj.Instance.GetRoot(ObjConfig.Get(sign).RootTypeSign), true);
        }

        public void RecycleGo(string sign, GameObject preloadGo) {
            //如果预加载物体
            if (PreloadGoDic.ContainsKey(sign)) {
                preloadGo.name = string.Concat("预加载物体_", sign, "_", preloadGo.GetInstanceID());
                preloadGo.SetActive(false);
                PreloadGoDic[sign].Enqueue(preloadGo);
            }
        }
    }
}