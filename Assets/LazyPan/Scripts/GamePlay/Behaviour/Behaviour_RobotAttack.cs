using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LazyPan {
    public enum RobotState {
        Idle,
        Move,
        Attack,
    }
    public class Behaviour_RobotAttack : Behaviour {
        private Entity playerEntity;
        private float robotMovementSpeed;
        private float robotRotateSpeed;
        private Animator robotAnimator;
        private PlayableDirector robotDirector;
        private TimelineAsset robotAttackTimelineAsset;
        private CharacterController robotCharacterController;
        private Comp robotAttackTriggerComp;
        private RobotState robotState;
        private float distanceSqr;

        public Behaviour_RobotAttack(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.TryGetEntityByObjType(ObjType.MainPlayer, out playerEntity);
            Data.Instance.OnUpdateEvent.AddListener(RobotEvent);
            robotMovementSpeed = Random.Range(entity.EntitySetting.RobotMovementSpeed.x, entity.EntitySetting.RobotMovementSpeed.y);
            robotRotateSpeed = Random.Range(entity.EntitySetting.RobotRotateSpeed.x, entity.EntitySetting.RobotRotateSpeed.y);
            robotAnimator = entity.Comp.Get<Animator>("Animator");
            robotDirector = entity.Comp.Get<PlayableDirector>("PlayableDirector");
            robotAttackTimelineAsset = entity.Comp.Get<TimelineAsset>("RobotAttackTimelineAsset");
            robotAttackTriggerComp = entity.Comp.Get<Comp>("RobotAttackTriggerComp");
            robotCharacterController = entity.Comp.Get<CharacterController>("CharacterController");
            robotAttackTriggerComp.OnTriggerEnterEvent.AddListener(OnRobotAttackTriggerEnter);
            robotState = RobotState.Idle;
        }

        private void OnRobotAttackTriggerEnter(Collider collider) {
            if (collider.tag == "Player") {
                Debug.Log(entity.ID + " 攻击到玩家");
                int damageVal = (int)(entity.EntityData.AttackBase * entity.EntityData.AttackRatio * entity.EntityData.AttackExtraRatio);
                MessageRegister.Instance.Dis(MessageCode.DamageEntity, playerEntity, damageVal);
            }
        }

        private void RobotEvent() {
            if (playerEntity == null || playerEntity.Prefab == null || entity.Prefab == null || entity.EntityData.Health <= 0) {
                return;
            }

            distanceSqr = (entity.Prefab.transform.position - playerEntity.Prefab.transform.position).sqrMagnitude;
            switch (robotState) {
                case RobotState.Idle:
                    if (distanceSqr > entity.EntitySetting.RobotAttackDistance * entity.EntitySetting.RobotAttackDistance) {
                        robotState = RobotState.Move;
                    } else {
                        robotState = RobotState.Attack;
                    }
                    break;
                case RobotState.Move:
                    //如果距离大于攻击距离 移动
                    Vector3 targetVec = new Vector3(playerEntity.Prefab.transform.position.x,
                        entity.Prefab.transform.position.y, playerEntity.Prefab.transform.position.z);
                    Vector3 dir = (targetVec - entity.Prefab.transform.position).normalized;
                    // entity.Prefab.transform.position = Vector3.MoveTowards(entity.Prefab.transform.position,
                    //     targetVec, Time.deltaTime * robotMovementSpeed);
                    robotCharacterController.Move(dir * Time.deltaTime * robotMovementSpeed);
                    robotAnimator.SetFloat("Motion", 1);
                    if (robotDirector.state == PlayState.Playing) {
                        robotDirector.Stop();
                    }
                    //朝向玩家
                    Vector3 targetForwardVec = (playerEntity.Prefab.transform.position - entity.Prefab.transform.position).normalized; 
                    Vector3 targetForwardVecRet = new Vector3(targetForwardVec.x, 0, targetForwardVec.z);
                    entity.Prefab.transform.forward = Vector3.MoveTowards(entity.Prefab.transform.forward, targetForwardVecRet,
                        Time.deltaTime * robotRotateSpeed);

                    if (distanceSqr < entity.EntitySetting.RobotAttackDistance * entity.EntitySetting.RobotAttackDistance) {
                        //如果距离小于攻击距离 攻击
                        robotAnimator.SetFloat("Motion", 0);
                        robotAttackTriggerComp.Get<Collider>("AttackTrigger").enabled = true;
                        robotDirector.playableAsset = robotAttackTimelineAsset;
                        robotDirector.Play();

                        //朝向玩家
                        entity.Prefab.transform.forward = targetForwardVec;
                        robotState = RobotState.Attack;
                    }
                    break;
                case RobotState.Attack:
                    if (robotDirector.state != PlayState.Playing) {
                        robotState = RobotState.Move;
                    }
                    break;
            }
        }

        public override void OnClear() {
            base.OnClear();
            robotState = RobotState.Idle;
            Data.Instance.OnUpdateEvent.RemoveListener(RobotEvent);
            robotAttackTriggerComp.OnTriggerEnterEvent.RemoveListener(OnRobotAttackTriggerEnter);
        }
    }
}