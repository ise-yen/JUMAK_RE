using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{
    //public Image backgroundImage;
    public void StartClickBtn()
    {
        SceneManager.LoadScene("05_Game_Main");
    }
}
