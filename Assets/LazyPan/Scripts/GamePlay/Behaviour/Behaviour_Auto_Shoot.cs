using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Shoot : Behaviour {
        public Behaviour_Auto_Shoot(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(OnShootUpdate);
        }

        private void OnShootUpdate() {
            if (entity.EntityData.BaseRuntimeData.CurEnergy > 0) {
                if (entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime > 0) {
                    entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime -= Time.deltaTime;
                } else {
                    bool findRobotEntity = Cond.Instance.GetRandEntityByTypeWithinDistance(Label.ROBOT,
                        Cond.Instance.Get<Transform>(entity, Label.BODY).position,
                        entity.EntityData.BaseRuntimeData.CurDetectDistance, out Entity robotEntity);
                    if (findRobotEntity && robotEntity.EntityData.BaseRuntimeData.CurHealth > 0) {
                        GameObject template = Loader.LoadGo("弹药", "Common/Obj_Fx_Bullet", Data.Instance.ObjRoot, true);
                        Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
                        template.transform.position = bulletMuzzle.position;
                        template.transform.forward = (Cond.Instance.Get<Transform>(robotEntity, Label.HIT).position - bulletMuzzle.position).normalized;
                        template.GetComponent<Comp>().OnParticleCollisionEvent.AddListener(OnTriggerEnter);
                        template.GetComponent<Comp>().OnParticleCollisionEvent.AddListener((aaa) => {
                            Object.Destroy(template);
                        });
                    }
                    entity.EntityData.BaseRuntimeData.CurAttackIntervalDeployTime =
                        entity.EntityData.BaseRuntimeData.DefAttackIntervalTime;
                }
            }
        }

        private void OnTriggerEnter(GameObject go) {
            if (go.layer != LayerMask.NameToLayer("EnemySideBeHit")) {
                return;
            }
            if (Data.Instance.TryGetEntityByBodyPrefabID(go.GetInstanceID(), out Entity tmpEntity)) {
                if (tmpEntity.EntityData.BaseRuntimeData.CurHealth > 0) {
                    tmpEntity.EntityData.BaseRuntimeData.CurHealth -= entity.EntityData.BaseRuntimeData.CurAttack /*伤害*/;
                    if (tmpEntity.EntityData.BaseRuntimeData.CurHealth <= 0) {
                        bool hasFightFlow = Flo.Instance.GetFlow(out Flow_Battle battleFlow);
                        if (hasFightFlow) {
                            tmpEntity.EntityData.BaseRuntimeData.DeathDrop = 2;
                            MessageRegister.Instance.Dis(MessageCode.Dead, tmpEntity);
                            MessageRegister.Instance.Dis(MessageCode.DeadRecycle, tmpEntity);
                        }
                        return;
                    }
                    /*掉血表现*/
                    GameObject template = Loader.LoadGo("掉血", "Common/Obj_Fx_BeHit", Data.Instance.ObjRoot, true);
                    Transform squirt = Cond.Instance.Get<Transform>(tmpEntity, Label.SQUIRT);
                    template.transform.position = squirt.position;
                    template.transform.rotation = squirt.rotation;
                    /*击退表现*/
                    tmpEntity.EntityData.BaseRuntimeData.CurKnockbackDir = (
                        Cond.Instance.Get<Transform>(tmpEntity, Label.BODY).position -
                        Cond.Instance.Get<Transform>(entity, Label.BODY).position).normalized;
                    tmpEntity.EntityData.BaseRuntimeData.CurKnockbackDeployTime =
                        tmpEntity.EntityData.BaseRuntimeData.DefKnockbackTime;
                    /*受击材质高亮*/
                    Material mat = Cond.Instance.Get<Renderer>(tmpEntity, Label.Assemble(Label.BODY, Label.RENDERER)).material;
                    mat.SetColor("_EmissionColor", Color.white);
                    mat.EnableKeyword("_EMISSION");
                    /*复原*/
                    ClockUtil.Instance.AlarmAfter(0.1f, () => {
                        Material mat = Cond.Instance
                            .Get<Renderer>(tmpEntity, Label.Assemble(Label.BODY, Label.RENDERER)).material;
                        mat.SetColor("_EmissionColor", Color.black);
                        mat.EnableKeyword("_EMISSION");
                        });
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnShootUpdate);
        }
    }
}