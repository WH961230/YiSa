using UnityEngine;

namespace LazyPan {
    public class Behaviour_IncreaseAttackSpeed : Behaviour {
        private bool isOnceBuff;
        public Behaviour_IncreaseAttackSpeed(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.TryGetBuffSetting(BehaviourSign, out BuffSetting buffSetting);
            entity.EntityData.AttackInterval -= float.Parse(buffSetting.ParamStrs[0]);
            entity.EntityData.AttackInterval = Mathf.Max(0.01f, entity.EntityData.AttackInterval);
            MessageRegister.Instance.Dis(MessageCode.RefreshEntityUI, entity);
            isOnceBuff = true;
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            if (isOnceBuff) {
                BehaviourRegister.Instance.UnRegisterBehaviour(entity.ID, BehaviourSign);
                isOnceBuff = false;
            }
        }
        
        public override void OnClear() {
            base.OnClear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}