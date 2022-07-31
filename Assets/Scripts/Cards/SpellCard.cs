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

        public void Init(Spell spellBase)
        {
            spell = spellBase;

            setName(spell.GetName());
            setDescription(spell.GetDescription());
            setSprite(spell.GetArt());
        }
    }
}