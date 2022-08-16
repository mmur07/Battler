using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private float handMaxWidth = 20f;
        [SerializeField] private float handMaxHeight = 15f;
        [SerializeField] private RectTransform canvas;
        [SerializeField] private RectTransform cardLookPosition;
        [SerializeField] private BoardManager board;
        private List<PlayableCard> cards = new List<PlayableCard>();

        private PlayableCard holdingCard = null;

        private void Start()
        {
#if UNITY_EDITOR
            if (canvas == null || board == null)
            {
                Debug.LogError("Hand: Missing reference from editor.");
                return;
            }
#endif
        }

        public void AddCard(PlayableCard newCard)
        {
            cards.Add(newCard);
            RecalculateCardPositions();
        }

        public void UseCard(int cardIndex)
        {
            cards.RemoveAt(cardIndex);
        }

        private void Update()
        {
            //RecalculateCardPositions();
        }

        private void RecalculateCardPositions()
        {
            float xJumpPerCard = handMaxWidth / (cards.Count + 1);
            float iniPos = -handMaxWidth * 0.5f;

            for(int k = 0; k < cards.Count; k++)
            {
                float yPos = Mathf.Sin((float)(k + 1) / (cards.Count + 1) * Mathf.PI) * handMaxHeight;
                RectTransform cardTransform = cards[k].GetComponent<RectTransform>();
                Vector2 endPosition = new Vector2(iniPos + (xJumpPerCard * (k + 1)), yPos);
                Vector3 lookPos;
                //If the card's moving, change the end position of the animation and the rotation as if it were already in place
                if (!cards[k].IsMoving())
                {
                    cardTransform.anchoredPosition = endPosition;
                    lookPos = cardLookPosition.anchoredPosition - cardTransform.anchoredPosition;
                }
                else {
                    cards[k].SetEndAnimPosition(endPosition);
                    lookPos = cardLookPosition.anchoredPosition - cards[k].GetEndPosition();
                }

                Quaternion rotation = Quaternion.LookRotation(lookPos);
                cardTransform.rotation = rotation;
                if(cardTransform.transform.rotation.y > 0) cardTransform.transform.Rotate(0, -90, 90);
                else cardTransform.transform.Rotate(0, 90, -90);

                //Assign hand index
                cards[k].SetIndex(k);
            }
        }

        public void PickupCard(PlayableCard card)
        {
            holdingCard = card;
        }

        public bool DropCardInField(int index)
        {
            //Should ask the boardManager if there's enough space in board, if it's dropped in the correct position...
            holdingCard = null;
            if (board.DropCardInField(cards[index]))
            {
                cards.RemoveAt(index);
                RecalculateCardPositions();
                return true;
            }
            return false;
        }

        //---------------------------------------------------

        public bool IsDraggingCard()
        {
            return holdingCard != null;
        }
    }
}