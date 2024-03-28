using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Event_LevelUpgrade : Behaviour {
        private Flow_Battle battleFlow;
        public Behaviour_Event_LevelUpgrade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg(MessageCode.ClearRobot, SelectOneOutOfThree);
        }

		/*BUFF三选一*/
		private void SelectOneOutOfThree() {
            LevelUpgrade();
            SetCanControl(false);

            //弹出下一关机器人难度增加的选择 三选一
            bool isGetFlow = Flo.Instance.GetFlow(out battleFlow);
            if (isGetFlow) {
                Comp battleui = battleFlow.GetUI();
                Comp levelselect = Cond.Instance.Get<Comp>(battleui, Label.Assemble(Label.LEVEL, Label.SELECT));
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
        }

        private void SelectRobotSetting(LevelRobotSetting robotSetting) {
            int num = robotSetting.robotNum;
            while (num > 0) {
                Data.Instance.SelectRobots.Add(robotSetting.robotSign);
                num--;
            }

            Comp levelselect = Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.Assemble(Label.LEVEL, Label.SELECT));
            levelselect.gameObject.SetActive(false);
            SetCanControl(true);
            MessageRegister.Instance.Dis(MessageCode.NextLevel);
        }

        /*设置升级*/
        private void LevelUpgrade() {
            Data.Instance.LevelNum++;
            Data.Instance.SelectLevel = true;
        }

        /*设置是否可控*/
        private void SetCanControl(bool canControl) {
            Data.Instance.CanControl = canControl;
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg(MessageCode.ClearRobot, SelectOneOutOfThree);
        }
    }
}