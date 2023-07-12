using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDescHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Difficulty difficulty;

    public void OnPointerEnter(PointerEventData pointer)
    {
        HoverPanel.instance.ShowPanel(difficulty);
    }

    public void OnPointerExit(PointerEventData pointer)
    {
        HoverPanel.instance.HidePanel();
    }
}
