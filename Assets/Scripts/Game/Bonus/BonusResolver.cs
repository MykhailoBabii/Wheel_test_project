using System;
using Core;
using Game.Item;
using Game.Settings;

namespace Game.Wheel
{
    public interface IBonusResolver
    {
        void Resolve(ItemName ItemType, int count);
    }

    public class BonusResolver : IBonusResolver
    {
        protected readonly BonusProvider _bonusProvider;
        protected readonly IWalletController _moneyController;
        protected readonly ISkinController _skinController;
        protected readonly IWeaponController _weaponController;
        protected readonly ItemDescriptor _itemDescriptor;



        public BonusResolver(
            BonusProvider bonusProvider,
            IWalletController moneyController,
            ISkinController skinController,
            IWeaponController weaponController,
            ItemDescriptor itemDescriptor
            )
        {
            _bonusProvider = bonusProvider;
            _moneyController = moneyController;
            _skinController = skinController;
            _weaponController = weaponController;
            _itemDescriptor = itemDescriptor;
        }

        public void Resolve(ItemName itemName, int count)
        {
            var itemData = _itemDescriptor.GetItemData(itemName);
            switch (itemData.ItemType)
            {
                case ItemType.Money:
                    _moneyController.AddMoney(count);
                    break;

                case ItemType.Skin:
                    _skinController.AddSkin(itemName);
                    break;

                case ItemType.Weapon:
                    _weaponController.AddWeapon(itemName);
                    break;

                // add new types
                default:
                    break;
            }
        }
    }
}

