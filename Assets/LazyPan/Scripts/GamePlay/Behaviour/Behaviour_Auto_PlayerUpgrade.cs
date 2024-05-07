using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_PlayerUpgrade : Behaviour {
        private Flow_Battle battleFlow;
        public Behaviour_Auto_PlayerUpgrade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg(MessageCode.PlayerUpgrade, Select);
            InitBuff();
            /*激活信息UI*/
            Flo.Instance.GetFlow(out battleFlow);
            Comp info = Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.INFO);
            info.gameObject.SetActive(true);
        }

        /*初始化BUFF*/
        private void InitBuff() {
            Data.Instance.BuffInfo.Clear();
            /*初始化所有BUFF*/
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

            /*三选一UI*/
            Comp battleui = battleFlow.GetUI();
            Comp select = Cond.Instance.Get<Comp>(battleui, Label.SELECT);
            select.gameObject.SetActive(true);

            /*获取可用Buff*/
            List<BuffSettingInfo> parentInfo = new List<BuffSettingInfo>();
            for (int i = 0; i < Data.Instance.BuffInfo.Count; i++) {
                BuffInfo info = Data.Instance.BuffInfo[i];
                /*Buff未被禁用*/
                if (!info.Disable) {
                    parentInfo.Add(info.Setting);
                }
            }

            /*展示Buff*/
            bool isGetLevelBuffSetting = Loader.LoadSetting()
                .TryGetBuffByCount(3, parentInfo, out List<BuffSettingInfo> buffSettings);
            if (isGetLevelBuffSetting) {
                foreach (BuffSettingInfo tmpInfo in buffSettings) {
                    /*选项一*/
                    Comp canselect = Cond.Instance.Get<Comp>(select, Label.Assemble(Label.SELECT, Label.A));
                    Button button = Cond.Instance.Get<Button>(canselect, Label.BUTTON);
                    ButtonRegister.RemoveAllListener(button);
                    ButtonRegister.AddListener(button, SelectBuffSetting, buffSettings[0]);
                    bool get = TryGetBuffInfo(tmpInfo.Sign, out BuffInfo info);
                    if (get) {
                        if (tmpInfo.CanUpgrade) {
                            get = Loader.LoadSetting().BuffSetting.GetDescriptionByLevel(tmpInfo.Sign, info.Level, out string description);
                            if (get) {
                                Cond.Instance.Get<TextMeshProUGUI>(canselect, Label.INFO).text = description;
                            }
                        } else {
                            Cond.Instance.Get<TextMeshProUGUI>(canselect, Label.INFO).text = tmpInfo.Description;
                        }
                    }
                    Cond.Instance.Get<Image>(canselect, Label.ICON).sprite = tmpInfo.Icon;
                    SetSubscript(canselect, tmpInfo);
                }
            } else {
#if UNITY_EDITOR
                EditorApplication.isPaused = true;
#endif
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
            /*更新左上角信息*/
            RefreshLevelInfo();
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

            RefreshDisableBuff(buffSettingInfo);
        }

        /*获取Buff信息*/
        private bool TryGetBuffInfo(string sign, out BuffInfo info) {
            /*当前项可以升级 则升级*/
            for (int i = 0; i < Data.Instance.BuffInfo.Count; i++) {
                BuffInfo tmp = Data.Instance.BuffInfo[i];
                if (tmp.Setting.Sign == sign) {
                    info = tmp;
                    return true;
                }
            }

            info = default;
            return false;
        }

        /*刷新Buff是否禁用*/
        private void RefreshDisableBuff(BuffSettingInfo buffSettingInfo) {
            /*当前项可以升级 则升级*/
            for (int i = 0; i < Data.Instance.BuffInfo.Count; i++) {
                BuffInfo tmp = Data.Instance.BuffInfo[i];
                if (tmp.Setting == buffSettingInfo && tmp.Level > 0) {
                    tmp.Level++;
                    if (tmp.Level > tmp.Setting.UpgradeLimit) {
                        tmp.Disable = true;
                    }
                    break;
                }
            }
        }

        /*更新等级*/
        private void RefreshLevelInfo() {
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