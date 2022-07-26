using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler{
    [CreateAssetMenu(fileName = "Minion",
        menuName = "Battler/Minion")]
    public class Minion : Card
    {
        [SerializeField] private int baseAtk;
        [SerializeField] private int baseHP;

        private int currentATK;
        private int currentHP;

        public override CardType type
        {
            get
            {
                return CardType.Minion;
            }
        }

        //public Effect DeathRattle;
        //public Effect Battlecry;

        //------------------------------------------

        public override void Play(){

        }

        public void ModifyAttack(int atkModifier){
            currentATK += atkModifier;
            if(currentATK < 0) currentATK = 0;
        }

        public bool OnHit(int damage){
            currentHP -= damage;
            if(currentHP <= 0) {
                currentHP = 0;
                return true;
            }
            return false;
        }

        //------------------------------------------
        public int getBaseHP() {return baseHP;}

        public int getBaseAtk() {return baseAtk;}

        public int getCurrentHP() {return currentHP;}

        public int getCurrentATK() {return currentATK;}

        public void SetHP(int hp){
            currentHP = hp;
        }

        public void SetAttack(int atk){
            currentATK = atk;
        }
    }
}