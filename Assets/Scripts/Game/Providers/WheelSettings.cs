using System;
using System.Collections;
using System.Collections.Generic;
using Game.Item;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "WheelSettings", menuName = "Create/Wheel Settings")]
    public class WheelSettings : ScriptableObject
    {
        [field: SerializeField] public float SpinTime { get; private set; }
        [field: SerializeField] public int SpinCount { get; private set; }
        [field: SerializeField] public int StarCoins { get; private set; }
    }
}