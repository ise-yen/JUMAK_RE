using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    GameObject RecipeImg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void CloseRecipeBtn()
    {
        RecipeImg.SetActive(false);
    }

    public void OpenRecipeBtn()
    {
        RecipeImg.SetActive(true);
    }
}
