using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Event_LevelUpgrade : Behaviour {
        private Flow_Battle battleFlow;
        public Behaviour_Event_LevelUpgrade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg(MessageCode.LevelUpgrade, Select);
            Flo.Instance.GetFlow(out battleFlow);

            DisplayLevel(Data.Instance.GlobalInfo.Level);
            CloseLevel(true);
        }

		/*BUFF三选一*/
		private void Select() {
            LevelUpgrade();
            Time.timeScale = 0;

            //弹出下一关机器人难度增加的选择 三选一
            Comp battleui = battleFlow.GetUI();
            Comp select = Cond.Instance.Get<Comp>(battleui, Label.SELECT);
            select.gameObject.SetActive(true);
            bool isGetLevelRobotSetting = Loader.LoadSetting().TryGetRobotByCount(3, out List<RobotSettingInfo> robotSettings);
            if (isGetLevelRobotSetting) {
                Comp selectA = Cond.Instance.Get<Comp>(select, Label.Assemble(Label.SELECT, Label.A));
                Button A = Cond.Instance.Get<Button>(selectA, Label.BUTTON);
                ButtonRegister.RemoveAllListener(A);
                ButtonRegister.AddListener(A, SelectRobotSetting, robotSettings[0]);
                Cond.Instance.Get<TextMeshProUGUI>(selectA, Label.INFO).text = robotSettings[0].Description;
                Cond.Instance.Get<Image>(selectA, Label.ICON).sprite = robotSettings[0].Icon;

                Comp selectB = Cond.Instance.Get<Comp>(select, Label.Assemble(Label.SELECT, Label.B));
                Button B = Cond.Instance.Get<Button>(selectB, Label.BUTTON);
                ButtonRegister.RemoveAllListener(B);
                ButtonRegister.AddListener(B, SelectRobotSetting, robotSettings[1]);
                Cond.Instance.Get<TextMeshProUGUI>(selectB, Label.INFO).text = robotSettings[1].Description;
                Cond.Instance.Get<Image>(selectB, Label.ICON).sprite = robotSettings[1].Icon;

                Comp selectC = Cond.Instance.Get<Comp>(select, Label.Assemble(Label.SELECT, Label.C));
                Button C = Cond.Instance.Get<Button>(selectC, Label.BUTTON);
                ButtonRegister.RemoveAllListener(C);
                ButtonRegister.AddListener(C, SelectRobotSetting, robotSettings[2]);
                Cond.Instance.Get<TextMeshProUGUI>(selectC, Label.INFO).text = robotSettings[2].Description;
                Cond.Instance.Get<Image>(selectC, Label.ICON).sprite = robotSettings[2].Icon;
            }
        }

        /*选择机器人配置*/
        private void SelectRobotSetting(RobotSettingInfo robotSettingInfo) {
            /*怪物等级*/
            Data.Instance.GlobalInfo.RobotLevel += robotSettingInfo.RobotDifficulty;
            RefreshLevel();
            Comp select = Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.SELECT);
            select.gameObject.SetActive(false);
            Time.timeScale = 1;
            MessageRegister.Instance.Dis(MessageCode.LevelUpgradeIncreaseRobot, robotSettingInfo.Sign, robotSettingInfo.Num);
            MessageRegister.Instance.Dis(MessageCode.RobotCreate);
            CloseLevel(false);
        }

        /*设置升级*/
        private void LevelUpgrade() {
            Data.Instance.GlobalInfo.Level++;
            RefreshLevel();
            DisplayLevel(Data.Instance.GlobalInfo.Level);
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

        /*显示关卡数量*/
        private void DisplayLevel(int levelNum) {
            /*展示n秒的关卡数*/
            Comp ui = battleFlow.GetUI();
            Comp levelcomp = Cond.Instance.Get<Comp>(ui, Label.LEVEL);

            /*由低向高遍历*/
            List<Comp.CompData> datas = levelcomp.Comps;
            for (int i = 0; i < datas.Count; i++) {
                Comp.CompData tmpData = datas[i];
                if (levelNum == int.Parse(tmpData.Sign)) {
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.TOP).SetActive(true);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.PAST).SetActive(false);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.NOW).SetActive(true);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.FUTURE).SetActive(false);
                } else if (levelNum > int.Parse(tmpData.Sign)) {
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.TOP).SetActive(false);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.PAST).SetActive(true);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.NOW).SetActive(false);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.FUTURE).SetActive(false);
                } else {
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.TOP).SetActive(false);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.PAST).SetActive(false);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.NOW).SetActive(false);
                    Cond.Instance.Get<GameObject>(tmpData.Comp, Label.FUTURE).SetActive(true);
                }
            }

            levelcomp.gameObject.SetActive(true);
        }

        /*关闭关卡等级展示*/
        private void CloseLevel(bool delayClose) {
            /*展示n秒的关卡数*/
            Comp ui = battleFlow.GetUI();
            Comp levelcomp = Cond.Instance.Get<Comp>(ui, Label.LEVEL);

            if (delayClose) {
                ClockUtil.Instance.AlarmAfter(3, () => {
                    levelcomp.gameObject.SetActive(false);
                });
            } else {
                levelcomp.gameObject.SetActive(false);
            }
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg(MessageCode.LevelUpgrade, Select);
        }
    }
}