using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler
{
    [CreateAssetMenu(fileName = "Minion",
    menuName = "Battler/Spell")]
    public class Spell : Card
    {
        public override CardType type
        {
            get
            {
                return CardType.Spell;
            }
        }

        //------------------------------------------

        public override void Play()
        {

        }
    }
}