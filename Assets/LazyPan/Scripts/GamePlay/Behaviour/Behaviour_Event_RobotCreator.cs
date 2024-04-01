using System.Collections.Generic;

namespace LazyPan {
    public class Behaviour_Event_RobotCreator : Behaviour {
        private Clock clock;
        private List<Entity> robotSoldierEntities;
        private Queue<string> robotSoldierQueuies;

        public Behaviour_Event_RobotCreator(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            robotSoldierEntities = new List<Entity>();
            robotSoldierQueuies = new Queue<string>();
            /*第一波 普通机器人*/
            PrepareRobot("Obj_Robot_Soldier");
            PrepareRobot("Obj_Robot_Soldier");
            RobotCreate();
            MessageRegister.Instance.Reg(MessageCode.RobotCreate, RobotCreate);
            MessageRegister.Instance.Reg<string>(MessageCode.LevelUpgradeIncreaseRobot, PrepareRobot);
            MessageRegister.Instance.Reg<Entity, int>(MessageCode.BeInjuried, RobotBeInjured);
            Data.Instance.OnUpdateEvent.AddListener(Wait);
        }

        private void Wait() {
            
        }

        /*生成怪物*/
        private void RobotCreate() {
            RobotEvent(2, 1);
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
        private void PrepareRobot(string sign) {
            robotSoldierQueuies.Enqueue(sign);
        }

        /*怪物生成*/
        private void InstanceRobot() {
            if (robotSoldierQueuies.Count > 0) {
                string robotSign = robotSoldierQueuies.Dequeue();
                Entity robot = Obj.Instance.LoadEntity(robotSign);
                bool getSetting = Loader.LoadSetting().TryGetRobotBySign(robotSign, out RobotSettingInfo info);
                if (getSetting) {
                    robot.EntityData.BaseRuntimeData.RobotInfo.HealthPoint = info.HealthPoint;
                    robotSoldierEntities.Add(robot);
                }
            }
        }

        /*机器人受伤*/
        private void RobotBeInjured(Entity robot, int damage) {
            /*受伤*/
            robot.EntityData.BaseRuntimeData.RobotInfo.HealthPoint -= damage;
            /*血量小于零掉落*/
            if (robot.EntityData.BaseRuntimeData.RobotInfo.HealthPoint <= 0) {
                /*敌方攻击*/
                if (robot.EntityData.BaseRuntimeData.RobotInfo.DeathType == 0) {
                    /*掉落*/
                    entity.EntityData.BaseRuntimeData.RobotInfo.DeathDropType = UnityEngine.Random.Range(0, 3);
                    MessageRegister.Instance.Dis(MessageCode.DeathDrop, robot);
                }

                /*死亡*/
                RemoveRobot(robot);
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
            MessageRegister.Instance.UnReg<string>(MessageCode.LevelUpgradeIncreaseRobot, PrepareRobot);
            MessageRegister.Instance.UnReg(MessageCode.RobotCreate, RobotCreate);
            ClockUtil.Instance.Stop(clock);
            Data.Instance.OnUpdateEvent.RemoveListener(Wait);
        }
    }
}