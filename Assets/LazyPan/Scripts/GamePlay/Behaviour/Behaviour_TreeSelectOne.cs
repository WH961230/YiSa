using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class Behaviour_TreeSelectOne : Behaviour {
        public Behaviour_TreeSelectOne(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            MessageRegister.Instance.Reg<Entity>(MessageCode.LevelUp, OpenTreeSelectOne);
        }

        private void OpenTreeSelectOne(Entity tempEntity) {
            if (tempEntity.ID == entity.ID) {
                GameObject LevelUpGo = GameObject.Instantiate(entity.EntitySetting.LevelUpGo, entity.Prefab.transform);
                LevelUpGo.transform.localPosition = Vector3.zero;
                Sound.Instance.SoundPlay("LevelUp", entity.Prefab.transform.position, false, 3);
                ClockUtil.Instance.AlarmAfter(1.5f, () => {
                    Time.timeScale = 0;
                    Comp comp = UI.Instance.Open("UI_Select");
                    int[] buffKeys = GetIndexs(BuffConfig.GetKeys().Count, 3);
                    Debug.Log("buffKey[0] " + buffKeys[0] + " buffKey[1] " + buffKeys[1] + " buffKey[2] " + buffKeys[2]);
                    Data.Instance.TryGetEntityByObjType(ObjType.MainPlayer, out Entity playerEntity);
                    if (Data.Instance.TryGetBuffSetting(BuffConfig.GetKeys()[buffKeys[0]],
                        out BuffSetting buffSetting1)) {
                        comp.Get<Image>("UI_Select_Img1").sprite = buffSetting1.Icon;
                        comp.Get<TextMeshProUGUI>("UI_Select_Text1").text = buffSetting1.Detail;
                        ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Select_Btn1"));
                        ButtonRegister.AddListener(comp.Get<Button>("UI_Select_Btn1"), () => {
                            BehaviourRegister.Instance.RegisterBehaviour(playerEntity.ID,
                                BuffConfig.GetKeys()[buffKeys[0]]);
                            UI.Instance.Close();
                            Time.timeScale = 1;
                        });
                    }

                    if (Data.Instance.TryGetBuffSetting(BuffConfig.GetKeys()[buffKeys[1]],
                        out BuffSetting buffSetting2)) {
                        comp.Get<Image>("UI_Select_Img2").sprite = buffSetting2.Icon;
                        comp.Get<TextMeshProUGUI>("UI_Select_Text2").text = buffSetting2.Detail;
                        ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Select_Btn2"));
                        ButtonRegister.AddListener(comp.Get<Button>("UI_Select_Btn2"), () => {
                            BehaviourRegister.Instance.RegisterBehaviour(playerEntity.ID,
                                BuffConfig.GetKeys()[buffKeys[1]]);
                            UI.Instance.Close();
                            Time.timeScale = 1;
                        });
                    }

                    if (Data.Instance.TryGetBuffSetting(BuffConfig.GetKeys()[buffKeys[2]],
                        out BuffSetting buffSetting3)) {
                        comp.Get<Image>("UI_Select_Img3").sprite = buffSetting3.Icon;
                        comp.Get<TextMeshProUGUI>("UI_Select_Text3").text = buffSetting3.Detail;
                        ButtonRegister.RemoveAllListener(comp.Get<Button>("UI_Select_Btn3"));
                        ButtonRegister.AddListener(comp.Get<Button>("UI_Select_Btn3"), () => {
                            BehaviourRegister.Instance.RegisterBehaviour(playerEntity.ID,
                                BuffConfig.GetKeys()[buffKeys[2]]);
                            UI.Instance.Close();
                            Time.timeScale = 1;
                        });
                    }
                });
            }
        }

        private int[] GetIndexs(int maxCount, int requireIntLength) {
            if (maxCount < requireIntLength) {
                return null;
            }

            int tmpMaxCount = maxCount;
            int index = 0;
            int[] tempArray = new int[maxCount]; //根据最大数量生成的顺序数组
            while (tmpMaxCount > 0) {
                tempArray[index] = index;
                index++;
                tmpMaxCount--;
            }

            int[] tempRetArray = new int[requireIntLength]; //结果数组
            int tmpRequireIntLength = requireIntLength;
            index = 0;
            while (tmpRequireIntLength > 0) {
                int randIndex = Random.Range(0, tempArray.Length - 1);
                GetIndexNotEqualMinusOne(ref tempArray, ref tempRetArray, index, randIndex);
                index++;
                tmpRequireIntLength--;
            }

            return tempRetArray;
        }

        private void GetIndexNotEqualMinusOne(ref int[] parentIndexs, ref int[] insertIndexs, int insertIndex,
            int index) {
            if (parentIndexs[index] != -1) {
                //如果顺序数组当前的值不为-1则加入
                insertIndexs[insertIndex] = parentIndexs[index];
                parentIndexs[index] = -1;
            } else {
                GetIndexNotEqualMinusOne(ref parentIndexs, ref insertIndexs, insertIndex, (index + 1) % parentIndexs.Length);
            }
        }

        public override void OnClear() {
            base.OnClear();
            MessageRegister.Instance.UnReg<Entity>(MessageCode.LevelUp, OpenTreeSelectOne);
        }
    }
}