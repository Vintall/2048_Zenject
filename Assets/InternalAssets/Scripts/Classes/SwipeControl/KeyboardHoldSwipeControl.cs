using UnityEngine;
using Zenject;

public class KeyboardHoldSwipeControl : ISwipeControl
{
    SignalBus onSwipe;
    public void Initialize()
    {
    }
    public void Tick()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            SendControll(SwipeDirection.Left);
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            SendControll(SwipeDirection.Up);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            SendControll(SwipeDirection.Right);
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            SendControll(SwipeDirection.Down);
    }

    public KeyboardHoldSwipeControl(SignalBus signalBus)
    {
        this.onSwipe = signalBus;
    }
    void SendControll(SwipeDirection swipeDirection)
    {
        onSwipe.Fire(new PlayerControlSwapSignal { swipeDirection = swipeDirection });
    }
}
