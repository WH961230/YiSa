using UnityEngine;

namespace LazyPan {
    public class Behaviour {
        public string BehaviourSign;
        public Entity entity;

        protected Behaviour(Entity entity, string behaviourSign) {
            this.entity = entity;
            BehaviourSign = behaviourSign;
#if UNITY_EDITOR
            ConsoleEx.Instance.Content("log", $"ID:{entity.ID} 注册行为:{BehaviourConfig.Get(BehaviourSign).Name}");
#endif
        }

        public virtual void Clear() {
#if UNITY_EDITOR
            ConsoleEx.Instance.Content("log", $"ID:{entity.ID} 注销行为:{BehaviourConfig.Get(BehaviourSign).Name}");
#endif
        }
    }
}