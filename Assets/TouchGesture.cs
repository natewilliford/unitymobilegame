using UnityEngine;
using System.Collections;

public enum TouchGestureType {
    Tap,
    Drag,
    DragEnd,
}

public class TouchGesture {
    public TouchGestureType type;
    public Vector2 startPosition;
    public Vector2 endPosition;
}
