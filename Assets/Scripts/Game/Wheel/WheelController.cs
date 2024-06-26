using System;
using System.Collections.Generic;
using Core.Utilities;
using Game.Item;
using Game.Settings;
using Game.UI;
using UnityEngine;

namespace Game.Wheel
{
    public interface IWheelController
    {
        void TrySpin();
        void PrepareWheel();
        IIntReadOnlyProperty SpinAttempt { get; }
        IIntReadOnlyProperty SpinPrice { get; }
        IBoolReadOnlyProperty CanSpin { get; }
    }

    public class WheelController : IWheelController
    {
        public IIntReadOnlyProperty SpinAttempt => _spinAttempt;
        public IIntReadOnlyProperty SpinPrice => _spinPrice;
        public IBoolReadOnlyProperty CanSpin => _canSpin;

        private BoolProperty _canSpin = new(true);
        private IntProperty _spinAttempt = new(0);
        private IntProperty _spinPrice = new(0);
        private List<SpinData> _currentSpinDataList = new();
        private List<SpinData> _firstSpins = new();
        private SpinData _currentSpinData;

        protected readonly BonusProvider _bonusProvider;
        protected readonly WheelSettings _wheelSettings;
        protected readonly IBonusResolver _bonusResolver;
        protected readonly WheelView _wheelView;
        protected readonly IWalletController _walletController;
        protected readonly ISkinController _skinController;
        protected readonly IWeaponController _weaponController;
        protected readonly SpinPriceProvider _spinPriceProvider;
        protected readonly ItemDescriptor _itemDescriptor;


        public WheelController(
            BonusProvider bonusProvider,
            IBonusResolver bonusResolver,
            WheelView wheelView,
            WheelSettings wheelSettings,
            IWalletController walletController,
            SpinPriceProvider spinPriceProvider,
            ItemDescriptor itemDescriptor,
            IWeaponController weaponController,
            ISkinController skinController)
        {
            _bonusProvider = bonusProvider;
            _bonusResolver = bonusResolver;
            _wheelView = wheelView;
            _wheelSettings = wheelSettings;
            _walletController = walletController;
            _spinPriceProvider = spinPriceProvider;
            _itemDescriptor = itemDescriptor;
            _weaponController = weaponController;
            _skinController = skinController;
        }

        public void PrepareWheel()
        {
            _currentSpinDataList = GenerateSpinBonusList();
            _wheelView.PrepareWheel(_currentSpinDataList);
        }

        public void TrySpin()
        {
            _walletController.TrySpend(_spinPrice.Value, Spin);
        }

        private void ApplyBonus()
        {
            _bonusResolver.Resolve(_currentSpinData.ItemName, _currentSpinData.Count);
        }

        private void Spin()
        {
            _spinAttempt.Plus(1);
            _canSpin.SetValue(false);
            _currentSpinData = GetRandomSpinData();
            var spinTime = _wheelSettings.SpinTime;
            var spinCount = _wheelSettings.SpinCount;
            var bonusIndex = _currentSpinData.ListIndex;
            _wheelView.SpinToSection(bonusIndex, spinTime, spinCount, OnSpinComplete);
        }

        private void OnSpinComplete()
        {
            ApplyBonus();
            CheckToSetAnotherData();

            if (_spinPriceProvider.PriceList.Count == _spinAttempt.Value)
            {
                _spinPrice.SetValue(0);
                Debug.Log("Attempts is out");
            }

            else
            {
                var spinPrice = _spinPriceProvider.PriceList[_spinAttempt.Value];
                _spinPrice.SetValue(spinPrice);
                _canSpin.SetValue(true);
                Debug.Log($"Can spin {_canSpin.Value}");
            }

            // _currentSpinDataList = ListTools.GetShuffleList(_currentSpinDataList);
        }

        private List<SpinData> GenerateSpinBonusList()
        {
            List<SpinData> spinDatas = new();
            var bonusDatas = _bonusProvider.GetBonuses();
            var shuffleList = ListTools.GetShuffleList(bonusDatas);
            var bonusDatasList = new List<BonusData>(shuffleList);

            for (int i = 0; i < bonusDatasList.Count; i++)
            {
                
                var data = bonusDatasList[i];
                var type = data.ItemType;
                var count = data.BonusCount;
                var probability = data.Probability;
                var sprite = _itemDescriptor.GetItemIcon(type);
                var spinData = new SpinData(type, count, i, sprite, probability);

                if (data.IsFirst == true)
                {
                    _firstSpins.Add(spinData);
                }

                spinDatas.Add(spinData);
            }

            return spinDatas;
        }

        private SpinData GetRandomSpinData()
        {
            if (_firstSpins.Count > _spinAttempt.Value)
            {
                var data = _firstSpins[_spinAttempt.Value];
                Debug.Log($"Selected [First] bonus: {data.ItemName} X{data.Count}");
                return data;
            }

            while (true)
            {
                var selected = ListTools.GetRandom(_currentSpinDataList);

                if (UnityEngine.Random.value < selected.Probability)
                {
                    Debug.Log($"Selected [Random] bonus: {selected.ItemName} X{selected.Count}");
                    return selected;
                }
            }
        }

        private void CheckToSetAnotherData()
        {
            var itemName = _currentSpinData.ItemName;
            var itemType = _itemDescriptor.GetItemData(itemName).ItemType;

            switch (itemType)
            {
                case ItemType.Skin:
                    var skin = _skinController.GetUniqueItem();
                    TryChangeBonusItem(skin);
                    Debug.Log($"{itemName} changed to {skin}");
                    break;
                case ItemType.Weapon:
                    var weapon = _weaponController.GetUniqueItem();
                    TryChangeBonusItem(weapon);
                    Debug.Log($"{itemName} changed to {weapon}");
                    break;

                default: break;
            }
        }

        private void TryChangeBonusItem(ItemName itemName)
        {
            var bonusIndex = _currentSpinData.ListIndex;
            if (itemName == ItemName.None)
            {
                _currentSpinDataList.Remove(_currentSpinData);
                _wheelView.BlockSection(bonusIndex);
                Debug.Log("Any new items are missing");
                return;
            }

            var sprite = _itemDescriptor.GetItemIcon(itemName);
            var count = _currentSpinData.Count;
            var probability = _currentSpinData.Probability;
            var newData = new SpinData(itemName, count, bonusIndex, sprite, probability);
            var section = _wheelView.GetSection(_currentSpinData.ListIndex);

            section.SetIcon(sprite);
            _currentSpinDataList.Remove(_currentSpinData);
            _currentSpinDataList.Add(newData);
        }
    }
}
