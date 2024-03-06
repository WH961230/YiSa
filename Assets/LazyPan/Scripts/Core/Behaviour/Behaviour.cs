using UnityEngine;

namespace LazyPan {
    public class Behaviour {
        public string BehaviourSign;
        public Entity entity;

        protected Behaviour(Entity entity, string behaviourSign) {
            this.entity = entity;
            BehaviourSign = behaviourSign;
            Debug.LogFormat("ID:{0} 注册行为:{1}", entity.ID, behaviourSign);
        }

        public virtual void Clear() {
            Debug.LogFormat("ID:{0} 注销行为:{1}", entity.ID, BehaviourSign);
        }
    }
}