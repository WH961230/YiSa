using UnityEngine;
using UnityEngine.Playables;

namespace LazyPan {
    public class Flow_Begin : Flow {
		private Comp UI_Begin;

		private Entity Obj_Volume_Volume;
		private Entity Obj_Light_DirectionalLight;
		private Entity Obj_Camera_Camera;
		private Entity Obj_Terrain_Terrain;
		private Entity Obj_Event_StartGame;
		private Entity Obj_Player_Soldier;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            ConsoleEx.Instance.Content("log", "Flow_Begin  开始流程");
			UI_Begin = UI.Instance.Open("UI_Begin");

			Obj_Volume_Volume = Obj.Instance.LoadEntity("Obj_Volume_Volume");
			Obj_Light_DirectionalLight = Obj.Instance.LoadEntity("Obj_Light_DirectionalLight");
			Obj_Camera_Camera = Obj.Instance.LoadEntity("Obj_Camera_Camera");
			Obj_Terrain_Terrain = Obj.Instance.LoadEntity("Obj_Terrain_Terrain");
			Obj_Event_StartGame = Obj.Instance.LoadEntity("Obj_Event_StartGame");
			Obj_Player_Soldier = Obj.Instance.LoadEntity("Obj_Player_Soldier");
        }

		/*获取UI*/
		public Comp GetUI() {
			return UI_Begin;
		}


        /*下一步*/
        public void Next(string teleportSceneSign) {
            Clear();
            Launch.instance.StageLoad(teleportSceneSign);
        }

        public override void Clear() {
            base.Clear();
			Obj.Instance.UnLoadEntity(Obj_Player_Soldier);
			Obj.Instance.UnLoadEntity(Obj_Event_StartGame);
			Obj.Instance.UnLoadEntity(Obj_Terrain_Terrain);
			Obj.Instance.UnLoadEntity(Obj_Camera_Camera);
			Obj.Instance.UnLoadEntity(Obj_Light_DirectionalLight);
			Obj.Instance.UnLoadEntity(Obj_Volume_Volume);

			UI.Instance.Close("UI_Begin");

        }
    }
}