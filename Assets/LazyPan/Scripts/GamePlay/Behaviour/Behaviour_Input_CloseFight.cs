using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace LazyPan {
    public class Behaviour_Input_CloseFight : Behaviour {
        private TimelineAsset asset;
        private PlayableDirector playableDirector;
        private TrailRenderer trailRenderer;
        private Animator animator;
        private bool isPlay;

        public Behaviour_Input_CloseFight(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            asset = Cond.Instance.Get<TimelineAsset>(entity,  Label.Assemble(Label.CLOSEFIGHT, Label.TIMELINEASSET));
            playableDirector = Cond.Instance.Get<PlayableDirector>(entity, Label.PLAYABLEDIRECTOR);
            trailRenderer = Cond.Instance.Get<TrailRenderer>(entity, Label.Assemble(Label.SWORD, Label.TRAILRENDERER));
            animator = Cond.Instance.Get<Animator>(entity, Label.ANIMATOR);
            playableDirector.played += playableDirector => { isPlay = true; trailRenderer.Clear(); animator.enabled = true; };
            playableDirector.stopped += director => { isPlay = false; animator.enabled = false; };
            InputRegister.Instance.Load(InputRegister.Instance.LeftClick, CloseFight);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.CLOSEFIGHT, Label.TRIGGER, Label.COMP))
                .OnTriggerEnterEvent.AddListener(OnCloseFightTriggerEnter);
        }

        private void OnCloseFightTriggerEnter(Collider arg0) {
            if (arg0.gameObject.layer != LayerMask.NameToLayer("BeHit")) {
                return;
            }
            if (Data.Instance.TryGetEntityByBodyPrefabID(arg0.gameObject.GetInstanceID(), out Entity tmpEntity)) {
                tmpEntity.EntityData.BaseRuntimeData.CurHealth -= 10 /*伤害*/;
                Debug.Log($"curHealth:{entity.EntityData.BaseRuntimeData.CurHealth}");
                /*掉血表现*/
                GameObject template = Loader.LoadGo("掉血", "Obj/Fight/Obj_Fx_BeHit", Data.Instance.ObjRoot, true);
                Transform squirt = Cond.Instance.Get<Transform>(tmpEntity, Label.SQUIRT);
                template.transform.position = squirt.position;
                template.transform.rotation = squirt.rotation;
                /*击退表现*/
                tmpEntity.EntityData.BaseRuntimeData.CurKnockbackDir = (
                    Cond.Instance.Get<Transform>(tmpEntity, Label.BODY).position -
                    Cond.Instance.Get<Transform>(entity, Label.BODY).position).normalized;
                tmpEntity.EntityData.BaseRuntimeData.CurKnockbackDeployTime =
                    tmpEntity.EntityData.BaseRuntimeData.DefKnockbackSpeed;
            }
        }

        private void CloseFight(InputAction.CallbackContext obj) {
            if (isPlay) {
                return;
            }
            playableDirector.playableAsset = asset;
            playableDirector.Play();
        }

        public override void Clear() {
            base.Clear();
            InputRegister.Instance.UnLoad(InputRegister.Instance.LeftClick, CloseFight);
            Cond.Instance.Get<Comp>(entity, Label.Assemble(Label.CLOSEFIGHT, Label.TRIGGER, Label.COMP))
                .OnTriggerEnterEvent.RemoveListener(OnCloseFightTriggerEnter);
        }
    }
}