// using UnityEngine;
// using UnityEngine.UI;
//
// namespace LazyPan {
//     public class Behaviour_Auto_Pick : Behaviour {
//         private Comp battleui;
//         private Flow_Battle battleFlow;
//         private int lastInstanceID;
//
//         public Behaviour_Auto_Pick(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
//             Comp bodycomp = Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP));
//             Cond.Instance.Get<Comp>(bodycomp, Label.TRIGGER).OnTriggerEnterEvent.AddListener(Pick);
//             Flo.Instance.GetFlow(out battleFlow);
//             battleui = battleFlow.GetUI();
//         }
//
//         /*拾取*/
//         private void Pick(Collider arg0) {
//             if (arg0.gameObject.layer == LayerMask.NameToLayer("Drop")) {
//                 if (lastInstanceID != arg0.gameObject.GetInstanceID()) {
//                     arg0.GetComponent<Collider>().enabled = false;
//                     Object.Destroy(arg0.gameObject);
//                     Entity playerEntity = Cond.Instance.GetPlayerEntity();
//                     playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience += 1;
//                     playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience = Mathf.Min(
//                         playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience,
//                         Loader.LoadSetting().PlayerSetting.MaxExperience);
//                     Comp info = Cond.Instance.Get<Comp>(battleui, Label.INFO);
//                     Cond.Instance.Get<Slider>(info, Label.EXP).value =
//                         playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience /
//                         Loader.LoadSetting().PlayerSetting.MaxExperience;
//                     if (playerEntity.EntityData.BaseRuntimeData.PlayerInfo.Experience >=
//                         Loader.LoadSetting().PlayerSetting.MaxExperience) {
//                         MessageRegister.Instance.Dis(MessageCode.PlayerUpgrade);
//                     }
//                 
//                     lastInstanceID = arg0.gameObject.GetInstanceID();
//                 }
//             }
//         }
//
//         public override void Clear() {
//             base.Clear();
//             Comp bodycomp = Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.BODY, Label.COMP));
//             Cond.Instance.Get<Comp>(bodycomp, Label.TRIGGER).OnTriggerEnterEvent.RemoveListener(Pick);
//         }
//     }
// }