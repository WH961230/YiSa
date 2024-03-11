using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Shoot : Behaviour {
        public Behaviour_Auto_Shoot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(OnShootUpdate);
        }

        private void OnShootUpdate() {
            if (entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime > 0) {
                entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime -= Time.deltaTime;
            } else {
                Entity robotEntity = Cond.Instance.GetTypeRandEntity(Label.ROBOT);
                if (robotEntity != null) {
                    GameObject template = Loader.LoadGo("弹药", "Obj/Fight/Obj_Fx_Bullet", Data.Instance.ObjRoot, true);
                    Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
                    template.transform.position = bulletMuzzle.position;
                    template.transform.forward = (Cond.Instance.Get<Transform>(robotEntity, Label.HIT).position - bulletMuzzle.position).normalized;
                    template.GetComponent<Comp>().OnParticleCollisionEvent.AddListener(OnTriggerEnter);
                }
                entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime =
                    entity.EntityData.BaseRuntimeData.DefAttackIntervalTime;
            }
        }

        private void OnTriggerEnter(GameObject go) {
            if (go.layer != LayerMask.NameToLayer("BeHit")) {
                return;
            }
            if (Data.Instance.TryGetEntityByBodyPrefabID(go.GetInstanceID(), out Entity tmpEntity)) {
                tmpEntity.EntityData.BaseRuntimeData.CurHealth -= 10 /*伤害*/;
                Debug.Log($"curHealth:{entity.EntityData.BaseRuntimeData.CurHealth}");
                /*掉血表现*/
                GameObject template = Loader.LoadGo("掉血", "Obj/Fight/Obj_Fx_BeHit", Data.Instance.ObjRoot, true);
                Transform squirt = Cond.Instance.Get<Transform>(tmpEntity, Label.SQUIRT);
                template.transform.position = squirt.position;
                template.transform.rotation = squirt.rotation;
                /*击退表现*/
                tmpEntity.EntityData.BaseRuntimeData.CurKnockbackDir = (
                    Cond.Instance.Get<Transform>(tmpEntity, Label.BODY).position -
                    Cond.Instance.Get<Transform>(entity, Label.BODY).position).normalized;
                tmpEntity.EntityData.BaseRuntimeData.CurKnockbackDeployTime =
                    tmpEntity.EntityData.BaseRuntimeData.DefKnockbackTime;
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnShootUpdate);
        }
    }
}