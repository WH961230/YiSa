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
                //弹出下一关机器人难度增加的选择 三选一
                bool isGetFlow = Flo.Instance.GetFlow(out battleFlow);
                if (isGetFlow) {
                    Comp battleui = battleFlow.GetUI();
                    levelselect = Cond.Instance.Get<Comp>(battleui, Label.Assemble(Label.LEVEL, Label.SELECT));
                    levelselect.gameObject.SetActive(true);
                    bool isGetLevelRobotSetting = Loader.LoadSetting().TryGetLevelRobotSetting(Data.Instance.LevelNum, 3,
                        out List<LevelRobotSetting> robotSettings);
                    if (isGetLevelRobotSetting) {
                        Button A = Cond.Instance.Get<Button>(levelselect, Label.A);
                        ButtonRegister.AddListener(A, SelectRobotSetting, robotSettings[0]);
                        Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.A).text = robotSettings[0].Description;
                        Cond.Instance.Get<Image>(levelselect, Label.A).sprite = robotSettings[0].Icon;

                        Button B = Cond.Instance.Get<Button>(levelselect, Label.B);
                        ButtonRegister.AddListener(B, SelectRobotSetting, robotSettings[1]);
                        Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.B).text = robotSettings[1].Description;
                        Cond.Instance.Get<Image>(levelselect, Label.B).sprite = robotSettings[1].Icon;

                        Button C = Cond.Instance.Get<Button>(levelselect, Label.C);
                        ButtonRegister.AddListener(C, SelectRobotSetting, robotSettings[2]);
                        Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.C).text = robotSettings[2].Description;
                        Cond.Instance.Get<Image>(levelselect, Label.C).sprite = robotSettings[2].Icon;
                    }
                }
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