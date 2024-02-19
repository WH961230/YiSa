namespace LazyPan {
    public class Behaviour_IncreaseAmmunitionAmount : Behaviour {
        private bool isOnceBuff;
        public Behaviour_IncreaseAmmunitionAmount(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.TryGetBuffSetting(BehaviourSign, out BuffSetting buffSetting);
            entity.EntityData.BulletNum += int.Parse(buffSetting.ParamStrs[0]);
            entity.EntityData.BulletMaxNum += int.Parse(buffSetting.ParamStrs[0]);
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