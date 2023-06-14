using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class SwipeControl : ISwipeControl
{
    
    SwipeState swipeState = SwipeState.None;
    readonly SignalBus signalBus;
    
    float treshold;
    Vector2 startPosition;
    Vector2 endPosition;

    public SwipeControl(SignalBus signalBus) => this.signalBus = signalBus;
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

                SendControl(DefineControl(direction));
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
    SwipeDirection DefineControl(Vector2 swipeDirection)
    {
        if (swipeDirection.x >= swipeDirection.y)
        {
            if (swipeDirection.x >= -swipeDirection.y)
                return SwipeDirection.Right;
            else
                return SwipeDirection.Down;
        }
        else
        {
            if (swipeDirection.x >= -swipeDirection.y)
                return SwipeDirection.Up;
            else
                return SwipeDirection.Left;
        }
    }
    void SendControl(SwipeDirection swipeDirection) => 
        signalBus.Fire(new PlayerControlSwapSignal { swipeDirection = swipeDirection });
    
}
