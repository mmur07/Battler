using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace battler
{
    public class MinionCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text atkText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image sprite;

        private Minion minion;

        //----------------------------------------------

        public void Init(Minion minionBase)
        {
            minion = minionBase;

            setHP(minion.getBaseHP());
            setATK(minion.getBaseAtk());
            setName(minion.GetName());
            setDescription(minion.GetDescription());
            setSprite(minion.GetArt());
        }

        //-----------------------------------------------

        public void setHP(int hp)
        {
            hpText.text = hp.ToString();
        }

        public void setATK(int atk)
        {
            atkText.text = atk.ToString();
        }

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