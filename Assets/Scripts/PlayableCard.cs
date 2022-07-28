using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace battler
{
    public class PlayableCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image sprite;
        [SerializeField] private RectTransform animationRect;
        [SerializeField] private AnimationClip unhoverClip;
        [SerializeField] private AnimationClip hoverClip;

        private bool pointerHover = false;
        private Animation animation;

        //----------------------------------------
        private void Start()
        {
            animation = GetComponentInChildren<Animation>();
#if UNITY_EDITOR
            if (animationRect == null || unhoverClip == null || hoverClip == null) {
                Debug.LogError("PlayableCard: Missing reference from editor");
                return;
            }
#endif
        }

        public void setName(string name)
        {
            nameText.text = name;
        }

        public void setDescription(string description)
        {
            descriptionText.text = description;
        }

        public void setSprite(Sprite sprt)
        {
            sprite.sprite = sprt;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Sali");
            pointerHover = false;

            float startTime = 0f;
            float endTime = .33f;
            float endScale = 1f;
            float endYOffset = 25f;
            
            AnimationCurve curve1 = AnimationCurve.Linear(startTime, animationRect.localScale.x, endTime, endScale);
            AnimationCurve curve2 = AnimationCurve.Linear(startTime, animationRect.localScale.y, endTime, endScale);
            unhoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.x", curve1);
            unhoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.y", curve2);

            animation.AddClip(unhoverClip, unhoverClip.name);
            animation.Play(unhoverClip.name);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Entre");
            pointerHover = true;

            float startTime = 0f;
            float endTime = .33f;
            float endScale = 1.25f;
            float endYOffset = 25f;

            AnimationCurve curve1 = AnimationCurve.Linear(startTime, animationRect.localScale.x, endTime, endScale);
            AnimationCurve curve2 = AnimationCurve.Linear(startTime, animationRect.localScale.y, endTime, endScale);

            hoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.x", curve1);
            hoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.y", curve2);

            animation.AddClip(hoverClip, hoverClip.name);
            animation.Play(hoverClip.name);
        }
    }
}
