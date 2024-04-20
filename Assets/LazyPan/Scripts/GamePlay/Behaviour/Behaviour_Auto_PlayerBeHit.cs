using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_PlayerBeHit : Behaviour {
        public Behaviour_Auto_PlayerBeHit(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg<Entity, int>(MessageCode.BeInjuried, BeHit);
        }

		/*受到伤害*/
		private void BeHit(Entity target, int damage) {
            if (target.ID == entity.ID) {
                entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint -= damage;
                entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint = Mathf.Max(
                    entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint,
                    0);

                Flo.Instance.GetFlow(out Flow_Battle battleflow);
                Comp info = Cond.Instance.Get<Comp>(battleflow.GetUI(), Label.INFO);
                Cond.Instance.Get<Slider>(info, Label.HEALTH).value = entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint /
                                                                      Loader.LoadSetting().PlayerSetting.MaxHealth;

                if (entity.EntityData.BaseRuntimeData.PlayerInfo.HealthPoint == 0) {
                    Next();
                }
            }
		}

        /*下一步*/
        private void Next() {
            Flo.Instance.GetFlow(out Flow_Battle flowBattle);
            flowBattle.Settlement();
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity, int>(MessageCode.BeInjuried, BeHit);
        }
    }
}