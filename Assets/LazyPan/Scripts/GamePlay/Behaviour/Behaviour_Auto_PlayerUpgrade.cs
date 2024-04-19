﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_PlayerUpgrade : Behaviour {
        private Flow_Battle battleFlow;
        public Behaviour_Auto_PlayerUpgrade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg(MessageCode.PlayerUpgrade, Select);
            Data.Instance.BuffInfo.Clear();
            for (int i = 0; i < Loader.LoadSetting().BuffSetting.BuffSettingInfo.Count; i++) {
                BuffSettingInfo tmp = Loader.LoadSetting().BuffSetting.BuffSettingInfo[i];
                Data.Instance.BuffInfo.Add(new BuffInfo() {
                    Setting = tmp,
                    Level = tmp.CanUpgrade ? 1 : 0,
                    Disable = false,
                });
            }
        }

		/*升级*/
		private void Select() {
            /*移动禁止*/
            Time.timeScale = 0;

            /*弹出 Buff 难度增加的选择 三选一*/
            bool isGetFlow = Flo.Instance.GetFlow(out battleFlow);
            if (isGetFlow) {
                Comp battleui = battleFlow.GetUI();
                Comp select = Cond.Instance.Get<Comp>(battleui, Label.SELECT);
                select.gameObject.SetActive(true);
                /*找到所有可以选择的Buff*/
                List<BuffSettingInfo> parentInfo = new List<BuffSettingInfo>();
                for (int i = 0; i < Data.Instance.BuffInfo.Count; i++) {
                    BuffInfo info = Data.Instance.BuffInfo[i];
                    /*Buff未被禁用*/
                    if (!info.Disable) {
                        parentInfo.Add(info.Setting);
                    }
                }
                /*展示Buff*/
                bool isGetLevelBuffSetting = Loader.LoadSetting().TryGetBuffByCount(3, parentInfo, out List<BuffSettingInfo> buffSettings);
                if (isGetLevelBuffSetting) {
                    Comp selectA = Cond.Instance.Get<Comp>(select, Label.Assemble(Label.SELECT, Label.A));
                    Button A = Cond.Instance.Get<Button>(selectA, Label.BUTTON);
                    ButtonRegister.RemoveAllListener(A);
                    ButtonRegister.AddListener(A, SelectBuffSetting, buffSettings[0]);
                    Cond.Instance.Get<TextMeshProUGUI>(selectA, Label.INFO).text = buffSettings[0].Description;
                    Cond.Instance.Get<Image>(selectA, Label.ICON).sprite = buffSettings[0].Icon;
                    SetSubscript(selectA, buffSettings[0]);

                    Comp selectB = Cond.Instance.Get<Comp>(select, Label.Assemble(Label.SELECT, Label.B));
                    Button B = Cond.Instance.Get<Button>(selectB, Label.BUTTON);
                    ButtonRegister.RemoveAllListener(B);
                    ButtonRegister.AddListener(B, SelectBuffSetting, buffSettings[1]);
                    Cond.Instance.Get<TextMeshProUGUI>(selectB, Label.INFO).text = buffSettings[1].Description;
                    Cond.Instance.Get<Image>(selectB, Label.ICON).sprite = buffSettings[1].Icon;
                    SetSubscript(selectB, buffSettings[1]);

                    Comp selectC = Cond.Instance.Get<Comp>(select, Label.Assemble(Label.SELECT, Label.C));
                    Button C = Cond.Instance.Get<Button>(selectC, Label.BUTTON);
                    ButtonRegister.RemoveAllListener(C);
                    ButtonRegister.AddListener(C, SelectBuffSetting, buffSettings[2]);
                    Cond.Instance.Get<TextMeshProUGUI>(selectC, Label.INFO).text = buffSettings[2].Description;
                    Cond.Instance.Get<Image>(selectC, Label.ICON).sprite = buffSettings[2].Icon;
                    SetSubscript(selectC, buffSettings[2]);
                }
            }
        }

        /*设置下标 无法升级的就提供图标 可以升级的就提供等级*/
        private void SetSubscript(Comp select, BuffSettingInfo setting) {
            /*获取配置的公共数据*/
            BuffInfo buffinfo = null;
            for (int i = 0; i < Data.Instance.BuffInfo.Count; i++) {
                BuffInfo info = Data.Instance.BuffInfo[i];
                if (info.Setting == setting) {
                    buffinfo = info;
                    break;
                }
            }

            if (buffinfo != null) {
                Sprite empty = Cond.Instance.Get<Sprite>(Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.SELECT), Label.EMPTY);
                Sprite full = Cond.Instance.Get<Sprite>(Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.SELECT), Label.FULL);
                Image diffA = Cond.Instance.Get<Image>(select, Label.A);
                Image diffB = Cond.Instance.Get<Image>(select, Label.B);
                Image diffC = Cond.Instance.Get<Image>(select, Label.C);
                Image subIcon = Cond.Instance.Get<Image>(select, Label.Assemble(Label.SUBSCRIPT, Label.ICON));
                if (buffinfo.Level > 0) {/*可升级 显示等级*/
                    diffA.gameObject.SetActive(true);
                    diffB.gameObject.SetActive(true);
                    diffB.sprite = buffinfo.Level > 1 ? full : empty;
                    diffC.gameObject.SetActive(true);
                    diffC.sprite = buffinfo.Level > 2 ? full : empty;
                    subIcon.gameObject.SetActive(false);
                } else {/*不可升级 显示图标*/
                    diffA.gameObject.SetActive(false);
                    diffB.gameObject.SetActive(false);
                    diffC.gameObject.SetActive(false);
                    subIcon.gameObject.SetActive(true);
                    subIcon.sprite = buffinfo.Setting.SubscriptIcon;
                }
            }
        }

        /*选择BUFF配置*/
        private void SelectBuffSetting(BuffSettingInfo buffSettingInfo) {
            /*己方等级*/
            Data.Instance.GlobalInfo.OwnLevel++;
            RefreshLevel();
            /*经验值归零*/
            entity.EntityData.BaseRuntimeData.PlayerInfo.Experience = 0;
            Comp battleui = battleFlow.GetUI();
            Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
            Cond.Instance.Get<Slider>(info, Label.EXP).value =
                entity.EntityData.BaseRuntimeData.PlayerInfo.Experience /
                Loader.LoadSetting().PlayerSetting.MaxExperience;

            Time.timeScale = 1;
            Comp select = Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.SELECT);
            select.gameObject.SetActive(false);
            Cond.Instance.GetTowerEntity(out Entity towerEntity);

            if (BehaviourRegister.Instance.TryGetRegisterBehaviour(towerEntity.ID, buffSettingInfo.BehaviourSign, out Behaviour behaviour)) {
                if (buffSettingInfo.CanUpgrade) {
                    behaviour.Upgrade();
                }
            } else {
                BehaviourRegister.Instance.RegisterBehaviour(towerEntity.ID, buffSettingInfo.BehaviourSign);
            }

            /*当前项可以升级 则升级*/
            for (int i = 0; i < Data.Instance.BuffInfo.Count; i++) {
                BuffInfo tmp = Data.Instance.BuffInfo[i];
                if (tmp.Setting == buffSettingInfo && tmp.Level > 0) {
                    tmp.Level++;
                    if (tmp.Level > 3) {
                        tmp.Disable = true;
                    }
                    break;
                }
            }
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

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg(MessageCode.PlayerUpgrade, Select);
        }
    }
}