using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class RocketInfo : MonoBehaviour
{
    public Image Icon;
    public Image Key;
    public Image Fuse;
    public Slider LaunchSlider;
    
    public float shakeRange = 20f;

    public Sprite ASprite;
    public Sprite BSprite;
    public Sprite XSprite;
    public Sprite YSprite;
    public Sprite R1Sprite;
    public Sprite L1Sprite;
    public Sprite R2Sprite;
    public Sprite L2Sprite;
    public Sprite UpSprite;
    public Sprite DownSprite;
    public Sprite LeftSprite;
    public Sprite RightSprite;

    private Vector3 originalRotation;

    public bool IsShaking;

    private void Start()
    {
        originalRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if(IsShaking)
        {
            float z = Random.value * shakeRange - (shakeRange / 2);
            Icon.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z + z);
            Fuse.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z + z);
            LaunchSlider.transform.eulerAngles = new Vector3(originalRotation.x, originalRotation.y, originalRotation.z + z);
        }
    }

    public void UpdateLaunchSlider(float currentValue, float maxValue)
    {
        LaunchSlider.value = currentValue / maxValue;
        Fuse.fillAmount = currentValue / maxValue;
    }

    public void DisplayKey(Assets.Scripts.Button key)
    {
        switch (key)
        {
            case Assets.Scripts.Button.A:
                Key.sprite = ASprite;
                break;
            case Assets.Scripts.Button.B:
                Key.sprite = BSprite;
                break;
            case Assets.Scripts.Button.X:
                Key.sprite = XSprite;
                break;
            case Assets.Scripts.Button.Y:
                Key.sprite = YSprite;
                break;
            case Assets.Scripts.Button.L1:
                Key.sprite = L1Sprite;
                break;
            case Assets.Scripts.Button.R1:
                Key.sprite = R1Sprite;
                break;
            case Assets.Scripts.Button.R2:
                Key.sprite = R2Sprite;
                break;
            case Assets.Scripts.Button.L2:
                Key.sprite = L2Sprite;
                break;
            case Assets.Scripts.Button.Up:
                Key.sprite = UpSprite;
                break; 
            case Assets.Scripts.Button.Down:
                Key.sprite = DownSprite;
                break;
            case Assets.Scripts.Button.Left:
                Key.sprite = LeftSprite;
                break;
            case Assets.Scripts.Button.Right:
                Key.sprite = RightSprite;
                break;
        }
    }
}
