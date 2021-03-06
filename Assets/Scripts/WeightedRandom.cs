﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedEntry
{
    public WeightedEntry(int _item, int _weight)
    {
        item = _item;
        weight = _weight;
    }

    public int item;
    public int weight;

}
public static class WeightedRandom {

	public static int WeightedSelect(WeightedEntry[] entries)
    {
        int sumWeight = 0;
        for (int i = 0; i < entries.Length; i++)
        {
            sumWeight += entries[i].weight;
        }
        
        int randValue = Random.Range(1, sumWeight + 1);
        for (int i = 0; i < entries.Length; i++)
        {
            randValue -= entries[i].weight;
            if (randValue <= 0)
            {
                return entries[i].item;
            }
        }
        return -1;
    }
}
