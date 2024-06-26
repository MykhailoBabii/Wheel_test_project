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
        private List<SegmentData> _currentSegmentDataList = new();
        private List<SegmentData> _firstSpins = new();
        private SegmentData _currentSegmentData;

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
            _currentSegmentDataList = GenerateSpinBonusList();
            _wheelView.PrepareWheel(_currentSegmentDataList);
        }

        public void TrySpin()
        {
            _walletController.TrySpend(_spinPrice.Value, Spin);
        }

        private void ApplyBonus()
        {
            _bonusResolver.Resolve(_currentSegmentData.ItemName, _currentSegmentData.Count);
        }

        private void Spin()
        {
            _spinAttempt.Plus(1);
            _canSpin.SetValue(false);
            _currentSegmentData = GetRandomSegmentData();
            var spinTime = _wheelSettings.SpinTime;
            var spinCount = _wheelSettings.SpinCount;
            var bonusIndex = _currentSegmentData.ListIndex;
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

            // _currentSegmentDataList = ListTools.GetShuffleList(_currentSegmentDataList);
        }

        private List<SegmentData> GenerateSpinBonusList()
        {
            List<SegmentData> segmentDatas = new();
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
                var segmentData = new SegmentData(type, count, i, sprite, probability);

                if (data.IsFirst == true)
                {
                    _firstSpins.Add(segmentData);
                }

                segmentDatas.Add(segmentData);
            }

            return segmentDatas;
        }

        private SegmentData GetRandomSegmentData()
        {
            if (_firstSpins.Count > _spinAttempt.Value)
            {
                var data = _firstSpins[_spinAttempt.Value];
                Debug.Log($"Selected [First] bonus: {data.ItemName} X{data.Count}");
                return data;
            }

            while (true)
            {
                var selected = ListTools.GetRandom(_currentSegmentDataList);

                if (UnityEngine.Random.value < selected.Probability)
                {
                    Debug.Log($"Selected [Random] bonus: {selected.ItemName} X{selected.Count}");
                    return selected;
                }
            }
        }

        private void CheckToSetAnotherData()
        {
            var itemName = _currentSegmentData.ItemName;
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
            var bonusIndex = _currentSegmentData.ListIndex;
            if (itemName == ItemName.None)
            {
                _currentSegmentDataList.Remove(_currentSegmentData);
                _wheelView.BlockSection(bonusIndex);
                Debug.Log("Any new items are missing");
                return;
            }

            var sprite = _itemDescriptor.GetItemIcon(itemName);
            var count = _currentSegmentData.Count;
            var probability = _currentSegmentData.Probability;
            var newData = new SegmentData(itemName, count, bonusIndex, sprite, probability);
            var section = _wheelView.GetSection(_currentSegmentData.ListIndex);

            section.SetIcon(sprite);
            _currentSegmentDataList.Remove(_currentSegmentData);
            _currentSegmentDataList.Add(newData);
        }
    }
}
