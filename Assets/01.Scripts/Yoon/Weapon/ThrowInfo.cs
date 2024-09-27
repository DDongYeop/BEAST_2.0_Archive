using System.Runtime.CompilerServices;
using UnityEngine;

public struct ThrowInfo
{
    public float throwForce;

    // click pos
    // public Vector3 startPosition;
    // public Vector3 currentPosition;

    public Vector2 DragDirection { get; set; }

    public Vector2 ReverseDragDirection
    {
        get
        {
            return -1 * DragDirection;
        }
    }

    public float DragDistance
    {
        get
        {
            return DragDirection.magnitude * 10;
        }
    }

    public Vector2 Force
    {
        get
        {
            return ReverseDragDirection * DragDistance;
        }
    }
}
