using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using Slider = UnityEngine.UI.Slider;

namespace LazyPan {
    public class Comp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [HideInInspector] public UnityEvent<Collider> OnTriggerEnterEvent;
        [HideInInspector] public UnityEvent<Collider> OnTriggerStayEvent;
        [HideInInspector] public UnityEvent<Collider> OnTriggerExitEvent;
        
        [HideInInspector] public UnityEvent<Collision> OnCollisionEnterEvent;
        [HideInInspector] public UnityEvent<Collision> OnCollisionStayEvent;
        [HideInInspector] public UnityEvent<Collision> OnCollisionExitEvent;

        [HideInInspector] public UnityEvent OnBecameVisibleEvent;
        [HideInInspector] public UnityEvent OnBecameInvisibleEvent;

        [HideInInspector] public UnityEvent<PointerEventData> OnPointerEnterEvent;
        [HideInInspector] public UnityEvent<PointerEventData> OnPointerExitEvent;

        [HideInInspector] public UnityEvent OnDrawGizmosAction;
        public string ObjSign;
        public string ObjInfo;
        public Entity Entity;
        public List<GameObjectData> GameObjects = new List<GameObjectData>();
        public List<TransformData> Transforms = new List<TransformData>();
        public List<ColliderData> Colliders = new List<ColliderData>();
        public List<CharacterControllerData> CharacterControllers = new List<CharacterControllerData>();
        public List<ButtonData> Buttons = new List<ButtonData>();
        public List<SliderData> Sliders = new List<SliderData>();
        public List<TextData> Texts = new List<TextData>();
        public List<TextMeshProUGUIData> TextMeshProUGUIs = new List<TextMeshProUGUIData>();
        public List<TMP_InputFieldData> TMPInputFields = new List<TMP_InputFieldData>();
        public List<AnimatorData> Animators = new List<AnimatorData>();
        public List<ImageData> Images = new List<ImageData>();
        public List<PlayableDirectorData> PlayableDirectors = new List<PlayableDirectorData>();
        public List<TimelineAssetData> TimelineAssets = new List<TimelineAssetData>();
        public List<CompData> Comps = new List<CompData>();
        public List<UnityEventData> UnityEventDatas = new List<UnityEventData>();
        public List<DoTweenAnimationData> DoTweenAnimationDatas = new List<DoTweenAnimationData>();
        public List<string> BehaviourBundles = new List<string>();

        public T Get<T>(string sign) where T : Object {
            if (typeof(T) == typeof(CharacterController)) {
                foreach (CharacterControllerData controllerData in CharacterControllers) {
                    if (controllerData.Sign == sign) {
                        return controllerData.Controller as T;
                    }
                }
            } else if (typeof(T) == typeof(GameObject)) {
                foreach (GameObjectData gameObjectData in GameObjects) {
                    if (gameObjectData.Sign == sign) {
                        return gameObjectData.GO as T;
                    }
                }
            } else if (typeof(T) == typeof(Transform)) {
                foreach (TransformData transformData in Transforms) {
                    if (transformData.Sign == sign) {
                        return transformData.Tran as T;
                    }
                }
            } else if (typeof(T) == typeof(Button)) {
                foreach (ButtonData buttonData in Buttons) {
                    if (buttonData.Sign == sign) {
                        return buttonData.Button as T;
                    }
                }
            } else if (typeof(T) == typeof(Slider)) {
                foreach (SliderData sliderData in Sliders) {
                    if (sliderData.Sign == sign) {
                        return sliderData.Slider as T;
                    }
                }
            } else if (typeof(T) == typeof(Text)) {
                foreach (TextData textData in Texts) {
                    if (textData.Sign == sign) {
                        return textData.Text as T;
                    }
                }
            } else if (typeof(T) == typeof(TextMeshProUGUI)) {
                foreach (TextMeshProUGUIData textMeshProUGUIData in TextMeshProUGUIs) {
                    if (textMeshProUGUIData.Sign == sign) {
                        return textMeshProUGUIData.TextMeshProUGUI as T;
                    }
                }
            } else if (typeof(T) == typeof(Collider)) {
                foreach (ColliderData colliderData in Colliders) {
                    if (colliderData.Sign == sign) {
                        return colliderData.Collider as T;
                    }
                }
            } else if (typeof(T) == typeof(TMP_InputField)) {
                foreach (TMP_InputFieldData textData in TMPInputFields) {
                    if (textData.Sign == sign) {
                        return textData.Text as T;
                    }
                }
            } else if (typeof(T) == typeof(Animator)) {
                foreach (AnimatorData animatorData in Animators) {
                    if (animatorData.Sign == sign) {
                        return animatorData.Animator as T;
                    }
                }
            } else if (typeof(T) == typeof(Image)) {
                foreach (ImageData imageData in Images) {
                    if (imageData.Sign == sign) {
                        return imageData.Image as T;
                    }
                }
            } else if (typeof(T) == typeof(PlayableDirector)) {
                foreach (PlayableDirectorData playableDirectorData in PlayableDirectors) {
                    if (playableDirectorData.Sign == sign) {
                        return playableDirectorData.PlayableDirector as T;
                    }
                }
            } else if (typeof(T) == typeof(TimelineAsset)) {
                foreach (TimelineAssetData timelineAssetData in TimelineAssets) {
                    if (timelineAssetData.Sign == sign) {
                        return timelineAssetData.TimelineAsset as T;
                    }
                }
            } else if (typeof(T) == typeof(Comp)) {
                foreach (CompData compData in Comps) {
                    if (compData.Sign == sign) {
                        return compData.Comp as T;
                    }
                }
            } else if (typeof(T) == typeof(DOTweenAnimation)) {
                foreach (DoTweenAnimationData compData in DoTweenAnimationDatas) {
                    if (compData.Sign == sign) {
                        return compData.DOTweenAnimation as T;
                    }
                }
            }

            return null;
        }

