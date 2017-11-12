using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoPanel : MonoBehaviour
{
    public Image Slide;
    public Sprite[] Slides;

    public float wait;

    private void Start()
    {
        StartCoroutine(StartSlideshow());
    }

    private IEnumerator StartSlideshow()
    {
        Slide.sprite = Slides[0];
        yield return new WaitForSeconds(wait * 4);
        Slide.sprite = Slides[1];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[2];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[3];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[4];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[5];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[6];
        yield return new WaitForSeconds(wait * 4);
        Slide.sprite = Slides[7];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[8];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[9];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[10];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[11];
        yield return new WaitForSeconds(wait * 4);
        Slide.sprite = Slides[12];
        yield return new WaitForSeconds(wait);
        Slide.sprite = Slides[13];
        yield return new WaitForSeconds(wait * 4);

        SceneManager.LoadScene("Main");
    }
}
