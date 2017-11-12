using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folk : MonoBehaviour
{
    public Sprite HandsUpSprite;
    public Sprite HandsDownSprite;
    
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HandsUp()
    {
        if(HandsUpSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = HandsUpSprite;
        }
    }

    public void HandsDown()
    {
        if(HandsDownSprite != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = HandsDownSprite;
        }
    }
}
