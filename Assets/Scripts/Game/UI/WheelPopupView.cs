using System;
using Game.Settings;
using Game.Wheel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class WheelPopupView : MonoBehaviour
    {
        [Inject] private readonly IWheelController _wheelController;
        [Inject] private readonly IWalletController _walletController;
        [Inject] private readonly SpinPriceProvider _spinPriceProvider;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _coinCountText;
        [SerializeField] private TextMeshProUGUI _attemptCountText;
        [SerializeField] private TextMeshProUGUI _spinPiceText;

        private string _maxAttemptions;

        void Awake()
        {
            _maxAttemptions = $"{_spinPriceProvider.PriceList.Count}";

            _spinButton.onClick.AddListener(SpinHandler);
            _walletController.CoinsCount.AddValueChangedListenter(SetCoinCount, true);
            _wheelController.SpinAttempt.AddValueChangedListenter(SetAttemptCount, true);
            _wheelController.SpinPrice.AddValueChangedListenter(SetSpinPrice, true);
            _wheelController.CanSpin.AddValueChangedListenter(SwitchSpinButton);
        }

        private void SwitchSpinButton(bool canSpin)
        {
            _spinButton.interactable = canSpin;
        }

        private void SetAttemptCount(int count)
        {
            _attemptCountText.SetText($"{count}/{_maxAttemptions}");
        }

        private void SetCoinCount(int count)
        {
            _coinCountText.SetText($"{count}");
        }

        private void SpinHandler()
        {
            _wheelController.TrySpin();
        }

        private void SetSpinPrice(int price)
        {
            _spinPiceText.SetText($"{price}");
        }
    }
}
