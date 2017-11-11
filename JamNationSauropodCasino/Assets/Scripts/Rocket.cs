﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    public Image LaunchSlider;
    public Text KeyDisplay;

    public float LaunchTime;
    public float SelectedLaunchTime;

    public SoundController Sound;

    private float _launchTimer;
    private bool _launched;
    private bool _isSelected;

    private void Start()
    {
        _launchTimer = LaunchTime;
    }

    private void Update()
    {
        if(!_launched)
        {
            _launchTimer -= Time.deltaTime;
            LaunchSlider.fillAmount = _launchTimer / LaunchTime;

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

    public void DisplayKey(string key)
    {
        KeyDisplay.text = key;
    }

    public void Select()
    {
        LaunchSlider.color = Color.red;
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
        transform.SetParent(MainManager.Instance.RocketContainer.transform.parent);
        MainManager.Instance.RocketContainer.RocketLaunched();
        _launched = true;

        Sound.fuse.Stop();
        Sound.thruster.Play();

        // TEMP
        transform.position += new Vector3(0, 5, 0);
        //
    }

    private void Detonate()
    {
        MainManager.Instance.UnbindKey(this);

        Sound.thruster.Stop();
        Sound.detonation.Play();
        Sound.postDetonation.Play();

        //Destroy(gameObject);
    }
}
