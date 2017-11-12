using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Image Words;
    public Sprite Lame;
    public Sprite SoMuchBoom;
    public Sprite DoYouEven;

    private void Start()
    {
        ShowBoom();
    }

    private void Update()
    {
        if(Input.GetButtonDown("BackButton_All"))
        {
            SceneManager.LoadScene("Title");
        }
        if (Input.GetButtonDown("StartButton_All"))
        {
            SceneManager.LoadScene("Main");
        }
    }
    
    public void ShowLame()
    {
        Words.sprite = Lame;
    }

    public void ShowBoom()
    {
        Words.sprite = SoMuchBoom;
    }

    public void ShowEven()
    {
        Words.sprite = DoYouEven;
    }
}
