using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class Board : PointerDetector
    {
        [SerializeField] int maxMinions = 6;
        [SerializeField] float minionOffset = 20f;

        private List<MinionBody> minionsOnBoard = new List<MinionBody>();

        private RectTransform rect = null;

        //------------------------------------------------------

        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        public bool IsFull()
        {
            return minionsOnBoard.Count >= maxMinions;
        }

        public void PlayCard(MinionBody minion)
        {
            minionsOnBoard.Add(minion);
            //Tranfer parentship from hand to board.
            RecalculateMinionPositions();
        }

        private void RecalculateMinionPositions()
        {
            //Calculate new starting position for placing cards in board
            float startingPos;
            if (minionsOnBoard.Count % 2 == 0) startingPos = -minionOffset * 0.5f - minionOffset * (Mathf.Max(0, (int)(minionsOnBoard.Count * 0.5f - 1)));
            else startingPos = -minionOffset * Mathf.Max(0, (int)(minionsOnBoard.Count * 0.5f));
            for(int k = 0; k < minionsOnBoard.Count; k++)
            {
                minionsOnBoard[k].GetComponent<RectTransform>().anchoredPosition = new Vector2(startingPos + k * minionOffset, 0);
            }
        }
    }
}