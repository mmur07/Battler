using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace battler
{
    public class Board : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] int maxMinions = 6;
        [SerializeField] float minionOffset = 20f;
        [SerializeField] Hand hand = null;

        private RectTransform rectTransform;

        private bool insideArea = false;
        private int hoveringCardPositionIndex = -1;
        private List<MinionBody> minionsOnBoard = new List<MinionBody>();
        private List<List<float>> minionPositions;

        //------------------------------------------------------

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            if(hand == null)
            {
                Debug.LogError("Board: Missing reference from editor");
                return;
            }

            CalculateMinionsPosition();
        }

        private void CalculateMinionsPosition()
        {
            minionPositions = new List<List<float>>(maxMinions);
            float startingPos;

            for (int k = 0; k <= maxMinions; k++)
            {
                minionPositions.Add(new List<float>());
                if (k % 2 == 0) startingPos = -minionOffset * 0.5f - minionOffset * (Mathf.Max(0, (int)(k * 0.5f - 1)));
                else startingPos = -minionOffset * Mathf.Max(0, (int)(k * 0.5f));
                for (int l = 0; l < k; l++)
                {
                    minionPositions[k].Add(startingPos + l * minionOffset);
                }
            }
        }

        private void Update()
        {
            if(insideArea && hand.IsDraggingCard() && hand.GetDraggingCard().GetCardType() == CardType.Minion && !IsFull())
            {
                int cardPosInBoard = PointerToBoardPosition();
                if(hoveringCardPositionIndex != cardPosInBoard) //Need to translate already positioned cards
                {
                    hoveringCardPositionIndex = cardPosInBoard;
                    for(int k = 0; k < minionsOnBoard.Count; k++)
                    {
                        if(k < cardPosInBoard) minionsOnBoard[k].AnimatedTranslation(new Vector2(minionPositions[minionsOnBoard.Count + 1][k], 0));
                        else minionsOnBoard[k].AnimatedTranslation(new Vector2(minionPositions[minionsOnBoard.Count + 1][k + 1], 0));
                    }
                }
            }
        }

        //Gets the position in board in which the minion would be placed.
        private int PointerToBoardPosition()
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            float xPos = Input.mousePosition.x - corners[0].x - corners[3].x * 0.5f;
            if (minionsOnBoard.Count == 0) return 0;
            if (xPos < minionPositions[minionsOnBoard.Count][0]) return 0;
            if (minionsOnBoard.Count == 1 && xPos > minionPositions[minionsOnBoard.Count][0]) return 1;
            for (int k = 0; k < minionsOnBoard.Count - 1; k++)
            {
                if (xPos > minionPositions[minionsOnBoard.Count][k] && xPos < minionPositions[minionsOnBoard.Count][k + 1]) return k + 1;
            }
            return minionsOnBoard.Count;
        }

        public void PlayCard(MinionBody minion)
        {
            minionsOnBoard.Insert(hoveringCardPositionIndex, minion);
            SetMinionsPosition();
            hoveringCardPositionIndex = -1;
        }

        private void SetMinionsPosition()
        {
            for(int k = 0; k < minionsOnBoard.Count; k++)
            {
                if(minionsOnBoard[k].IsMoving()) minionsOnBoard[k].AnimatedTranslation(new Vector2(minionPositions[minionsOnBoard.Count][k], 0));
                else minionsOnBoard[k].GetComponent<RectTransform>().anchoredPosition = new Vector2(minionPositions[minionsOnBoard.Count][k], 0);
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            insideArea = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            insideArea = false;
        }

        //-------------------------------------------------------

        public bool IsFull()
        {
            return minionsOnBoard.Count >= maxMinions;
        }

        public bool IsInsideArea()
        {
            return insideArea;
        }
    }
}