using System;
using System.Collections;
using System.Collections.Generic;
using Core.Utilities;
using Game.Settings;
using UnityEngine;

public interface IWalletController
{
    void AddMoney(int count);
    void TrySpend(int count, Action onSpendSuccess);
    IIntReadOnlyProperty CoinsCount { get; }
}

public class WalletController : IWalletController
{
    protected readonly WheelSettings _wheelSettings;
    private IntProperty _coinsCount = new(0);

    public IIntReadOnlyProperty CoinsCount => _coinsCount;

    public WalletController(WheelSettings wheelSettings)
    {
        _wheelSettings = wheelSettings;
        _coinsCount.SetValue(_wheelSettings.StarCoins);
    }
    public void TrySpend(int count, Action onSpendSuccess)
    {
        if (_coinsCount.Value >= count)
        {
            _coinsCount.Minus(count);
            onSpendSuccess?.Invoke();
        }

        else
        {
            Debug.Log("No money");
        }
    }

    public void AddMoney(int count)
    {
        _coinsCount.Plus(count);
        Debug.Log($"Money is added: {count}");
    }
}

