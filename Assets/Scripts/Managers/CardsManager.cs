using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    public class CardsManager : MonoBehaviour
    {
        [SerializeField] private GameObject minionCardPrefab;
        [SerializeField] private GameObject spellCardPrefab;
        [SerializeField] private Transform canvas;

        public Minion minionToCreate;
        public Spell spellToCreate;

        // Start is called before the first frame update
        void Start()
        {
            AddCardToHand(minionToCreate);
            AddCardToHand(spellToCreate);
        }

        public void AddCardToHand(Card card)
        {
            GameObject c;
            switch (card.type)
            {
                case CardType.Minion:
                    c = Instantiate(minionCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas);
                    MinionCard newMinion = c.GetComponent<MinionCard>();
                    newMinion.Init(minionToCreate);
                    break;

                case CardType.Spell:
                    c = Instantiate(spellCardPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas);
                    SpellCard newSpell = c.GetComponent<SpellCard>();
                    newSpell.Init(spellToCreate);
                    break;
            }
        }
    }
}