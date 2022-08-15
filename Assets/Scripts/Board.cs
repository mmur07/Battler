using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class Board : PointerDetector
    {
        [SerializeField] int maxMinions = 6;
        [SerializeField] float minionOffset = 20f;

        private List<PlayableCard> minionsOnBoard = new List<PlayableCard>();

        private RectTransform rect = null;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        public bool PlayCard(PlayableCard minion)
        {
            if(IsInsideArea() && minionsOnBoard.Count < maxMinions)
            {
                minionsOnBoard.Add(minion);
                //Tranfer parentship to board.
                minion.transform.SetParent(transform);
                minion.GetComponent<RectTransform>().rotation = Quaternion.identity;
                RecalculateMinionPositions();
                return true;
            }
            return false;
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