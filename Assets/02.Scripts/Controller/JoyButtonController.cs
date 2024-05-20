using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyButtonController : MonoBehaviour
//, IPointerUpHandler, IPointerDownHandler
{
    //Button joyButton;
    [HideInInspector]
    public bool Pressed;

    void Awake()
    {
    }

    void Update()
    {
        Pressed = false;
    }

    public void OnClickButton()
    {
        Pressed = true;
    }

}
