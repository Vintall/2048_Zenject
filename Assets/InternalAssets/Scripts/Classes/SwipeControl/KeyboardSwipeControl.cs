using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class KeyboardSwipeControl : ISwipeControl
{
    SignalBus onSwipe;
    public void Initialize()
    {
    }
    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            SendControll(SwipeDirection.Left);
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            SendControll(SwipeDirection.Up);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            SendControll(SwipeDirection.Right);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            SendControll(SwipeDirection.Down);
    }
    
    public KeyboardSwipeControl(SignalBus signalBus)
    {
        this.onSwipe = signalBus;
    }
    void SendControll(SwipeDirection swipeDirection)
    {
        onSwipe.Fire(new PlayerControlSwapSignal { swipeDirection = swipeDirection });
    }
}
