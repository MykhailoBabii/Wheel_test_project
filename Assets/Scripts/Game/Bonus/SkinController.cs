using System.Collections;
using System.Collections.Generic;
using Game.Item;
using Game.Settings;
using UnityEngine;

namespace Game
{
    public interface ISkinController
    {
        void AddSkin(ItemName itemType);
        ItemName GetUniqueItem();
    }

    public class SkinController : ISkinController
    {
        protected readonly ItemDescriptor _itemDescriptor;

        public SkinController(ItemDescriptor itemDescriptor)
        {
            _itemDescriptor = itemDescriptor;
        }

        private List<ItemName> _itemList = new();
        public void AddSkin(ItemName itemType)
        {
            _itemList.Add(itemType);
            Debug.Log($"You get new skin {itemType}");
        }

        public ItemName GetUniqueItem()
        {
            var items = _itemDescriptor.GetItemsByType(ItemType.Skin);

            foreach (var item in items)
            {
                if (_itemList.Contains(item.ItemName) == false)
                {
                    return item.ItemName;
                }
            }

            return ItemName.None;
        }
    }
}


