using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Auto_Motion : Behaviour {
        private CharacterController characterController;
        public Behaviour_Auto_Motion(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            //设置检测频率雇佣时间
            entity.EntityData.BaseRuntimeData.CurDetectFrequencyDeployTime =
                entity.EntityData.BaseRuntimeData.DefDetectFrequency;
            characterController = Cond.Instance.Get<CharacterController>(entity, Label.CHARACTERCONTROLLER);
            Data.Instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        private void OnUpdate() {
            //检测目标
            DetectTarget();
            //向目标移动
            MotionToTarget();
        }

        private void DetectTarget() {
            if (entity.EntityData.BaseRuntimeData.CurDetectFrequencyDeployTime > 0) {
                entity.EntityData.BaseRuntimeData.CurDetectFrequencyDeployTime -= Time.deltaTime;
                return;
            }
            entity.EntityData.BaseRuntimeData.CurDetectFrequencyDeployTime =
                entity.EntityData.BaseRuntimeData.DefDetectFrequency;
            GetTargetEntity();
        }

        private void MotionToTarget() {
            /*朝目标移动*/
            bool findTarget = Cond.Instance.GetEntityByID(entity.EntityData.BaseRuntimeData.CurAttackTargetEntityID,
                out Entity targetEntity);
            if (findTarget) {
                Vector3 dir = (Cond.Instance.Get<Transform>(targetEntity, Label.BODY).position -
                               Cond.Instance.Get<Transform>(entity, Label.BODY).position).normalized;
                characterController.Move(dir * Time.deltaTime * entity.EntityData.BaseRuntimeData.CurMotionSpeed);
            }
            //碰撞后 自爆伤害
        }

        private void GetTargetEntity() {
            //先判断玩家是否在检测距离内 检测内 先找玩家 检测距离外下一步
            bool findPlayerEntity = Cond.Instance.GetEntitiesByTypeWithinDistance("Player",
                Cond.Instance.Get<Transform>(entity, Label.BODY).position,
                entity.EntityData.BaseRuntimeData.DefDetectDistance, out List<Entity> entities);
            if (findPlayerEntity) {
                entity.EntityData.BaseRuntimeData.CurAttackTargetEntityID = entities[0].ID;
            } else {
                //判断塔是否激活 如果塔未激活 找玩家 塔激活了 找塔
                bool findTower = Cond.Instance.GetTowerEntity(out Entity towerEntity);
                if (findTower) {
                    if (towerEntity.EntityData.BaseRuntimeData.CurEnergy > 0) {
                        entity.EntityData.BaseRuntimeData.CurAttackTargetEntityID = towerEntity.ID;
                    } else {
                        entity.EntityData.BaseRuntimeData.CurAttackTargetEntityID = Cond.Instance.GetPlayerEntity().ID;
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(OnUpdate);
        }
    }
}