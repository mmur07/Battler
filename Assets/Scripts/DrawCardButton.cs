using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class DrawCardButton : MonoBehaviour
    {
        [SerializeField] private BoardManager boardManager;
        [SerializeField] private List<Card> cardDrawList;

        private void Start()
        {
            if (boardManager == null || cardDrawList.Count == 0)
            {
                Debug.LogError("DrawCardButton: Missing reference from editor");
                return;
            }
        }

        public void OnPress()
        {
            Card randomCard = cardDrawList[Random.Range(0, cardDrawList.Count - 1)];

            boardManager.AddCardToHand(randomCard);
        }
    }
}