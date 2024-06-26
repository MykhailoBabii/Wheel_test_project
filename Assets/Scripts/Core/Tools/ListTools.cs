using System.Collections.Generic;
using System.Linq;
using System;

public static class ListTools
{
    public static List<T> GetShuffleList<T>(List<T> inputList)
    {
        Random random = new();
        var shuffleList = inputList.OrderBy(value => random.Next()).ToList();
        return shuffleList;
    }

    public static T GetRandom<T>(List<T> list)
    {
        var randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }
}
