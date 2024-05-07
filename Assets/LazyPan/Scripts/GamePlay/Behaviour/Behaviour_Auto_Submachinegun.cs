﻿using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Submachinegun : Behaviour {
        private BuffSettingInfo buffSettingInfo;
        private float deploy;
        private float attackIntervalTime;
        public Behaviour_Auto_Submachinegun(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(Submachinegun);
            Loader.LoadSetting().BuffSetting
                .GetSettingBySign(behaviourSign, out buffSettingInfo);

            buffSettingInfo.GetParam("AttackIntervalTime", out string attackintervaltime);
            attackIntervalTime = float.Parse(attackintervaltime);
        }

        private void Submachinegun() {
            if (entity.EntityData.BaseRuntimeData.TowerInfo.Energy > 0) {
                if (deploy > 0) {
                    deploy -= Time.deltaTime;
                } else {
                    bool findRobotEntity = Cond.Instance.GetRandEntityByTypeWithinDistance(Label.ROBOT,
                        Cond.Instance.Get<Transform>(entity, Label.BODY).position,
                        Loader.LoadSetting().TowerSetting.AttackRange, out Entity robotEntity);
                    if (findRobotEntity && robotEntity.EntityData.BaseRuntimeData.RobotInfo.HealthPoint > 0) {
                        buffSettingInfo.GetParam("Bullet", out string bullet);
                        GameObject template = Loader.LoadGo("弹药", string.Concat("Common/", bullet), Data.Instance.ObjRoot, true);
                        Transform bulletMuzzle = Cond.Instance.Get<Transform>(entity, Label.MUZZLE);
                        template.transform.position = bulletMuzzle.position;
                        template.transform.forward = (Cond.Instance.Get<Transform>(robotEntity, Label.HIT).position - bulletMuzzle.position).normalized;
                        Comp templateComp = template.GetComponent<Comp>();
                        templateComp.OnParticleCollisionEvent.RemoveAllListeners();
                        templateComp.OnParticleCollisionEvent.AddListener(OnTriggerEnter);
                        templateComp.OnParticleCollisionEvent.AddListener((aaa) => {
                            Object.Destroy(template);
                        });
                    }
                    deploy = attackIntervalTime;
                }
            }
        }

        private void OnTriggerEnter(GameObject go) {
            if (go.layer != LayerMask.NameToLayer("EnemySideBeHit")) {
                return;
            }
            if (Data.Instance.TryGetEntityByBodyPrefabID(go.GetInstanceID(), out Entity tmpEntity)) {
                if (tmpEntity.EntityData.BaseRuntimeData.Type != "Robot") {
                    return;
                }
                if (tmpEntity.EntityData.BaseRuntimeData.RobotInfo.HealthPoint > 0) {
                    tmpEntity.EntityData.BaseRuntimeData.RobotInfo.BeAttackType = 1;
                    MessageRegister.Instance.Dis(MessageCode.BeInjuried, tmpEntity, Loader.LoadSetting().TowerSetting.Attack);
                    /*掉血表现*/
                    GameObject template = Loader.LoadGo("掉血", "Common/Obj_Fx_BeHit", Data.Instance.ObjRoot, true);
                    Transform squirt = Cond.Instance.Get<Transform>(tmpEntity, Label.SQUIRT);
                    template.transform.position = squirt.position;
                    template.transform.rotation = squirt.rotation;
                    /*击退表现*/
                    MessageRegister.Instance.Dis(MessageCode.BeHit, entity, tmpEntity);
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
            Data.Instance.OnUpdateEvent.RemoveListener(Submachinegun);
        }
    }
}