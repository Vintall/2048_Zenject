using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum SwipeDirection
{
    Left,
    Up,
    Right,
    Down
}

public class SwipeControll : ITickable, IInitializable
{
    enum SwipeState
    {
        None,
        Touch,
        Swiping
    }
    SwipeState swipeState = SwipeState.None;
    readonly SignalBus signalBus;
    
    float treshold;
    Vector2 startPosition;
    Vector2 endPosition;

    public SwipeControll(SignalBus signalBus) => this.signalBus = signalBus;
    public void Initialize() // Aka Awake
    {
       treshold = Screen.width / 10;
    }
    public void Tick() // Aka Update
    {
        if (Input.touchCount == 0)
        {
            if (swipeState == SwipeState.Swiping)
            {
                Vector2 direction = endPosition - startPosition;

                DefineControl(direction);
            }

            swipeState = SwipeState.None;
            return;
        }

        if (swipeState == SwipeState.None)
        {
            swipeState = SwipeState.Touch;
            startPosition = Input.touches[0].position;
        }
        if (Vector2.Distance(Input.touches[0].position, startPosition) > treshold)
        {
            swipeState = SwipeState.Swiping;
            endPosition = Input.touches[0].position;
        }
    }
    void DefineControl(Vector2 swipeDirection)
    {
        SwipeDirection conclusion;

        if (swipeDirection.x >= swipeDirection.y)
        {
            if (swipeDirection.x >= -swipeDirection.y)
                conclusion = SwipeDirection.Right;
            else
                conclusion = SwipeDirection.Down;
        }
        else
        {
            if (swipeDirection.x >= -swipeDirection.y)
                conclusion = SwipeDirection.Up;
            else
                conclusion = SwipeDirection.Left;
        }

        SendControl(conclusion);
    }
    void SendControl(SwipeDirection swipeDirection)
    {
        signalBus.Fire(new PlayerControlSwapSignal { swipeDirection = swipeDirection });
    }
    
}
