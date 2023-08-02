using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallData
{
    public WallDirection wallDirection;

    public float GetWallAngleY()
    {
        switch (wallDirection)
        {
            case WallDirection.Horizontal:
                return 90;
            case WallDirection.Vertical:
                return 0;
            default:
                return 0;
        }
    }
}
