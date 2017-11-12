using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsPanel : MonoBehaviour
{
    public Image RainbowBoom;
    public Image TimedBlast;
    public Image UnityBoom;
    public float FirstScaleTime;
    public float FirstScale;
    public float PauseTime;
    public float SecondScaleTime;
    public float SecondScale;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            AnimateRainbowBoom();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            AnimateTimedBlast();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            AnimateUnityBoom();
        }
    }

    public void AnimateRainbowBoom()
    {
        StartCoroutine(AnimateWords(RainbowBoom, FirstScaleTime));
    }

    public void AnimateTimedBlast()
    {
        StartCoroutine(AnimateWords(TimedBlast, FirstScaleTime));
    }

    public void AnimateUnityBoom()
    {
        StartCoroutine(AnimateWords(UnityBoom, FirstScaleTime));
    }

    private IEnumerator AnimateWords(Image words, float time)
    {
        float elapsedTime = 0;
        float alpha = 0;
        float zPos = 0;
        
        while (elapsedTime < time)
        {
            alpha = Mathf.Lerp(alpha, 1, (elapsedTime / time));
            zPos = Mathf.Lerp(zPos, FirstScale, (elapsedTime / time));
            words.color = new Color(1, 1, 1, alpha);
            words.transform.localPosition = new Vector3(0, 0, -zPos);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(PauseTime);

        elapsedTime = 0;
        alpha = 1;
        zPos = FirstScale;

        while (elapsedTime < time)
        {
            alpha = Mathf.Lerp(alpha, 0, (elapsedTime / time));
            zPos = Mathf.Lerp(zPos, SecondScale, (elapsedTime / time));
            words.color = new Color(1, 1, 1, alpha);
            words.transform.localPosition = new Vector3(0, 0, -zPos);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        words.transform.localPosition = Vector3.zero;
    }
}
