using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_SelfDetonationAttack : Behaviour {
        public Behaviour_Auto_SelfDetonationAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP)).OnControllerColliderHitEvent
                .AddListener(OnHitTrigger);
        }

        private void OnHitTrigger(ControllerColliderHit hit) {
            if (hit.gameObject.layer != LayerMask.NameToLayer("FriendSideBeHit")) {
                return;
            }

            if (Data.Instance.TryGetEntityByBodyPrefabID(hit.gameObject.GetInstanceID(), out Entity tmpEntity)) {
                Debug.Log("hit GO:" + tmpEntity.EntityData.BaseRuntimeData.Type);
                tmpEntity.EntityData.BaseRuntimeData.CurHealth -= entity.EntityData.BaseRuntimeData.CurAttack;
                Debug.Log($"玩家血量:{tmpEntity.EntityData.BaseRuntimeData.CurHealth}");
                bool hasFightFlow = Flo.Instance.GetFlow(out Flow_Battle fight);
                if (hasFightFlow) {
                    fight.RemoveRobot(entity);
                    fight.AddRobot();
                    if (tmpEntity.EntityData.BaseRuntimeData.CurHealth <= 0) {
                        Debug.LogError("游戏结束");
                        fight.Next();
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
        }
    }
}