namespace LazyPan {
    public class Behaviour_IncreaseBaseAttack : Behaviour {
        private bool isOnceBuff;
        public Behaviour_IncreaseBaseAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.TryGetBuffSetting(BehaviourSign, out BuffSetting buffSetting);
            entity.EntityData.AttackBase = entity.EntityData.AttackBase + int.Parse(buffSetting.ParamStrs[0]);
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