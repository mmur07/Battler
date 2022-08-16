using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace battler
{
    public class SpellCard : PlayableCard
    {
        private Spell spell;

        //-----------------------------------------------------

        public void Init(Spell spellBase)
        {
            spell = spellBase;
            cardType = CardType.Spell;

            SetName(spell.GetName());
            SetDescription(spell.GetDescription());
            SetSprite(spell.GetArt());
        }

        public Spell GetCard()
        {
            return spell;
        }
    }
}