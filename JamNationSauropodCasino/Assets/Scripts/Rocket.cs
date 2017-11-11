using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    public Image LaunchSlider;
    public Text KeyDisplay;

    public float LaunchTime;

    private float _launchTimer;
    private bool _launched;

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
                Launch();
            }
        }
    }

    public void DisplayKey(char key)
    {
        KeyDisplay.text = key.ToString();
    }

    private void Launch()
    {
        transform.SetParent(MainManager.Instance.RocketContainer.transform.parent);
        MainManager.Instance.RocketContainer.RocketLaunched();
        _launched = true;

        // TEMP
        transform.position += new Vector3(0, 5, 0);
        Invoke("Detonate", 2f); 
        //
    }

    private void Detonate()
    {
        MainManager.Instance.UnbindKey(this);
        Destroy(gameObject);
    }
}
