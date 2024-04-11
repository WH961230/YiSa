using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Event_LevelUpgrade : Behaviour {
        private Flow_Battle battleFlow;
        public Behaviour_Event_LevelUpgrade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg(MessageCode.LevelUpgrade, SelectOneOutOfThree);
            bool getFlow = Flo.Instance.GetFlow(out battleFlow);
            if (getFlow) {
                RefreshLevel();
            }
        }

		/*BUFF三选一*/
		private void SelectOneOutOfThree() {
            LevelUpgrade();
            SetCanControl(false);

            //弹出下一关机器人难度增加的选择 三选一
            Comp battleui = battleFlow.GetUI();
            Comp levelselect = Cond.Instance.Get<Comp>(battleui, Label.Assemble(Label.LEVEL, Label.SELECT));
            levelselect.gameObject.SetActive(true);
            bool isGetLevelRobotSetting = Loader.LoadSetting().TryGetRobotByCount(3, out List<RobotSettingInfo> robotSettings);
            if (isGetLevelRobotSetting) {
                Button A = Cond.Instance.Get<Button>(levelselect, Label.A);
                ButtonRegister.RemoveAllListener(A);
                ButtonRegister.AddListener(A, SelectRobotSetting, robotSettings[0]);
                Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.A).text = robotSettings[0].Description;
                Cond.Instance.Get<Image>(levelselect, Label.A).sprite = robotSettings[0].Icon;

                Button B = Cond.Instance.Get<Button>(levelselect, Label.B);
                ButtonRegister.RemoveAllListener(B);
                ButtonRegister.AddListener(B, SelectRobotSetting, robotSettings[1]);
                Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.B).text = robotSettings[1].Description;
                Cond.Instance.Get<Image>(levelselect, Label.B).sprite = robotSettings[1].Icon;

                Button C = Cond.Instance.Get<Button>(levelselect, Label.C);
                ButtonRegister.RemoveAllListener(C);
                ButtonRegister.AddListener(C, SelectRobotSetting, robotSettings[2]);
                Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.C).text = robotSettings[2].Description;
                Cond.Instance.Get<Image>(levelselect, Label.C).sprite = robotSettings[2].Icon;
            }
        }

        /*选择机器人配置*/
        private void SelectRobotSetting(RobotSettingInfo robotSettingInfo) {
            /*怪物等级*/
            Data.Instance.GlobalInfo.RobotLevel += robotSettingInfo.robotDifficulty;
            RefreshLevel();
            /*恢复控制*/
            SetCanControl(true);
            Comp levelselect = Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.Assemble(Label.LEVEL, Label.SELECT));
            levelselect.gameObject.SetActive(false);
            MessageRegister.Instance.Dis(MessageCode.LevelUpgradeIncreaseRobot, robotSettingInfo.Sign);
            MessageRegister.Instance.Dis(MessageCode.RobotCreate);
        }

        /*设置升级*/
        private void LevelUpgrade() {
            Data.Instance.GlobalInfo.Level++;
            RefreshLevel();
        }

        /*更新等级*/
        private void RefreshLevel() {
            Comp battleui = battleFlow.GetUI();
            Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
            TextMeshProUGUI robotLevel = Cond.Instance.Get<TextMeshProUGUI>(info, Label.Assemble(Label.ROBOT, Label.LEVEL));
            robotLevel.text = Data.Instance.GlobalInfo.RobotLevel.ToString("D2");
            TextMeshProUGUI ownLevel = Cond.Instance.Get<TextMeshProUGUI>(info, Label.Assemble(Label.OWN, Label.LEVEL));
            ownLevel.text = Data.Instance.GlobalInfo.OwnLevel.ToString("D2");
            TextMeshProUGUI level = Cond.Instance.Get<TextMeshProUGUI>(info, Label.LEVEL);
            level.text = Data.Instance.GlobalInfo.Level.ToString("D2");
        }

        /*设置是否可控*/
        private void SetCanControl(bool canControl) {
            Data.Instance.GlobalInfo.AllowMovement = canControl;
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg(MessageCode.LevelUpgrade, SelectOneOutOfThree);
        }
    }
}