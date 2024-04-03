using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_Pick : Behaviour {
        private Comp battleui;
        private Flow_Battle battleFlow;

        public Behaviour_Auto_Pick(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.TRIGGER)).OnTriggerEnterEvent
                .AddListener(Pick);
            bool isGetFlow = Flo.Instance.GetFlow(out battleFlow);
            if (isGetFlow) {
                battleui = battleFlow.GetUI();

                Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                Cond.Instance.Get<Slider>(info, Label.EXP).value =
                    Cond.Instance.GetPlayerEntity().EntityData.BaseRuntimeData.PlayerInfo.Experience /
                    Loader.LoadSetting().PlayerSetting.MaxExperience;
            }
        }

        /*拾取*/
        private void Pick(Collider arg0) {
            if (arg0.gameObject.layer == LayerMask.NameToLayer("Drop")) {
                Object.Destroy(arg0.gameObject);
                Entity playerEntity = Cond.Instance.GetPlayerEntity();
                playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience += 1;
                playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience = Mathf.Min(
                    playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience,
                    Loader.LoadSetting().PlayerSetting.MaxExperience);
                Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                Cond.Instance.Get<Slider>(info, Label.EXP).value =
                    playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience /
                    Loader.LoadSetting().PlayerSetting.MaxExperience;
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.TRIGGER)).OnTriggerEnterEvent
                .RemoveListener(Pick);
        }
    }
}