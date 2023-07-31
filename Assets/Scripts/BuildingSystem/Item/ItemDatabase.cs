using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : SingletonBehaviour<ItemDatabase>
{
    private Dictionary<string, ItemData> itemDatabase;

    protected override void Awake()
    {
        base.Awake();
        itemDatabase = new Dictionary<string, ItemData>();

        foreach (var itemData in  Resources.LoadAll<ItemData>("Item Database"))
        {
            itemDatabase.Add(itemData.id, itemData);
        }
 
    }
    
    public ItemData CreateItemInstance(string id)
    {
        ItemData itemData = UnityEngine.Object.Instantiate(itemDatabase[id]);
        return itemData;
    }
}
