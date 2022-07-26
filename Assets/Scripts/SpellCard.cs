using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace battler
{
    public class SpellCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image sprite;

        private Spell spell;

        public void Init(Spell spellBase)
        {
            spell = spellBase;

            setName(spell.GetName());
            setDescription(spell.GetDescription());
            setSprite(spell.GetArt());
        }

        //-----------------------------------------------

        public void setName(string name)
        {
            nameText.text = name;
        }

        public void setDescription(string description)
        {
            descriptionText.text = description;
        }

        public void setSprite(Sprite sprt)
        {
            sprite.sprite = sprt;
        }
    }
}