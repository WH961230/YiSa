using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public abstract class Flow : IFlow {
        public Flow BaseFlow;
        public Flow CurrentFlow;
        public Dictionary<Type, Flow> FlowDic = new Dictionary<Type, Flow>();

        //进入流程
        public virtual void OnInit(Flow baseFlow) {
            BaseFlow = baseFlow;
        }

        //结束流程
        public virtual void OnClear() {
            if (CurrentFlow != null) {
                CurrentFlow.OnClear();
                CurrentFlow = null;
            }

            foreach (Flow tempFlow in FlowDic.Values) {
                tempFlow.OnClear();
            }
            FlowDic.Clear();
        }

        //切换流程
        public virtual void ChangeFlow<T>() {
            if (CurrentFlow != null) {
                CurrentFlow.OnClear();
            }

            if (FlowDic.ContainsKey(typeof(T))) {
                CurrentFlow = FlowDic[typeof(T)];
                CurrentFlow.OnInit(this);
            }

            if (FlowDic.Count == 0) {
                Debug.LogError(string.Concat(typeof(T), "字典为空!"));
            }
        }

        public virtual T GetFlow<T>() where T : Flow {
            if (FlowDic.ContainsKey(typeof(T))) {
                return FlowDic[typeof(T)] as T;
            }

            return null;
        }

        public virtual void EndFlow() {
            OnClear();
        }
    }
}