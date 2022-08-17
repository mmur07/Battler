using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] int maxCards = 10;
        [SerializeField] private float handMaxWidth = 20f;
        [SerializeField] private float handMaxHeight = 15f;
        [SerializeField] private RectTransform canvas;
        [SerializeField] private RectTransform cardLookPosition;
        [SerializeField] private BoardManager board;
        private List<PlayableCard> cards = new List<PlayableCard>();

        private PlayableCard holdingCard = null;

        private List<List<CardPlacementData>> cardPositions;

        private struct CardPlacementData
        {
            public Vector2 position;
            public Quaternion rotation;
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (canvas == null || board == null)
            {
                Debug.LogError("Hand: Missing reference from editor.");
                return;
            }
#endif
            CalculateCardsPositions();
        }

        private void CalculateCardsPositions()
        {
            cardPositions = new List<List<CardPlacementData>>(maxCards);

            float xJumpPerCard, iniPos;

            for (int numCards = 0; numCards <= maxCards; numCards++)
            {
                cardPositions.Add(new List<CardPlacementData>());

                xJumpPerCard = handMaxWidth / (numCards + 1);
                iniPos = -handMaxWidth * 0.5f;

                for (int cardIndex = 0; cardIndex < numCards; cardIndex++)
                {
                    float yPos = Mathf.Sin((float)(cardIndex + 1) / (numCards + 1) * Mathf.PI) * handMaxHeight;

                    Vector2 cardPos = new Vector2(iniPos + (xJumpPerCard * (cardIndex + 1)), yPos);
                    Vector3 lookPos;

                    lookPos = cardLookPosition.anchoredPosition - cardPos;

                    Quaternion rotation = Quaternion.LookRotation(lookPos);

                    CardPlacementData cardPlacement;
                    cardPlacement.position = cardPos;
                    cardPlacement.rotation = rotation;

                    cardPositions[numCards].Add(cardPlacement);
                }
            }
        }

        public void AddCard(PlayableCard newCard)
        {
            cards.Add(newCard);
            RepositionCards();
        }

        public void UseCard(int cardIndex)
        {
            cards.RemoveAt(cardIndex);
        }

        private void RepositionCards()
        {
            for(int k = 0; k < cards.Count; k++)
            {
                RectTransform cardTransform = cards[k].GetComponent<RectTransform>();
                //If the card's moving, change the end position of the animation and the rotation as if it were already in place
                if (!cards[k].IsMoving())
                {
                    cardTransform.anchoredPosition = cardPositions[cards.Count][k].position;
                    cardTransform.rotation = cardPositions[cards.Count][k].rotation;
                }
                else {
                    cards[k].SetEndAnimPosition(cardPositions[cards.Count][k].position);
                    Vector2 lookPos = cardLookPosition.anchoredPosition - cards[k].GetEndPosition();

                    cardTransform.rotation = Quaternion.LookRotation(lookPos);
                }
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
                RepositionCards();
                return true;
            }
            return false;
        }

        //---------------------------------------------------

        public bool IsDraggingCard()
        {
            return holdingCard != null;
        }

        public PlayableCard GetDraggingCard()
        {
            return holdingCard;
        }

        public bool IsFull()
        {
            return cards.Count < maxCards;
        }
    }
}