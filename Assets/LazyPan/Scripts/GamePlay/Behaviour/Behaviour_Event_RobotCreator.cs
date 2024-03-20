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
            Data.Instance.SelectRobots = new List<string>();
            Data.Instance.SelectRobots.Add("Obj_Robot_Soldier");
            Data.Instance.SelectRobots.Add("Obj_Robot_Soldier");
            PrepareRobot();
            RobotEvent(2, 1);
            MessageRegister.Instance.Reg<Entity>(MessageCode.DeadRecycle, RobotDeadRecycle);
            MessageRegister.Instance.Reg(MessageCode.GameOver, RemoveAllRobot);
            Data.Instance.OnUpdateEvent.AddListener(Wait);
        }

        private void Wait() {
            if (Data.Instance.StartNextLevel) {
                //准备机器人
                PrepareRobot();
                RobotEvent(2, 1);
                Data.Instance.StartNextLevel = false;
            }
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
            foreach (string robotSign in Data.Instance.SelectRobots) {
                robotSoldierQueuies.Enqueue(robotSign);
            }
        }

        /*怪物生成*/
        private void InstanceRobot() {
            if (robotSoldierQueuies.Count > 0) {
                string robotSign = robotSoldierQueuies.Dequeue();
                robotSoldierEntities.Add(Obj.Instance.LoadEntity(robotSign));
            }
        }

        /*机器人死亡*/
        private void RobotDeadRecycle(Entity entity) {
            if (entity.EntityData.BaseRuntimeData.Type == "Robot") {
                RemoveRobot(entity);
            }
        }

        /*移除怪物*/
        private void RemoveRobot(Entity robotEntity) {
            if (robotSoldierEntities.Contains(robotEntity)) {
                robotSoldierEntities.Remove(robotEntity);
                Obj.Instance.UnLoadEntity(robotEntity);
                if (robotSoldierEntities.Count == 0) {
#if UNITY_EDITOR
                    ConsoleEx.Instance.Content("log", $"怪物清空!");
#endif
                    Data.Instance.LevelNum++;
                    Data.Instance.SelectLevel = true;
                    Data.Instance.CanControl = false;
                }
            }
        }

        /*移除所有机器人*/
        private void RemoveAllRobot() {
#if UNITY_EDITOR
            ConsoleEx.Instance.Content("log", $"所有怪物清空!");
#endif
            foreach (Entity robotEntity in robotSoldierEntities) {
                Obj.Instance.UnLoadEntity(robotEntity);
            }
            robotSoldierEntities.Clear();
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.DeadRecycle, RobotDeadRecycle);
            MessageRegister.Instance.UnReg(MessageCode.GameOver, RemoveAllRobot);
            ClockUtil.Instance.Stop(clock);
            Data.Instance.OnUpdateEvent.RemoveListener(Wait);
        }
    }
}