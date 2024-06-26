using System.Collections;
using System.Collections.Generic;
using Game.Item;
using Game.Settings;
using UnityEngine;

namespace Game
{
    public interface IWeaponController
    {
        void AddWeapon(ItemName weaponType);
        ItemName GetUniqueItem();
    }

    public class WeaponController : IWeaponController
    {
        protected readonly ItemDescriptor _itemDescriptor;

        public WeaponController(ItemDescriptor itemDescriptor)
        {
            _itemDescriptor = itemDescriptor;
        }

        private List<ItemName> _itemList = new();
        public void AddWeapon(ItemName weaponType)
        {
            _itemList.Add(weaponType);
            Debug.Log($"You get new weapon {weaponType}");
        }

        public ItemName GetUniqueItem()
        {
            var items = _itemDescriptor.GetItemsByType(ItemType.Weapon);

            foreach (var item in items)
            {
                if(_itemList.Contains(item.ItemName) == false)
                {
                    return item.ItemName;
                }
            }

            return ItemName.None;
        }
    }
}

