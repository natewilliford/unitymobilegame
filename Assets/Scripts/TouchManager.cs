using UnityEngine;
using System;
using System.Collections.Generic;

public class TouchManager : IObservable<TouchGesture> {
   
    private const float DRAG_DISTANCE = 8f;
    private const float TAP_TIME = 0.3f;

    private TouchGesture touchGesture;

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

        if (touchPhase == TouchPhase.Began) {

            touchGesture = new TouchGesture();
            touchGesture.startPosition = pos;
            touchGesture.startTime = DateTime.Now;
            touchGesture.endPosition = pos;
            touchGesture.type = TouchGestureType.Tap;
            //Debug.Log("Setting tap");
            
        } else if (touchPhase == TouchPhase.Moved || touchPhase == TouchPhase.Stationary) {
            touchGesture.endPosition = pos;

            if (touchGesture.type == TouchGestureType.Tap && DateTime.Now.Subtract(touchGesture.startTime).TotalSeconds >= TAP_TIME) {

                touchGesture.type = TouchGestureType.None;
            }
            
            if (Vector2.Distance(touchGesture.startPosition, touchGesture.endPosition) >= DRAG_DISTANCE) {
                touchGesture.type = TouchGestureType.Drag;
                DoObserveStuff(touchGesture);
            }
        } else if (touchPhase == TouchPhase.Ended) {
            //Debug.Log("touch ended");
            touchGesture.endPosition = pos;
            if (touchGesture.type == TouchGestureType.Drag) {
                touchGesture.type = TouchGestureType.DragEnd;
                DoObserveStuff(touchGesture);
            } else if (touchGesture.type == TouchGestureType.Tap) {
                DoObserveStuff(touchGesture);
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
