using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EdgeVectorExtensions
{
    public static VectorEdge Fix(this VectorEdge original)
    {
       
        if ((original.biggerPoint - original.smallerPoint).y < 0 )
        {
            return new VectorEdge(original.biggerPoint, original.smallerPoint);
        }
        else if ((original.biggerPoint - original.smallerPoint).x < 0 )
        {
            return new VectorEdge(original.biggerPoint, original.smallerPoint);
        }
        else
        {
            return original;
        }
        
    }
}
