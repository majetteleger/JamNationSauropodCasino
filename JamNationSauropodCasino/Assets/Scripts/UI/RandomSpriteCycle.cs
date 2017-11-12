using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSpriteCycle : MonoBehaviour
{
    public Sprite[] Sprites;
    public float CycleTime;

    private float _cycleTimer;
    private int _cycleIndex;
    private bool _back;

    private void Start()
    {
        _cycleTimer = CycleTime;
        _cycleIndex = Random.Range(0, Sprites.Length);

        transform.localScale = Vector3.one * Random.Range(1.15f, 1.85f);
        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, Random.Range(0.5f, 1.0f), GetComponent<Image>().color.b, GetComponent<Image>().color.a);

    }

    private void Update()
    {
        _cycleTimer -= Time.deltaTime;
        
        if (_cycleTimer <= 0)
        {
            if (_cycleIndex == 0)
            {
                _back = false;
            }
            else if (_cycleIndex == Sprites.Length - 1)
            {
                _back = true;
            }

            if (_back)
            {
                _cycleIndex--;
            }
            else
            {
                _cycleIndex++;
            }
            
            GetComponent<Image>().sprite = Sprites[_cycleIndex];
            _cycleTimer = CycleTime;
        }
    }
}
