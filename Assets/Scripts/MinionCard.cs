using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace battler
{
    public class MinionCard : PlayableCard
    {
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text atkText;

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
    }
}