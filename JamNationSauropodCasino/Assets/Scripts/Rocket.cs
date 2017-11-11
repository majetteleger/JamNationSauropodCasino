using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    public float LaunchTime;
    public float SelectedLaunchTime;

    public RocketInfo RocketInfo { get; set; }

    private float _launchTimer;
    private bool _launched;
    private bool _isSelected;

    private void Start()
    {
        _launchTimer = LaunchTime;
        ShowInfo();
    }

    private void Update()
    {
        if(!_launched)
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
                    Detonate();
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

        _isSelected = true;
    }

    public void Unselect()
    {
        Detonate();
    }

    private void Launch()
    {
        _launched = true;

        // TEMP
        transform.position += new Vector3(0, 5, 0);
        //
    }

    private void Detonate()
    {
        ResetAndHideInfo();

        MainManager.Instance.UnbindKey(this);
        Destroy(gameObject);
    }
}
