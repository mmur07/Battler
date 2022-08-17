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
        [SerializeField] private GameObject minionBodyPrefab;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Hand playerHand;
        [SerializeField] Board board;

        [Header("Testing purposes, please delete me ")]
        public Minion minionToCreate1;
        public Minion minionToCreate2;
        public Spell spellToCreate;

        //------------------------------------------------------

        void Start()
        {
#if UNITY_EDITOR
            if (minionCardPrefab == null || spellCardPrefab == null || canvas == null || playerHand == null || board == null || minionBodyPrefab == null)
            {
                Debug.LogError("BoardManager: Missing reference from editor.");
                return;
            }
#endif
            StartCoroutine(AddInitialCards());
        }

        IEnumerator AddInitialCards() //Testing purposes only. Coroutine because this need to be executed after all scene's Start() methods.
        {
            yield return null; //Skip 1st frame.

            AddCardToHand(minionToCreate1);
            AddCardToHand(spellToCreate);
            AddCardToHand(minionToCreate2);
            AddCardToHand(minionToCreate1);
            AddCardToHand(minionToCreate2);
            AddCardToHand(minionToCreate1);
            AddCardToHand(minionToCreate2);
        }

        public void AddCardToHand(Card card)
        {
            if (playerHand.IsFull())
            {
                GameObject c;
                switch (card.type)
                {
                    case CardType.Minion:
                        c = Instantiate(minionCardPrefab, playerHand.transform);
                        MinionCard newMinionCard = c.GetComponent<MinionCard>();
                        newMinionCard.Init((Minion)card);
                        newMinionCard.SetReferences(canvas, playerHand);
                        playerHand.AddCard(newMinionCard);
                        break;

                    case CardType.Spell:
                        c = Instantiate(spellCardPrefab, playerHand.transform);
                        SpellCard newSpellCard = c.GetComponent<SpellCard>();
                        newSpellCard.Init((Spell)card);
                        newSpellCard.SetReferences(canvas, playerHand);
                        playerHand.AddCard(newSpellCard);
                        break;
                }
            }
        }

        public bool DropCardInField(PlayableCard card)
        {
            if (board.IsInsideArea())
            {
                if (card.GetCardType() == CardType.Minion && !board.IsFull())
                {
                    MinionCard mCard = (MinionCard)card;
                    GameObject m = Instantiate(minionBodyPrefab, board.transform);
                    MinionBody newMinionBody = m.GetComponent<MinionBody>();
                    newMinionBody.Init(mCard.GetCard());
                    board.PlayCard(newMinionBody);

                    Destroy(card.gameObject);
                    return true;
                }
                //Spells are WIP
            }
            return false;
        }
    }
}