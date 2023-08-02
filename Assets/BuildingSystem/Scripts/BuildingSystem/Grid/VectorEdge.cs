using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public record VectorEdge(Vector2Int smallerPoint, Vector2Int biggerPoint)
{
    public Vector2 GetMiddlePoint()
    {
        return Vector2.Lerp(smallerPoint, biggerPoint, 0.5f);
    }
}

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {
        
    }
}
