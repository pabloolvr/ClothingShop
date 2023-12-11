using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class deals with all player animations. It sends events whenever a new frame is played.
/// This is a workaround to avoid having to create new animator/animations for each wearable,
/// </summary>
public class PlayerAnimator : MonoBehaviour
{
    public int WalkUpAnimId { get; private set; }
    public int WalkDownAnimId { get; private set; }
    public int WalkLeftAnimId { get; private set; }
    public int WalkRightAnimId { get; private set; }
    public int IdleUpAnimId { get; private set; }
    public int IdleDownAnimId { get; private set; }
    public int IdleLeftAnimId { get; private set; }
    public int IdleRightAnimId { get; private set; }

    public event Action OnAnimatorReset = delegate { };

    public event Action<int> OnIdleUpFrameEvent = delegate { };
    public event Action<int> OnIdleDownFrameEvent = delegate { };
    public event Action<int> OnIdleLeftFrameEvent = delegate { };
    public event Action<int> OnIdleRightFrameEvent = delegate { };
    public event Action<int> OnWalkUpFrameEvent = delegate { };
    public event Action<int> OnWalkDownFrameEvent = delegate { };
    public event Action<int> OnWalkLeftFrameEvent = delegate { };
    public event Action<int> OnWalkRightFrameEvent = delegate { };

    private Animator _bodyAnimator;
    private int _curWalkAnimId = -1;
    private int _curIdleAnimId = -1;

    public void Initialize(PlayerWearableSocket[] wearableSockets)
    {
        _bodyAnimator = GetComponent<Animator>();

        WalkUpAnimId = Animator.StringToHash("WalkUp");
        WalkDownAnimId = Animator.StringToHash("WalkDown");
        WalkLeftAnimId = Animator.StringToHash("WalkLeft");
        WalkRightAnimId = Animator.StringToHash("WalkRight");
        IdleUpAnimId = Animator.StringToHash("IdleUp");
        IdleDownAnimId = Animator.StringToHash("IdleDown");
        IdleLeftAnimId = Animator.StringToHash("IdleLeft");
        IdleRightAnimId = Animator.StringToHash("IdleRight");

        foreach (PlayerWearableSocket wearableSocket in wearableSockets)
        {
            wearableSocket.Initialize(this);
        }
    }

    private void Start()
    {
        ResetAnimator();
    }

    public void ResetAnimator()
    {
        if (_curIdleAnimId != -1)
        {
            _bodyAnimator.SetBool(_curIdleAnimId, false);
        }

        if (_curWalkAnimId != -1)
        {
            _bodyAnimator.SetBool(_curWalkAnimId, false);
            _curWalkAnimId = -1;
        }

        _bodyAnimator.SetBool(IdleDownAnimId, true);
        _curIdleAnimId = IdleDownAnimId;
        _bodyAnimator.Play(IdleDownAnimId, 0, 0);
        OnAnimatorReset();
    }

    public void PlayWalkAnim(int walkAnim)
    {
        if (_curWalkAnimId == walkAnim) return;

        StopWalkAnim();
        _bodyAnimator.SetBool(walkAnim, true);
        _curWalkAnimId = walkAnim;

        if (_curIdleAnimId != -1)
        {
            _bodyAnimator.SetBool(_curIdleAnimId, false);
            _curIdleAnimId = -1;
        }
    }

    public void StopWalkAnim()
    {
        if (_curWalkAnimId == -1) return;
        if (_curIdleAnimId != -1) return;

        _bodyAnimator.SetBool(_curWalkAnimId, false);

        if (_curWalkAnimId == WalkUpAnimId)
        {
            _bodyAnimator.SetBool(IdleUpAnimId, true);
            _curIdleAnimId = IdleUpAnimId;
        }
        else if (_curWalkAnimId == WalkDownAnimId)
        {
            _bodyAnimator.SetBool(IdleDownAnimId, true);
            _curIdleAnimId = IdleDownAnimId;
        }
        else if (_curWalkAnimId == WalkLeftAnimId)
        {
            _bodyAnimator.SetBool(IdleLeftAnimId, true);
            _curIdleAnimId = IdleLeftAnimId;
        }
        else if (_curWalkAnimId == WalkRightAnimId)
        {
            _bodyAnimator.SetBool(IdleRightAnimId, true);
            _curIdleAnimId = IdleRightAnimId;
        }
      
        _curWalkAnimId = -1;
    }

    public void OnIdleUpFrame(int frame)
    {
        OnIdleUpFrameEvent(frame);
    }

    public void OnIdleDownFrame(int frame)
    {
        OnIdleDownFrameEvent(frame);
    }

    public void OnIdleLeftFrame(int frame)
    {
        OnIdleLeftFrameEvent(frame);
    }

    public void OnIdleRightFrame(int frame)
    {
        OnIdleRightFrameEvent(frame);
    }

    public void OnWalkUpFrame(int frame)
    {
        OnWalkUpFrameEvent(frame);
    }

    public void OnWalkDownFrame(int frame)
    {
        OnWalkDownFrameEvent(frame);
    }

    public void OnWalkLeftFrame(int frame)
    {
        OnWalkLeftFrameEvent(frame);
    }

    public void OnWalkRightFrame(int frame)
    {
        OnWalkRightFrameEvent(frame);
    }
}
