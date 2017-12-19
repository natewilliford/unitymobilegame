using UnityEngine;
using System;

public enum TouchState {
    NotStarted,
    None,
    Tapping,
    Dragging,
}

public class TouchTracker {

    
    private const float DRAG_DISTANCE = 8f;
    private const float TAP_TIME = 0.3f;

    public Vector2 originPosition;
    public Vector2 currentPosition;
    private DateTime startTime;

    public TouchState touchState;
    

    public void TrackTouchDown(Vector2 pos, DateTime now) {
        if (touchState == TouchState.NotStarted) {
            originPosition = pos;
            startTime = now;
            //Debug.Log("Setting startTime to: " + now.ToUniversalTime().ToString());
            touchState = TouchState.Tapping;
        }
        currentPosition = pos;

        //Debug.Log("startTime" + startTime.ToUniversalTime().ToString());
        //Debug.Log("now" + now.ToUniversalTime().ToString());
        //Debug.Log("Time interval: " + (now - startTime).TotalSeconds);

        if (touchState == TouchState.Tapping && now.Subtract(startTime).TotalSeconds >= TAP_TIME) {
            touchState = TouchState.None;
        }

        //Debug.Log("dist: " + Vector2.Distance(originPosition, pos).ToString("R"));
        
        if (Vector2.Distance(originPosition, pos) >= DRAG_DISTANCE) {
            touchState = TouchState.Dragging;
        }
    }
}
