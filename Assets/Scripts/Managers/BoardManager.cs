using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace battler
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private GameObject minionCardPrefab;
        [SerializeField] private GameObject spellCardPrefab;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Hand playerHand;
        [SerializeField] Board board;

        public Minion minionToCreate;
        public Spell spellToCreate;

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            if (minionCardPrefab == null || spellCardPrefab == null || canvas == null || playerHand == null || board == null)
            {
                Debug.LogError("BoardManager: Missing reference from editor.");
                return;
            }
#endif

            AddCardToHand(minionToCreate);
            AddCardToHand(spellToCreate);
            AddCardToHand(minionToCreate);
            AddCardToHand(minionToCreate);
            AddCardToHand(minionToCreate);
            AddCardToHand(minionToCreate);
            AddCardToHand(minionToCreate);
        }

        public void AddCardToHand(Card card)
        {
            GameObject c;
            switch (card.type)
            {
                case CardType.Minion:
                    c = Instantiate(minionCardPrefab, playerHand.transform);
                    MinionCard newMinionCard = c.GetComponent<MinionCard>();
                    newMinionCard.Init(minionToCreate);
                    newMinionCard.SetReferences(canvas, playerHand);
                    playerHand.AddCard(newMinionCard);
                    break;

                case CardType.Spell:
                    c = Instantiate(spellCardPrefab, playerHand.transform);
                    SpellCard newSpellCard = c.GetComponent<SpellCard>();
                    newSpellCard.Init(spellToCreate);
                    newSpellCard.SetReferences(canvas, playerHand);
                    playerHand.AddCard(newSpellCard);
                    break;
            }
        }

        public bool DropCardInField(PlayableCard card)
        {
            if (board.PlayCard(card))
            {
                return true;
            }
            return false;
        }
    }
}