        public UnityEvent GetEvent(string sign) {
            foreach (UnityEventData data in UnityEventDatas) {
                if (data.Sign == sign) {
                    return data.UnityEvent;
                }
            }

            return null;
        }

        public void OnTriggerEnter(Collider other) {
            OnTriggerEnterEvent?.Invoke(other);
        }

        public void OnTriggerStay(Collider other) {
            OnTriggerStayEvent?.Invoke(other);
        }

        public void OnTriggerExit(Collider other) {
            OnTriggerExitEvent?.Invoke(other);
        }

        public void OnCollisionEnter(Collision other) {
            OnCollisionEnterEvent?.Invoke(other);
        }

        public void OnCollisionStay(Collision other) {
            OnCollisionStayEvent?.Invoke(other);
        }

        public void OnCollisionExit(Collision other) {
            OnCollisionExitEvent?.Invoke(other);
        }

        public void OnBecameVisible() {
            OnBecameVisibleEvent?.Invoke();
        }

        public void OnBecameInvisible() {
            OnBecameInvisibleEvent?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnPointerEnterEvent?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnPointerExitEvent?.Invoke(eventData);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            OnDrawGizmosAction.Invoke();
        }
#endif

        [Serializable]
        public class DoTweenAnimationData {
            public string Sign;
            public DOTweenAnimation DOTweenAnimation;
        }
        
        [Serializable]
        public class UnityEventData {
            public string Sign;
            public UnityEvent UnityEvent;
        }

        [Serializable]
        public class CompData {
            public string Sign;
            public Comp Comp;
        }
        
        [Serializable]
        public class TimelineAssetData {
            public string Sign;
            public TimelineAsset TimelineAsset;
        }

        [Serializable]
        public class PlayableDirectorData {
            public string Sign;
            public PlayableDirector PlayableDirector;
        }

        [Serializable]
        public class ImageData {
            public string Sign;
            public Image Image;
        }

        [Serializable]
        public class AnimatorData {
            public string Sign;
            public Animator Animator;
        }

        [Serializable]
        public class TMP_InputFieldData {
            public string Sign;
            public TMP_InputField Text;
        }

        [Serializable]
        public class ButtonData {
            public string Sign;
            public Button Button;
        }

        [Serializable]
        public class SliderData {
            public string Sign;
            public Slider Slider;
        }

        [Serializable]
        public class TextData {
            public string Sign;
            public Text Text;
        }

        [Serializable]
        public class TextMeshProUGUIData {
            public string Sign;
            public TextMeshProUGUI TextMeshProUGUI;
        }

        [Serializable]
        public class GameObjectData {
            public string Sign;
            public GameObject GO;
        }

        [Serializable]
        public class TransformData {
            public string Sign;
            public Transform Tran;
        }

        [Serializable]
        public class CharacterControllerData {
            public string Sign;
            public CharacterController Controller;
        }

        [Serializable]
        public class ColliderData {
            public string Sign;
            public Collider Collider;
        }
    }
}