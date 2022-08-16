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
        [SerializeField] private float minionSeparationAnimTime = 0.5f;

        private Minion baseMinion;

        //LERP Animation variables
        private bool moving = false;
        private Vector2 endPosition;
        private Vector2 startPosition;
        private float elapsedTime = 0f;
        private RectTransform rectTransform;

        //------------------------------------------------

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            //Apply LERP animation
            if (moving)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / minionSeparationAnimTime;
                Debug.Log(percentageComplete);
                rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));

                if (percentageComplete >= 1f)
                {
                    elapsedTime = 0f;
                    moving = false;
                }
            }
        }

        public void Init(Minion minion)
        {
            baseMinion = minion;
            atkText.text = minion.getBaseAtk().ToString();
            hpText.text = minion.getBaseHP().ToString();
            art.sprite = minion.GetArt();
        }

        public void AnimatedTranslation(Vector2 position)
        {
            moving = true;
            startPosition = rectTransform.anchoredPosition;
            endPosition = position;
        }

        //---------------------------------------------------------

        public void SetHP(int hp)
        {
            hpText.text = hp.ToString();
        }

        public void SetATK(int atk)
        {
            atkText.text = atk.ToString();
        }

        public bool IsMoving()
        {
            return moving;
        }

        public void SetAnimatedTranslationDestination(Vector2 position)
        {
            if(moving) endPosition = position;
        }
    }
}

