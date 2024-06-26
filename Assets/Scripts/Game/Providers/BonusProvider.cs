using System;
using System.Collections;
using System.Collections.Generic;
using Game.Item;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "BonusProvider", menuName = "Create/Bonus Provider")]
    public class BonusProvider : ScriptableObject
    {
        [SerializeField] private List<BonusData> _bonusDatasList;
        
        internal List<BonusData> GetBonuses()
        {
            return _bonusDatasList;
        }
    }

    [Serializable]
    public class BonusData
    {
        [field: SerializeField] public ItemName ItemType { get; private set; }
        [field: SerializeField] public int BonusCount { get; private set; }
        [field: SerializeField, Range(0, 1)] public float Probability { get; private set; }
        [field: SerializeField] public bool IsFirst { get; private set; }

    }

    public class SegmentData
    {
        public ItemName ItemName { get; private set; }
        public int Count { get; private set; }
        public int ListIndex { get; private set; }
        public float Probability { get; private set; }
        public Sprite Sprite { get; private set; }
        

        public SegmentData(ItemName itemName, int count, int listIndex, Sprite sprite, float probability)
        {
            ItemName = itemName;
            Count = count;
            ListIndex = listIndex;
            Sprite = sprite;
            Probability = probability;
        }
    }
}

