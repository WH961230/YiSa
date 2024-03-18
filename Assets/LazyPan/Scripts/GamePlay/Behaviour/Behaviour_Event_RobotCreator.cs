using System.Collections.Generic;

namespace LazyPan {
    public class Behaviour_Event_RobotCreator : Behaviour {
        private Flow_Battle flow;
        private Clock clock;
        private List<Entity> robotSoldierEntities;
        private Queue<string> robotSoldierQueuies;

        public Behaviour_Event_RobotCreator(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            robotSoldierEntities = new List<Entity>();
            robotSoldierQueuies = new Queue<string>();
            Flo.Instance.GetFlow(out flow);
            /*第一波 普通机器人*/
            PrepareRobot("Obj_Robot_Soldier");
            PrepareRobot("Obj_Robot_Soldier");
            RobotEvent(2, 1);
            MessageRegister.Instance.Reg<Entity>(MessageCode.Dead, RobotDead);
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
        private void PrepareRobot(string robotSign) {
            robotSoldierQueuies.Enqueue(robotSign);
        }

        /*怪物生成*/
        private void InstanceRobot() {
            if (robotSoldierQueuies.Count > 0) {
                string robotSign = robotSoldierQueuies.Dequeue();
                robotSoldierEntities.Add(Obj.Instance.LoadEntity(robotSign));
            }
        }

        /*机器人死亡*/
        private void RobotDead(Entity entity) {
            if (entity.EntityData.BaseRuntimeData.Type == "Robot") {
                RemoveRobot(entity);
            }
        }

        /*移除怪物*/
        private void RemoveRobot(Entity robotEntity) {
            if (robotSoldierEntities.Contains(robotEntity)) {
                robotSoldierEntities.Remove(robotEntity);
                Obj.Instance.UnLoadEntity(robotEntity);
            }
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.Dead, RobotDead);
            ClockUtil.Instance.Stop(clock);
        }
    }
}