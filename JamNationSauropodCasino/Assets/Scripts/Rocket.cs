using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    public float LaunchTime;
    public float SelectedLaunchTime;
    
    public RocketInfo RocketInfo { get; set; }
    public MainManager.RocketColor RocketColor { get; set; }
    public bool IsSelected { get; set; }
    public SoundController Sound;
    public Exploder Exploder;
    public Color[] Color1;
    public Color[] Color2;
    public Sprite[] Icon;

    private float _launchTimer;
    private bool _launched;
    private bool _detonated;

    public int RocketType = -1;

    private void Start()
    {
        _launchTimer = LaunchTime;
        ShowInfo();

        RocketType = Mathf.RoundToInt(Random.value * (Exploder.GetExplodablesCount() - 1));

        Exploder.Select(RocketType);
        
        RocketInfo.RocketBackground.color = Color1[RocketType];
        RocketInfo.RocketForegroung.color = Color2[RocketType];
        RocketInfo.Icon.sprite = Icon[RocketType];
    }

    private void Update()
    {
        if(!_launched && !_detonated)
        {
            _launchTimer -= Time.deltaTime;

            RocketInfo.UpdateLaunchSlider(_launchTimer, LaunchTime);

            if (_launchTimer <= 0)
            {
                if(IsSelected)
                {
                    Launch();
                }
                else
                {
                    DetonateFail();
                }
            }
        }
    }
    
    public void ShowInfo()
    {
        RocketInfo.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void ResetAndHideInfo()
    {
        RocketInfo.LaunchSlider.handleRect.GetComponent<Image>().color = Color.white;
        RocketInfo.GetComponent<CanvasGroup>().alpha = 0;
        RocketInfo.IsShaking = false;
    }

    public void Select()
    {
        IsSelected = true;

        RocketInfo.IsShaking = true;
        RocketInfo.LaunchSlider.handleRect.GetComponent<Image>().color = Color.magenta;
        LaunchTime = SelectedLaunchTime;
        _launchTimer = LaunchTime;

        Sound.fuse.Play();
    }

    public void Unselect()
    {
        Detonate();
    }

    private void Launch()
    {
        if (_detonated) return;

        _launched = true;

        Sound.fuse.Stop();
        //Sound.thruster.Play();

        Exploder.Launch();
    }

    private void Detonate()
    {
        if (!_launched)
        {
            _detonated = true;
            DetonateFail();
            return;
        }

        _detonated = true;

        ResetAndHideInfo();

        MainManager.Instance.UnbindKey(this);

        //Sound.thruster.Stop();

        SpectacularityTracker.Instance.RegisterDetonation(this);

        Exploder.Detonate();

        Destroy(gameObject, 8f);//TODO: call from time transform
    }
    private void DetonateFail()
    {
        Sound.fuse.Stop();

        ResetAndHideInfo();

        MainManager.Instance.UnbindKey(this);

        Destroy(gameObject, 1f);
    }
}
