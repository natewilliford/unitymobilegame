using UnityEngine;
using System;

public enum TouchGestureType {
    None,
    Tap,
    Drag,
    DragEnd,
}

public class TouchGesture {
    public TouchGestureType type;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public DateTime startTime;
}
