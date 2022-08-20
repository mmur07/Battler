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
                    if (rotation.y > 0) rotation *= Quaternion.Euler(0, -90, 90);
                    else rotation *= Quaternion.Euler(0, 90, -90);

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
            RepositionHand();
        }

        public void UseCard(int cardIndex)
        {
            cards.RemoveAt(cardIndex);
        }

        private void RepositionHand()
        {
            for (int k = 0; k < cards.Count; k++)
            {
                RepositionCard(k, cardPositions[cards.Count][k].position, cardPositions[cards.Count][k].rotation);
            }
        }

        private void RepositionHand(int ignoreCard)
        {
            if (ignoreCard > cards.Count)
            {
                Debug.LogError("Index of ignored card greater than number of cards");
                return;
            }

            for(int k = 0; k < ignoreCard; k++)
            {
                RepositionCard(k, cardPositions[cards.Count - 1][k].position, cardPositions[cards.Count - 1][k].rotation);
            }
            for (int k = ignoreCard + 1; k < cards.Count; k++)
            {
                RepositionCard(k, cardPositions[cards.Count - 1][k - 1].position, cardPositions[cards.Count - 1][k - 1].rotation);
            }
        }

        private void RepositionCard(int cardIdx, Vector2 pos, Quaternion rot)
        {
            //If the card's moving, change the end position of the animation and the rotation as if it were already in place
            if (!cards[cardIdx].IsMoving())
            {
                cards[cardIdx].TranslateAnimation(pos, rot);
            }
            else
            {
                cards[cardIdx].SetEndAnimPosition(pos);
                cards[cardIdx].SetEndAnimRotation(rot);
            }

            cards[cardIdx].SetIndex(cardIdx);
        }

        public void PickupCard(PlayableCard card)
        {
            holdingCard = card;
            RepositionHand(card.GetIndex());
        }

        public bool DropCardInField(int index)
        {
            //Should ask the boardManager if there's enough space in board, if it's dropped in the correct position...
            holdingCard = null;
            if (board.DropCardInField(cards[index]))
            {
                cards.RemoveAt(index);
                RepositionHand();
                return true;
            }
            RepositionHand();
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