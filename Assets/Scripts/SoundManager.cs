using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{

    public AudioSource myFx;
   
    public AudioClip clickFx;

 

    public void ClickSound()
    {
        myFx.PlayOneShot(clickFx);
    }
}


