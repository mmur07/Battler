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
        [SerializeField] private float returnToHandAnimTime = 0.5f;
        private Animation animation;
        private RectTransform transform;
        private CanvasGroup canvasGroup;

        private Canvas canvas;
        private Hand hand;

        private Vector3 endPosition;
        private Vector3 startPosition;
        private float elapsedTime = 0f;

        private bool moving = false;

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

        private void Update()
        {
            //Apply LERP animation
            if(moving)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / returnToHandAnimTime;

                transform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));

                if(percentageComplete >= 1f)
                {
                    elapsedTime = 0f;
                    moving = false;
                }
            }
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
            //This animation returns the original scale and position of the card
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
            //Hovering animation will only start if the user's not selecting a different card
            if (!hand.IsDraggingCard())
            {
                //This animation scales up the card and moves it a little bit up
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
            if (!hand.IsDraggingCard() && !moving)
            {
                endPosition = transform.anchoredPosition;
                canvasGroup.blocksRaycasts = false;
                hand.SetDraggingCard(true);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (hand.IsDraggingCard())
            {
                startPosition = transform.anchoredPosition;
                moving = true;
                canvasGroup.blocksRaycasts = true;
                hand.SetDraggingCard(false);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(hand.IsDraggingCard()) transform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        //------------------------------------------------------

        public bool isMoving()
        {
            return moving;
        }

        public void setEndAnimPosition(Vector2 endPos)
        {
            endPosition = endPos;
        }

        public Vector2 getEndPosition()
        {
            return endPosition;
        }
    }
}
