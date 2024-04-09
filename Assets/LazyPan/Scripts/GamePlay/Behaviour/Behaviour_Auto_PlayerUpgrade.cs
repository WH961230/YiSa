﻿using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_Auto_PlayerUpgrade : Behaviour {
        private Flow_Battle battleFlow;
        public Behaviour_Auto_PlayerUpgrade(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg(MessageCode.PlayerUpgrade, LevelUp);
        }

		/*升级*/
		private void LevelUp() {
            /*经验值归零*/
            entity.EntityData.BaseRuntimeData.PlayerInfo.Experience = 0;
            /*移动禁止*/
            SetCanControl(false);
            /*触发Buff三选一*/
            //弹出 Buff 难度增加的选择 三选一
            bool isGetFlow = Flo.Instance.GetFlow(out battleFlow);
            if (isGetFlow) {
                Comp battleui = battleFlow.GetUI();
                Comp levelselect = Cond.Instance.Get<Comp>(battleui, Label.Assemble(Label.LEVEL, Label.SELECT));
                levelselect.gameObject.SetActive(true);
                bool isGetLevelBuffSetting = Loader.LoadSetting().TryGetBuffByCount(3, out List<BuffSettingInfo> buffSettings);
                if (isGetLevelBuffSetting) {
                    Button A = Cond.Instance.Get<Button>(levelselect, Label.A);
                    ButtonRegister.AddListener(A, SelectBuffSetting, buffSettings[0]);
                    Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.A).text = buffSettings[0].Description;
                    Cond.Instance.Get<Image>(levelselect, Label.A).sprite = buffSettings[0].Icon;

                    Button B = Cond.Instance.Get<Button>(levelselect, Label.B);
                    ButtonRegister.AddListener(B, SelectBuffSetting, buffSettings[1]);
                    Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.B).text = buffSettings[1].Description;
                    Cond.Instance.Get<Image>(levelselect, Label.B).sprite = buffSettings[1].Icon;

                    Button C = Cond.Instance.Get<Button>(levelselect, Label.C);
                    ButtonRegister.AddListener(C, SelectBuffSetting, buffSettings[2]);
                    Cond.Instance.Get<TextMeshProUGUI>(levelselect, Label.C).text = buffSettings[2].Description;
                    Cond.Instance.Get<Image>(levelselect, Label.C).sprite = buffSettings[2].Icon;
                }
            }
        }

        /*选择BUFF配置*/
        private void SelectBuffSetting(BuffSettingInfo buffSettingInfo) {
            SetCanControl(true);
            Comp levelselect = Cond.Instance.Get<Comp>(battleFlow.GetUI(), Label.Assemble(Label.LEVEL, Label.SELECT));
            levelselect.gameObject.SetActive(false);
            if(Be)
            BehaviourRegister.Instance.RegisterBehaviour(entity.ID, buffSettingInfo.BehaviourSign);
        }

        /*设置是否可控*/
        private void SetCanControl(bool canControl) {
            Cond.Instance.GetPlayerEntity().EntityData.BaseRuntimeData.PlayerInfo.AllowMovement = canControl;
        }

        public override void Clear() {
            base.Clear();
            MessageRegister.Instance.UnReg(MessageCode.PlayerUpgrade, LevelUp);
        }
    }
}