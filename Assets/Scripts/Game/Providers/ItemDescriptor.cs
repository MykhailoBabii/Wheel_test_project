using System;
using System.Collections;
using System.Collections.Generic;
using Game.Item;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "ItemDescriptor", menuName = "Create/Item Descriptor")]
    public class ItemDescriptor : ScriptableObject
    {
        public List<ItemData> ItemDataList => _itemDatas;
        [SerializeField] private List<ItemData> _itemDatas = new();
        [SerializeField] private SpriteAtlas _spriteAtlas;
        private readonly Dictionary<ItemName, ItemData> _itemDict = new();
        private readonly Dictionary<ItemType, List<ItemData>> _itemTypeDataDict = new();

        void OnEnable()
        {
            foreach (var item in _itemDatas)
            {
                _itemDict[item.ItemName] = item;

                if (!_itemTypeDataDict.ContainsKey(item.ItemType))
                {
                    _itemTypeDataDict[item.ItemType] = new List<ItemData>();
                }
                _itemTypeDataDict[item.ItemType].Add(item);

            }
        }

        public ItemData GetItemData(ItemName itemName)
        {
            return _itemDict[itemName];
        }

        public List<ItemData> GetItemsByType(ItemType itemType)
        {
            return _itemTypeDataDict[itemType];
        }

        public Sprite GetItemIcon(ItemName itemName)
        {
            var data = _itemDict[itemName];
            return _spriteAtlas.GetSprite(data.IconName);
        }
    }

    [Serializable]
    public class ItemData
    {
        [field: SerializeField] public ItemName ItemName { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public string IconName { get; private set; }
    }
}