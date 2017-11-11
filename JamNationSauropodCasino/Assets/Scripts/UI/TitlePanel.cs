using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePanel : MonoBehaviour
{
    public void UI_LoadGame()
    {
        SceneManager.LoadScene("Main");
    }
}
