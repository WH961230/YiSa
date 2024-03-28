using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Event_LevelSelect : Behaviour {
        private Flow_Battle battleFlow;
        private Comp levelselect;
        public Behaviour_Event_LevelSelect(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Data.Instance.OnUpdateEvent.AddListener(LevelSelect);
        }

        private void LevelSelect() {
            if (Data.Instance.SelectLevel) {

                Data.Instance.SelectLevel = false;
            }
        }

        private void SelectRobotSetting(LevelRobotSetting robotSetting) {
            int num = robotSetting.robotNum;
            while (num > 0) {
                Data.Instance.SelectRobots.Add(robotSetting.robotSign);
                num--;
            }

            levelselect.gameObject.SetActive(false);
            Data.Instance.StartNextLevel = true;
            Data.Instance.CanControl = true;
        }

        public override void Clear() {
            base.Clear();
            Data.Instance.OnUpdateEvent.RemoveListener(LevelSelect);
        }
    }
}