using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace battler
{
    public class PlayableCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image sprite;

        //----------------------------------------
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
