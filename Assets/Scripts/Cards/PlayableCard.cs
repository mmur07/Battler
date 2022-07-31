using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace battler
{
    public class PlayableCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image sprite;
        [Header("Animation settings")]
        [SerializeField] private RectTransform animationRect;
        [SerializeField] private AnimationClip unhoverClip;
        [SerializeField] private AnimationClip hoverClip;
        [SerializeField] private float onHoverScale = 1.25f;
        [SerializeField] private float onHoverOffsetY = 15f;
        [SerializeField] private float onHoverAnimTime = 0.33f;
        private Animation animation;
        private RectTransform transform;
        private CanvasGroup canvasGroup;

        private bool pointerHover = false;
        private bool isPointerClicked = false;
        private Canvas canvas;
        private Hand hand;

        //----------------------------------------
        private void Start()
        {
            animation = GetComponentInChildren<Animation>();
            transform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
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

        public void SetReferences(Canvas cnvas, Hand playerHand)
        {
            canvas = cnvas;
            hand = playerHand;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerHover = false;

            AnimationCurve curve1 = AnimationCurve.Linear(0f, animationRect.localScale.x, onHoverAnimTime, 1f);
            AnimationCurve curve2 = AnimationCurve.Linear(0f, animationRect.localScale.y, onHoverAnimTime, 1f);
            AnimationCurve curve3 = AnimationCurve.Linear(0f, animationRect.anchoredPosition.y, onHoverAnimTime, 0f);

            unhoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.x", curve1);
            unhoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.y", curve2);
            unhoverClip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve3);

            animation.AddClip(unhoverClip, unhoverClip.name);
            animation.Play(unhoverClip.name);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!hand.IsDraggingCard())
            {
                pointerHover = true;

                AnimationCurve curve1 = AnimationCurve.Linear(0f, animationRect.localScale.x, onHoverAnimTime, onHoverScale);
                AnimationCurve curve2 = AnimationCurve.Linear(0f, animationRect.localScale.y, onHoverAnimTime, onHoverScale);
                AnimationCurve curve3 = AnimationCurve.Linear(0f, animationRect.anchoredPosition.y, onHoverAnimTime, onHoverOffsetY);

                hoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.x", curve1);
                hoverClip.SetCurve("", typeof(RectTransform), "m_LocalScale.y", curve2);
                hoverClip.SetCurve("", typeof(RectTransform), "m_AnchoredPosition.y", curve3);

                animation.AddClip(hoverClip, hoverClip.name);
                animation.Play(hoverClip.name);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!hand.IsDraggingCard())
            {
                isPointerClicked = false;
                canvasGroup.blocksRaycasts = false;
                hand.SetDraggingCard(true);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (hand.IsDraggingCard())
            {
                isPointerClicked = true;
                canvasGroup.blocksRaycasts = true;
                hand.SetDraggingCard(false);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(hand.IsDraggingCard()) transform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
