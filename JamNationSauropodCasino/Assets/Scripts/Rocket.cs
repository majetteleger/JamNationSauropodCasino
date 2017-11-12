using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float LaunchTime;
    public float SelectedLaunchTime;
    
    public RocketInfo RocketInfo { get; set; }
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
        RocketInfo.LaunchSlider.color = Color.white;
        RocketInfo.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Select()
    {
        RocketInfo.LaunchSlider.color = Color.red;
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
