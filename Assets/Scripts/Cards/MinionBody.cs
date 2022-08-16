using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace battler
{
    public class MinionBody : MonoBehaviour
    {
        [SerializeField] private TMP_Text atkText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private Image art;

        private Minion baseMinion;

        //------------------------------------------------

        public void Init(Minion minion)
        {
            baseMinion = minion;
            atkText.text = minion.getBaseAtk().ToString();
            hpText.text = minion.getBaseHP().ToString();
            art.sprite = minion.GetArt();
        }

        public void SetHP(int hp)
        {
            hpText.text = hp.ToString();
        }

        public void SetATK(int atk)
        {
            atkText.text = atk.ToString();
        }
    }
}

