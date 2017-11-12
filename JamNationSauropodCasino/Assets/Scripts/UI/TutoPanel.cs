using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoPanel : MonoBehaviour
{
    public Image Slide;
    public Sprite[] Slides;

    private void Start()
    {
        StartCoroutine(StartSlideshow());
    }

    private IEnumerator StartSlideshow()
    {
        Slide.sprite = Slides[0];

        yield return new WaitForSeconds(0);

        Slide.sprite = Slides[0];

        yield return new WaitForSeconds(0);

        Slide.sprite = Slides[0];

        yield return new WaitForSeconds(0);

        Slide.sprite = Slides[0];

        yield return new WaitForSeconds(0);

        Slide.sprite = Slides[0];

        yield return new WaitForSeconds(0);

        Slide.sprite = Slides[0];

        yield return new WaitForSeconds(0);

        SceneManager.LoadScene("Main");
    }
}
