using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandle : MonoBehaviour
{
    public Sprite Selected;
    public Sprite Unselected;
    public Image image;
    public string action;
    
    public void Select()
    {
        image.sprite = Selected;
    }
    
    public void Unselect()
    {
        image.sprite = Unselected;
    }
    
}
