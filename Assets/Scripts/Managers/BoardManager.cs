using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private GameObject minionCardPrefab;
        [SerializeField] private GameObject spellCardPrefab;
        [SerializeField] private Transform canvas;
        [SerializeField] private Hand playerHand;

        public Minion minionToCreate;
        public Spell spellToCreate;

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            if(minionCardPrefab == null || spellCardPrefab == null || canvas == null || playerHand == null)
            {
                Debug.LogError("BoardManager: Missing reference from editor.");
                return;
            }
#endif

            AddCardToHand(minionToCreate);
            AddCardToHand(spellToCreate);
            AddCardToHand(minionToCreate);
            //AddCardToHand(minionToCreate);
            //AddCardToHand(minionToCreate);
            //AddCardToHand(minionToCreate);
            //AddCardToHand(minionToCreate);
        }

        public void AddCardToHand(Card card)
        {
            GameObject c;
            switch (card.type)
            {
                case CardType.Minion:
                    c = Instantiate(minionCardPrefab, playerHand.transform);
                    MinionCard newMinion = c.GetComponent<MinionCard>();
                    newMinion.Init(minionToCreate);
                    playerHand.AddCard(newMinion);
                    break;

                case CardType.Spell:
                    c = Instantiate(spellCardPrefab, playerHand.transform);
                    SpellCard newSpell = c.GetComponent<SpellCard>();
                    newSpell.Init(spellToCreate);
                    playerHand.AddCard(newSpell);
                    break;
            }
        }
    }
}