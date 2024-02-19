using UnityEngine;

namespace LazyPan {
    public class Behaviour_ArrowMark : Behaviour {
        private GameObject arrowGo;
        private UIArrow arrow;
        public Behaviour_ArrowMark(Entity entity, string sign) : base(entity, sign) {
            GameObject arrowTemplate = UI.Instance.Get("UI_Fight").Get<GameObject>("UI_ArrowTemplate");
            arrowGo = Object.Instantiate(arrowTemplate, UI.Instance.Get("UI_Fight").transform);
            arrowGo.name = string.Concat("地图方位标记_", entity.ID);
            arrowGo.SetActive(true);
            arrow = arrowGo.GetComponent<UIArrow>();
            arrow.OnInit();
            arrow.SetImage(entity.EntitySetting.ArrowMarkSprite);
            arrow.SetTarget(entity.Comp.Get<Transform>("ArrowMarkTran"));
        }

        public override void OnClear() {
            base.OnClear();
            arrow.SetTarget(null);
        }
    }
}