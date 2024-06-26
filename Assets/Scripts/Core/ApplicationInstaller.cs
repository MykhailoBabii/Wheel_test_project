using System;
using Core;
using Game;
using Game.Settings;
using Game.UI;
using Game.Wheel;
using UnityEngine;
using Zenject;

public class ApplicationInstaller : MonoInstaller
{
    [SerializeField] private WheelSettings _wheelSettings;
    [SerializeField] private BonusProvider _bonusProvider;
    [SerializeField] private SpinPriceProvider _spinPriceProvider;
    [SerializeField] private ItemDescriptor _itemDescriptor;
    [SerializeField] private WheelView _wheelView;
    [SerializeField] private WheelPopupView _wheelPopupView;
    [SerializeField] private EntryPoint _entryPoint;
    

    public override void InstallBindings()
    {
        InstalScriptableObjects();
        InstallControllers();
        InstallBehaviours();
    }

    private void InstallBehaviours()
    {
        Container.Bind<WheelView>().FromInstance(_wheelView).AsSingle();
        Container.Bind<EntryPoint>().FromInstance(_entryPoint).AsSingle();
        Container.Bind<WheelPopupView>().FromInstance(_wheelPopupView).AsSingle();
    }

    private void InstallControllers()
    {
        Container.Bind<IWheelController>().To<WheelController>().AsSingle();
        Container.Bind<IBonusResolver>().To<BonusResolver>().AsSingle();
        Container.Bind<IMoneyController>().To<MoneyController>().AsSingle();
        Container.Bind<ISkinController>().To<SkinController>().AsSingle();
        Container.Bind<IWeaponController>().To<WeaponController>().AsSingle();
        Container.Bind<IWalletController>().To<WalletController>().AsSingle();
    }

    private void InstalScriptableObjects()
    {
        Container.Bind<WheelSettings>().FromInstance(_wheelSettings).AsSingle();
        Container.Bind<BonusProvider>().FromInstance(_bonusProvider).AsSingle();
        Container.Bind<SpinPriceProvider>().FromInstance(_spinPriceProvider).AsSingle();
        Container.Bind<ItemDescriptor>().FromInstance(_itemDescriptor).AsSingle();
    }
}
