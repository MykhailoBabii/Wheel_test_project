using System;
using System.Collections;
using System.Collections.Generic;
using Game.Item;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "SpinPriceProvider", menuName = "Create/SpinPriceProvider")]
    public class SpinPriceProvider : ScriptableObject
    {
        [field: SerializeField] public List<int> PriceList { get; private set; }
    }
}