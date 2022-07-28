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
        private List<PlayableCard> cards = new List<PlayableCard>();

        private void Start()
        {
#if UNITY_EDITOR
            if (canvas == null)
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

        private void RecalculateCardPositions()
        {
            float xJumpPerCard = handMaxWidth / (cards.Count + 1);
            float iniPos = -handMaxWidth * 0.5f;

            for(int k = 0; k < cards.Count; k++)
            {
                cards[k].GetComponent<RectTransform>().anchoredPosition = new Vector2(iniPos + (xJumpPerCard * (k + 1)), 0);
            }
        }
    }
}