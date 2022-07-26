using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler{
    public abstract class Card : ScriptableObject
    {
        [SerializeField] protected string description;
        [SerializeField] protected string cardName;
        [SerializeField] protected Sprite art;
        //public Image Art;

        public virtual CardType type
        {
            get {
                return CardType.None;
            }
        }

        public abstract void Play();

        //-------------------------------------------------

        public string GetDescription() {return description;}

        public string GetName() {return cardName;}  

        public Sprite GetArt() {return art;}
    } 
}