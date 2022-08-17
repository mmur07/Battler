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

        //Gameobject components
        private Animation animation;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        //Scene components
        private Canvas canvas;
        private Hand hand;

        //Animation variables
        private Vector3 endPosition;
        private Vector3 startPosition;
        private float elapsedTime = 0f;
        private bool moving = false;

        //Misc
        private int handIndex;
        protected CardType cardType = CardType.None;

        //----------------------------------------

        private void Start()
        {
            animation = GetComponentInChildren<Animation>();
            rectTransform = GetComponent<RectTransform>();
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

        }

        //At first glance it may make more sense to pass the startPos and endPos here, but sometimes we need to change these variables (specially endPos) mid-animation.
        //There is probably a better solution, but i'm keeping it as it is ATM.
        private IEnumerator TranslateToEndPosition()
        {
            float percentageComplete = 0f;
            while (percentageComplete < 1f) { 

                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / returnToHandAnimTime;

                rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));

                yield return null;
            }

            elapsedTime = 0f;
            moving = false;
        }

        public void SetName(string name)
        {
            nameText.text = name;
        }

        public void SetDescription(string description)
        {
            descriptionText.text = description;
        }

        public void SetSprite(Sprite sprt)
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

        //If the card is returned to the player's hand (not placed in board, board fulll...) it plays a return to hand animation. When starting to drag the card,
        //we save this position for the animation.
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!hand.IsDraggingCard() && !moving)
            {
                endPosition = rectTransform.anchoredPosition;
                canvasGroup.blocksRaycasts = false;
                hand.PickupCard(this);
            }
        }

        //When the player's finished dragging a card, if it can't be played it returns to the player's hand, so it plays the return to hand animation.
        public void OnEndDrag(PointerEventData eventData)
        {
            if (hand.IsDraggingCard())
            {
                //Returns true when the card can be positioned in the field
                if (!hand.DropCardInField(handIndex))
                {
                    startPosition = rectTransform.anchoredPosition;
                    moving = true;
                    StartCoroutine(TranslateToEndPosition());
                    canvasGroup.blocksRaycasts = true;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(hand.IsDraggingCard()) rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        //------------------------------------------------------

        public bool IsMoving()
        {
            return moving;
        }

        public void SetEndAnimPosition(Vector2 endPos)
        {
            endPosition = endPos;
        }

        public Vector2 GetEndPosition()
        {
            return endPosition;
        }

        public void SetIndex(int idx)
        {
            handIndex = idx;
        }

        public int GetIndex()
        {
            return handIndex;
        }

        public CardType GetCardType()
        {
            return cardType;
        }
    }
}
