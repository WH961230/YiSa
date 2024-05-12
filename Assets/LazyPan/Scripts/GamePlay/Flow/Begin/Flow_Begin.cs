using UnityEngine;

namespace LazyPan {
    public class Flow_Begin : Flow {
		private Comp UI_Begin;

		private Entity Obj_Camera_Camera;
		private Entity Obj_Terrain_Terrain;
		private Entity Obj_Trigger_StartGame;
		private Entity Obj_Player_Soldier;
		private Entity Obj_Music_BackgroundMusic;

        public override void Init(Flow baseFlow) {
            base.Init(baseFlow);
            ConsoleEx.Instance.Content("log", "Flow_Begin  开始流程");
			UI_Begin = UI.Instance.Open("UI_Begin");

			Obj_Camera_Camera = Obj.Instance.LoadEntity("Obj_Camera_Camera");
			Obj_Terrain_Terrain = Obj.Instance.LoadEntity("Obj_Terrain_Terrain");
			Obj_Trigger_StartGame = Obj.Instance.LoadEntity("Obj_Trigger_StartGame");
			Obj_Player_Soldier = Obj.Instance.LoadEntity("Obj_Player_Soldier");
			Obj_Music_BackgroundMusic = Obj.Instance.LoadEntity("Obj_Music_BackgroundMusic");

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
			Obj.Instance.UnLoadEntity(Obj_Trigger_StartGame);
			Obj.Instance.UnLoadEntity(Obj_Terrain_Terrain);
			Obj.Instance.UnLoadEntity(Obj_Camera_Camera);
			Obj.Instance.UnLoadEntity(Obj_Music_BackgroundMusic);

			UI.Instance.Close("UI_Begin");

        }
    }
}