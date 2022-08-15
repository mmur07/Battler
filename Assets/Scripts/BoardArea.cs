using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isInsideArea = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInsideArea = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInsideArea = false;
    }

    //-------------------------------------------

    public bool IsInsideArea()
    {
        return isInsideArea;
    }
}
