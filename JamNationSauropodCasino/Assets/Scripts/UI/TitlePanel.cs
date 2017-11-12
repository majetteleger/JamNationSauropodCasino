using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePanel : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("StartButton_All"))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
