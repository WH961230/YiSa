using UnityEngine;

namespace LazyPan {
    public class Flow_Battle : Flow {
		private Comp UI_Battle;

		private Entity Obj_Terrain_Terrain;
		private Entity Obj_Event_BeginTimeline;
		private Entity Obj_Camera_Camera;
		private Entity Obj_Player_Soldier;
		private Entity Obj_Tower_Tower;
		private Entity Obj_Event_PlayerUpgrade;
		private Entity Obj_Event_LevelUpgrade;
		private Entity Obj_Event_ActivableCreator;
		private Entity Obj_Event_RobotCreator;
		private Entity Obj_Event_Settlement;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            ConsoleEx.Instance.Content("log", "Flow_Battle  战斗流程");
			UI_Battle = UI.Instance.Open("UI_Battle");

			Obj_Terrain_Terrain = Obj.Instance.LoadEntity("Obj_Terrain_Terrain");
			Obj_Event_BeginTimeline = Obj.Instance.LoadEntity("Obj_Event_BeginTimeline");

        }

		/*获取UI*/
		public Comp GetUI() {
			return UI_Battle;
		}

		/*开始游戏*/
		public void Play() {
			Obj.Instance.UnLoadEntity(Obj_Event_BeginTimeline);
			Obj_Camera_Camera = Obj.Instance.LoadEntity("Obj_Camera_Camera");
			Obj_Player_Soldier = Obj.Instance.LoadEntity("Obj_Player_Soldier");
			Obj_Tower_Tower = Obj.Instance.LoadEntity("Obj_Tower_Tower");
			Obj_Event_PlayerUpgrade = Obj.Instance.LoadEntity("Obj_Event_PlayerUpgrade");
			Obj_Event_LevelUpgrade = Obj.Instance.LoadEntity("Obj_Event_LevelUpgrade");
			Obj_Event_ActivableCreator = Obj.Instance.LoadEntity("Obj_Event_ActivableCreator");
			Obj_Event_RobotCreator = Obj.Instance.LoadEntity("Obj_Event_RobotCreator");
		}

		/*结算*/
		public void Settlement() {
			Obj_Event_Settlement = Obj.Instance.LoadEntity("Obj_Event_Settlement");
		}


        /*下一步*/
        public void Next(string teleportSceneSign) {
            Clear();
            Launch.instance.StageLoad(teleportSceneSign);
        }

        public override void Clear() {
            base.Clear();
			Obj.Instance.UnLoadEntity(Obj_Event_Settlement);
			Obj.Instance.UnLoadEntity(Obj_Event_RobotCreator);
			Obj.Instance.UnLoadEntity(Obj_Event_ActivableCreator);
			Obj.Instance.UnLoadEntity(Obj_Event_LevelUpgrade);
			Obj.Instance.UnLoadEntity(Obj_Event_PlayerUpgrade);
			Obj.Instance.UnLoadEntity(Obj_Tower_Tower);
			Obj.Instance.UnLoadEntity(Obj_Player_Soldier);
			Obj.Instance.UnLoadEntity(Obj_Camera_Camera);
			Obj.Instance.UnLoadEntity(Obj_Terrain_Terrain);

			UI.Instance.Close("UI_Battle");

        }
    }
}