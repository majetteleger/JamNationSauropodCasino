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
    public SoundController Sound;
    public Exploder Exploder;

    private float _launchTimer;
    private bool _launched;
    private bool _detonated;
    private bool _isSelected;

    private void Start()
    {
        _launchTimer = LaunchTime;
        ShowInfo();

        RocketColor = (MainManager.RocketColor)Random.Range(0, (int)MainManager.RocketColor.COUNT);

        switch (RocketColor)
        {
            case MainManager.RocketColor.Red:
                RocketInfo.KeyBackground.color = Color.red;
                break;
            case MainManager.RocketColor.Green:
                RocketInfo.KeyBackground.color = Color.green;
                break;
            case MainManager.RocketColor.Blue:
                RocketInfo.KeyBackground.color = Color.blue;
                break;
            case MainManager.RocketColor.Yellow:
                RocketInfo.KeyBackground.color = Color.yellow;
                break;
            case MainManager.RocketColor.Cyan:
                RocketInfo.KeyBackground.color = Color.cyan;
                break;
            case MainManager.RocketColor.Magenta:
                RocketInfo.KeyBackground.color = Color.magenta;
                break;
        }
    }

    private void Update()
    {
        if(!_launched && !_detonated)
        {
            _launchTimer -= Time.deltaTime;

            RocketInfo.UpdateLaunchSlider(_launchTimer, LaunchTime);

            if (_launchTimer <= 0)
            {
                if(_isSelected)
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
    }

    public void Select()
    {
        RocketInfo.LaunchSlider.handleRect.GetComponent<Image>().color = Color.magenta;
        LaunchTime = SelectedLaunchTime;
        _launchTimer = LaunchTime;

        Sound.fuse.Play();

        _isSelected = true;

        Exploder.Select(Mathf.RoundToInt(Random.value * 1f));//do it
    }

    public void Unselect()
    {
        Detonate();
    }

    private void Launch()
    {
        _launched = true;

        Sound.fuse.Stop();
        Sound.thruster.Play();

        Exploder.Launch();
    }

    private void Detonate()
    {
        _detonated = true;

        ResetAndHideInfo();

        MainManager.Instance.UnbindKey(this);

        Sound.thruster.Stop();

        SpectacularityTracker.Instance.RegisterDetonation(this);

        Exploder.Detonate();

        Destroy(gameObject, 8f);//TODO: call from time transform
    }
    private void DetonateFail()
    {
        ResetAndHideInfo();

        MainManager.Instance.UnbindKey(this);

        Destroy(gameObject, 1f);
    }
}
