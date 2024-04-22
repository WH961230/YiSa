using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyPan {
    public class Behaviour_Event_RobotCreator : Behaviour {
        private Clock clock;
        private List<string> robotSigns;//当前要生成的怪物
        private List<Entity> robotSoldierEntities;//此轮已生成的怪物
        private Queue<string> robotSoldierQueuies;//生成队列

        public Behaviour_Event_RobotCreator(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            robotSoldierEntities = new List<Entity>();
            robotSoldierQueuies = new Queue<string>();
            robotSigns = new List<string>();

            MessageRegister.Instance.Reg(MessageCode.RobotCreate, RobotCreate);
            MessageRegister.Instance.Reg<string, int>(MessageCode.LevelUpgradeIncreaseRobot, AddRobot);
            MessageRegister.Instance.Reg<Entity, int>(MessageCode.BeInjuried, RobotBeInjured);
            MessageRegister.Instance.Reg<Entity>(MessageCode.BeSelfDetonation, RobotBeSelfDetonation);

            /*第一波 普通机器人*/
            MessageRegister.Instance.Dis(MessageCode.LevelUpgradeIncreaseRobot, "Obj_Robot_Soldier", 5);
            MessageRegister.Instance.Dis(MessageCode.RobotCreate);
        }

        /*记录生成的敌人标识*/
        private void AddRobot(string sign, int num) {
            while (num > 0) {
                robotSigns.Add(sign);
                num--;
            }
        }

        /*生成怪物*/
        private void RobotCreate() {
            PrepareRobot();
            RobotEvent(2, 1.5f);
        }

        /*机器人事件 几秒钟后 间隔几秒 生成怪物 一共需要生成几只*/
        private void RobotEvent(float delay, float interval) {
            clock = ClockUtil.Instance.AlarmRepeat(delay, interval, () => {
                if (robotSoldierQueuies.Count > 0) {
                    InstanceRobot();
                } else {
                    ClockUtil.Instance.Stop(clock);
                }
            });
        }

        /*怪物队列预备*/
        private void PrepareRobot() {
            robotSoldierQueuies.Clear();
            foreach (string robotSign in robotSigns) {
                robotSoldierQueuies.Enqueue(robotSign);
            }
        }

        /*怪物生成*/
        private void InstanceRobot() {
            if (robotSoldierQueuies.Count > 0) {
                string robotSign = robotSoldierQueuies.Dequeue();
                Entity robot = Obj.Instance.LoadEntity(robotSign);
                robotSoldierEntities.Add(robot);
            }
        }

        /*机器人自爆*/
        private void RobotBeSelfDetonation(Entity robot) {
            if (robot.EntityData.BaseRuntimeData.Type == "Robot") {
                robot.EntityData.BaseRuntimeData.RobotInfo.HealthPoint = 0;
                /*自杀*/
                robot.EntityData.BaseRuntimeData.RobotInfo.BeAttackType = 2;
                /*死亡*/
                RemoveRobot(robot);
            }
        }

        /*机器人受伤*/
        private void RobotBeInjured(Entity robot, int damage) {
            if (robot.EntityData.BaseRuntimeData.Type == "Robot") {
                if (robot.EntityData.BaseRuntimeData.RobotInfo != null && !robot.EntityData.BaseRuntimeData.RobotInfo.IsDead) {
                    /*受伤*/
                    robot.EntityData.BaseRuntimeData.RobotInfo.HealthPoint -= damage;
                    /*血量小于零掉落*/
                    if (robot.EntityData.BaseRuntimeData.RobotInfo.HealthPoint <= 0) {
                        /*敌方攻击*/
                        if (robot.EntityData.BaseRuntimeData.RobotInfo.BeAttackType == 1) {
                            /*掉落*/
                            robot.EntityData.BaseRuntimeData.RobotInfo.DeathDropType = UnityEngine.Random.Range(0, 100);
                            MessageRegister.Instance.Dis(MessageCode.DeathDrop, robot);
                        }

                        /*死亡*/
                        RemoveRobot(robot);
                    }
                }
            }
        }

        /*移除怪物*/
        private void RemoveRobot(Entity robotEntity) {
            if (robotSoldierEntities.Contains(robotEntity)) {
                robotSoldierEntities.Remove(robotEntity);
                Obj.Instance.UnLoadEntity(robotEntity);
                if (robotSoldierEntities.Count == 0) {
                    ConsoleEx.Instance.Content("log", $"怪物清空!");
                    MessageRegister.Instance.Dis(MessageCode.LevelUpgrade);
                }
            }
        }

        /*移除所有机器人*/
        private void RemoveAllRobot() {
            ConsoleEx.Instance.Content("log", $"所有怪物清空!");
            foreach (Entity robotEntity in robotSoldierEntities) {
                Obj.Instance.UnLoadEntity(robotEntity);
            }
            robotSoldierEntities.Clear();
        }

        public override void Clear() {
            base.Clear();
            RemoveAllRobot();
            MessageRegister.Instance.UnReg<Entity, int>(MessageCode.BeInjuried, RobotBeInjured);
            MessageRegister.Instance.UnReg<Entity>(MessageCode.BeSelfDetonation, RobotBeSelfDetonation);
            MessageRegister.Instance.UnReg<string, int>(MessageCode.LevelUpgradeIncreaseRobot, AddRobot);
            MessageRegister.Instance.UnReg(MessageCode.RobotCreate, RobotCreate);
            ClockUtil.Instance.Stop(clock);
        }
    }
}