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
            }

            bool getEntity = Cond.Instance.GetTowerEntity(out Entity towerEntity);
            if (getEntity) {
                Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                Cond.Instance.Get<Slider>(info, Label.EXP).value = towerEntity.EntityData.BaseRuntimeData.CurExp /
                                                                   towerEntity.EntityData.BaseRuntimeData.CurExpMax;
            }
        }

        private void Pick(Collider arg0) {
            if (arg0.gameObject.layer == LayerMask.NameToLayer("Drop")) {
                Object.Destroy(arg0.gameObject);
                bool getEntity = Cond.Instance.GetTowerEntity(out Entity towerEntity);
                if (getEntity) {
                    towerEntity.EntityData.BaseRuntimeData.CurExp += 1;
                    towerEntity.EntityData.BaseRuntimeData.CurExp = Mathf.Min(towerEntity.EntityData.BaseRuntimeData.CurExp,
                        towerEntity.EntityData.BaseRuntimeData.CurExpMax);
                    Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
                    Cond.Instance.Get<Slider>(info, Label.EXP).value = towerEntity.EntityData.BaseRuntimeData.CurExp /
                                                                       towerEntity.EntityData.BaseRuntimeData.CurExpMax;
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.TRIGGER)).OnTriggerEnterEvent
                .RemoveListener(Pick);
        }
    }
}