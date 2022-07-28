using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace battler
{
    public class PlayableCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image sprite;

        private bool pointerHover = false;

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

        private void Update()
        {
            if (pointerHover) sprite.enabled = false;
            else sprite.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Sali");
            pointerHover = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Entre");
            pointerHover = true;
        }
    }
}
