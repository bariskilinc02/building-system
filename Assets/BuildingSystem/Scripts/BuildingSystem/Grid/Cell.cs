using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Item placedItem;

    public bool IsOccupied()
    {
        return placedItem != null;
    }
}
