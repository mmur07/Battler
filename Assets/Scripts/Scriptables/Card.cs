using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battler{
    public abstract class Card : ScriptableObject
    {
        [SerializeField] protected int manaCost;
        [SerializeField] protected string description;
        [SerializeField] protected string name;
        //public Image Art;

        public abstract void Play();

        //-------------------------------------------------
        public int GetManaCost() {return manaCost;}

        public string GetDescription() {return description;}

        public string GetName() {return name;}  
    } 
}