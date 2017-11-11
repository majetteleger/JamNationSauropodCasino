using Assets.Scripts;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float LaunchTime;
    public float SelectedLaunchTime;
    
    public RocketInfo RocketInfo { get; set; }
    public SoundController Sound;
    public TimeTransform TimeTransform;

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

        TimeTransform.enabled = true;
    }

    private void Detonate()
    {
        _detonated = true;

        ResetAndHideInfo();

        MainManager.Instance.UnbindKey(this);

        Sound.thruster.Stop();
        Sound.detonation.Play();
        Sound.postDetonation.Play();

        SpectacularityTracker.Instance.RegisterDetonation(this);

        TimeTransform.TriggerEvent.Invoke();

        Destroy(gameObject, 4f);//TODO: call from time transform
    }
    private void DetonateFail()
    {
        ResetAndHideInfo();

        MainManager.Instance.UnbindKey(this);

        Destroy(gameObject, 1f);
    }
}
