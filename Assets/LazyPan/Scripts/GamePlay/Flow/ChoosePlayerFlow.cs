using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyPan {
    public class ChoosePlayerFlow : Flow {
        private GameObject DefaultPlayerModel;
        private List<Comp> ChooseCompList = new List<Comp>();
        private Transform CurrentSelect;
        private Entity CurrentEntity;
        private Comp choosePlayerComp;
        private GameObject SoundGo;
        public override void OnInit(Flow baseFlow) {
            base.OnInit(baseFlow);
            if (SoundGo == null) {
                SoundGo = Sound.Instance.SoundPlay("ChoosePlayerFlowLoop", Vector3.zero, true, -1);
            }

            if (DefaultPlayerModel == null) {
                DefaultPlayerModel = GameObject.Find("Obj_BeginPlayerModel");
            }
            DefaultPlayerModel.SetActive(false);
            Data.Instance.PlayerEntitySign = null;

            choosePlayerComp = UI.Instance.Open("UI_ChoosePlayer");

            CreateAllChoosePlayerItem();

            Button choosePlayerBtn = choosePlayerComp.Get<Button>("UI_ChoosePlayer_NextBtn");
            ButtonRegister.RemoveAllListener(choosePlayerBtn);
            ButtonRegister.AddListener(choosePlayerBtn, () => {
                Obj.Instance.UnLoadEntity(CurrentEntity);
                BaseFlow.ChangeFlow<FightFlow>();
            });

            Data.Instance.OnLateUpdateEvent.AddListener(OnFollowTargetUpdate);
        }

        private void OnFollowTargetUpdate() {
            if (Data.Instance.TryGetEntityByObjType(ObjType.MainCamera, out Entity entity)) {
                entity.Prefab.transform.position = Vector3.Lerp(entity.Prefab.transform.position, GameObject.Find("MainCameraPosition_1").transform.position, Time.deltaTime * 5);
                entity.Prefab.transform.rotation = Quaternion.Lerp(entity.Prefab.transform.rotation, GameObject.Find("MainCameraPosition_1").transform.rotation, Time.deltaTime * 5);
            }
        }

        private void CreateAllChoosePlayerItem() {
            List<string> objSignList = new List<string>();
            foreach (string sign in ObjConfig.GetKeys()) {
                string type = ObjConfig.Get(sign).ObjTypeSign;
                if (type == "Player") {
                    objSignList.Add(sign);
                }
            }

            ChooseCompList.Clear();
            foreach (string objSign in objSignList) {
                Comp choosePlayerItem = CreateChoosePlayerItem(objSign);
                if (CurrentSelect == null) {
                    OnSelectEvent(choosePlayerItem);
                }
            }
        }

        private Comp CreateChoosePlayerItem(string objSign) {
            //创建按钮
            GameObject choosePlayerItem = choosePlayerComp.Get<GameObject>("UI_ChoosePlayer_Item");
            Transform choosePlayerParent = choosePlayerComp.Get<Transform>("UI_ChoosePlayer_ItemParent");
            GameObject choosePlayerGo = Object.Instantiate(choosePlayerItem, choosePlayerParent);
            choosePlayerGo.SetActive(true);
            //注册角色按钮
            Comp choosePlayerGoComp = choosePlayerGo.GetComponent<Comp>();
            choosePlayerGoComp.ObjInfo = objSign;
            choosePlayerGoComp.Get<TextMeshProUGUI>("ChoosePlayerItemDetail").text = ObjConfig.Get(objSign).Name;
            choosePlayerGoComp.Get<Transform>("Select").gameObject.SetActive(false);
            //按钮监听
            Button choosePlayerItemBtn = choosePlayerGoComp.Get<Button>("ChoosePlayerItem_Btn");
            ButtonRegister.AddListener(choosePlayerItemBtn, OnButtonEvent, choosePlayerGoComp);
            //按钮列表
            ChooseCompList.Add(choosePlayerGoComp);
            return choosePlayerGoComp;
        }

        private void OnButtonEvent(Comp comp) {
            OnSelectEvent(comp);
        }

        private void OnSelectEvent(Comp comp) {
            Transform selectTran = comp.Get<Transform>("Select");
            if (CurrentSelect != null) {
                CurrentSelect.gameObject.SetActive(false);
                CurrentSelect = null;
                Obj.Instance.UnLoadEntity(CurrentEntity);
            }
            CurrentSelect = selectTran;
            CurrentSelect.gameObject.SetActive(true);
            Data.Instance.PlayerEntitySign = comp.ObjInfo;
            CurrentEntity = Obj.Instance.LoadEntity(string.Concat(comp.ObjInfo, "_Model"));
            CurrentEntity.Comp.Get<Animator>("Animator").SetTrigger(string.Concat(comp.ObjInfo, "_Model"));
        }

        public override void OnClear() {
            base.OnClear();
            Sound.Instance.SoundRecycle(SoundGo);
            SoundGo = null;

            foreach (Comp chooseComp in ChooseCompList) {
                Button chooseBtn = chooseComp.Get<Button>("ChoosePlayerItem_Btn");
                ButtonRegister.RemoveListener(chooseBtn, OnButtonEvent, chooseComp);
                Object.Destroy(chooseComp.gameObject);
            }
            UI.Instance.Close("UI_ChoosePlayer");
            Data.Instance.OnLateUpdateEvent.RemoveListener(OnFollowTargetUpdate);
        }
    }
}