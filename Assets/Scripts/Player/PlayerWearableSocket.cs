using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerWearableSocket : MonoBehaviour
{
    public WearableItem Item 
    { 
        get
        {
            return _item;
        }
        set
        {
            _item = value;

            if (_item != null)
            {
                _spriteRenderer.sortingOrder = (int)_item.Layer;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private WearableItem _item;
    private SpriteRenderer _spriteRenderer;

    public void Initialize(PlayerAnimator itemAnimator)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        itemAnimator.OnIdleUpFrameEvent += OnIdleUpFrame;
        itemAnimator.OnIdleDownFrameEvent += OnIdleDownFrame;
        itemAnimator.OnIdleLeftFrameEvent += OnIdleLeftFrame;
        itemAnimator.OnIdleRightFrameEvent += OnIdleRightFrame;
        itemAnimator.OnWalkUpFrameEvent += OnWalkUpFrame;
        itemAnimator.OnWalkDownFrameEvent += OnWalkDownFrame;
        itemAnimator.OnWalkLeftFrameEvent += OnWalkLeftFrame;
        itemAnimator.OnWalkRightFrameEvent += OnWalkRightFrame;
    }

    public void OnIdleUpFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.IdleUpSprites[frame];
        _spriteRenderer.flipX = false;
    }

    public void OnIdleDownFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.IdleDownSprites[frame];
        _spriteRenderer.flipX = false;
    }

    public void OnIdleLeftFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.IdleLeftSprites[frame];
        _spriteRenderer.flipX = true;
    }

    public void OnIdleRightFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.IdleRightSprites[frame];
        _spriteRenderer.flipX = false;
    }

    public void OnWalkUpFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.WalkUpSprites[frame];
        _spriteRenderer.flipX = false;
    }

    public void OnWalkDownFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.WalkDownSprites[frame];
        _spriteRenderer.flipX = false;
    }

    public void OnWalkLeftFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.WalkLeftSprites[frame];
        _spriteRenderer.flipX = true;
    }

    public void OnWalkRightFrame(int frame)
    {
        if (Item == null) return;

        _spriteRenderer.sprite = Item.WalkRightSprites[frame];
        _spriteRenderer.flipX = false;
    }
}
