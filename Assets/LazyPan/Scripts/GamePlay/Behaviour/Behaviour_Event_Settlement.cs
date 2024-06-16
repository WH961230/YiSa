// using UnityEngine;
// using UnityEngine.UI;
//
// namespace LazyPan {
//     public class Behaviour_Event_Settlement : Behaviour {
//         public Behaviour_Event_Settlement(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
//             Settlement();
//         }
//
// 		/*结算*/
// 		private void Settlement() {
//             Time.timeScale = 0;
//             Flo.Instance.GetFlow(out Flow_Battle flowBattle);
//             Comp settlement = Cond.Instance.Get<Comp>(flowBattle.GetUI(), Label.SETTLEMENT);
//             settlement.gameObject.SetActive(true);
//
//             Button returnBtn = Cond.Instance.Get<Button>(settlement, Label.RETURN);
//             ButtonRegister.RemoveAllListener(returnBtn);
//             ButtonRegister.AddListener(returnBtn, Next, "Begin");
//
//             Button againBtn = Cond.Instance.Get<Button>(settlement, Label.AGAIN);
//             ButtonRegister.RemoveAllListener(againBtn);
//             ButtonRegister.AddListener(againBtn, Next, "Battle");
//         }
//
//         /*下一步*/
//         private void Next(string sceneSign) {
//             Time.timeScale = 1;
//             Flo.Instance.GetFlow(out Flow_Battle flowBattle);
//             flowBattle.Next(sceneSign);
//         }
//     }
// }