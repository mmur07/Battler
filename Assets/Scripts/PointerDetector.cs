using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace battler
{
    public class PointerDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool insideArea = false;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            insideArea = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            insideArea = false;
        }

        public bool IsInsideArea()
        {
            return insideArea;
        }
    }
}
