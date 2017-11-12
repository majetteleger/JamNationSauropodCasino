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
        _spriteRenderer.sprite = HandsUpSprite;
    }

    public void HandsDown()
    {
        _spriteRenderer.sprite = HandsDownSprite;
    }
}
