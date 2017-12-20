using UnityEngine;
using System;
using System.Collections.Generic;

public enum TouchState {
    None,
    Tapping,
    Dragging,
}

public class TouchManager : IObservable<TouchGesture> {

    
    private const float DRAG_DISTANCE = 8f;
    private const float TAP_TIME = 0.3f;

    public Vector2 originPosition;
    public Vector2 currentPosition;
    private DateTime startTime;

    public TouchState touchState;

    private List<IObserver<TouchGesture>> observers = new List<IObserver<TouchGesture>>();


    public void UpdateTouches() {
        foreach(Touch touch in Input.touches) {
            HandleTouch(touch.fingerId, touch.position, touch.phase);
        }

        if (Input.touchCount == 0) {
            if (Input.GetMouseButtonDown(0)) {
                HandleTouch(0, Input.mousePosition, TouchPhase.Began);
            } else if (Input.GetMouseButton(0)) {
                HandleTouch(0, Input.mousePosition, TouchPhase.Moved);
            } else if (Input.GetMouseButtonUp(0)) {
                HandleTouch(0, Input.mousePosition, TouchPhase.Ended);
            }
        }
    }

    private void HandleTouch(int touchFignerId, Vector2 pos, TouchPhase touchPhase) {
        // TODO: Track multiple fingers.
        if (touchFignerId != 0) {
            return;
        }

        TouchGesture gesture = new TouchGesture();
        gesture.startPosition = originPosition;
        gesture.endPosition = currentPosition;

        currentPosition = pos;
        if (touchPhase == TouchPhase.Began) {
            originPosition = pos;
            startTime = DateTime.Now;
            //Debug.Log("Setting startTime to: " + now.ToUniversalTime().ToString());
            touchState = TouchState.Tapping;
        } else if (touchPhase == TouchPhase.Moved || touchPhase == TouchPhase.Stationary) {
            //Debug.Log("startTime" + startTime.ToUniversalTime().ToString());
            //Debug.Log("now" + now.ToUniversalTime().ToString());
            //Debug.Log("Time interval: " + (now - startTime).TotalSeconds);

            if (touchState == TouchState.Tapping && DateTime.Now.Subtract(startTime).TotalSeconds >= TAP_TIME) {
                touchState = TouchState.None;
            }

            //Debug.Log("dist: " + Vector2.Distance(originPosition, pos).ToString("R"));

            if (Vector2.Distance(originPosition, pos) >= DRAG_DISTANCE) {
                touchState = TouchState.Dragging;
                gesture.type = TouchGestureType.Drag;
                DoObserveStuff(gesture);
            }
        } else if (touchPhase == TouchPhase.Ended) {
            if (touchState == TouchState.Dragging) {
                gesture.type = TouchGestureType.DragEnd;
                DoObserveStuff(gesture);
            } else if (touchState == TouchState.Tapping) {
                gesture.type = TouchGestureType.Tap;
                DoObserveStuff(gesture);
            }
        }
    }
    

    private void DoObserveStuff(TouchGesture gesture) {
        foreach(IObserver<TouchGesture> observer in observers) {
            observer.OnNext(gesture);
        }
    }

    public void Subscribe(IObserver<TouchGesture> observer) {
        observers.Add(observer);
    }
}
