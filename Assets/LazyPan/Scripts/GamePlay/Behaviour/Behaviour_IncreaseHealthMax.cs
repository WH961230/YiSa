namespace LazyPan {
    public class Behaviour_IncreaseHealthMax : Behaviour {
        private bool isOnceBuff;
        public Behaviour_IncreaseHealthMax(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.TryGetBuffSetting(BehaviourSign, out BuffSetting buffSetting);
            entity.EntityData.HealthMax += int.Parse(buffSetting.ParamStrs[0]);
            entity.EntityData.Health = entity.EntityData.HealthMax;
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