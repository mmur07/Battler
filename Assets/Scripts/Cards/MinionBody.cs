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
        private RectTransform rectTransform;
        private bool moving = false;
        private Coroutine translateCoroutine = null;

        //------------------------------------------------

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private IEnumerator TranslateToEndPosition(Vector2 endPosition)
        {
            float percentageComplete = 0f;
            float elapsedTime = 0f;
            moving = true;

            Vector2 startPosition = rectTransform.anchoredPosition;

            while(percentageComplete < 1f)
            {
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / minionSeparationAnimTime;
                rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));

                yield return null; //Wait for next frame
            }

            moving = false;
            translateCoroutine = null;
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
            if (IsMoving()) //If there's already a coroutine active, kill it and start a new animation from the current position.
            {
                StopCoroutine(translateCoroutine);
            }

            translateCoroutine = StartCoroutine(TranslateToEndPosition(position));
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
    }
}

