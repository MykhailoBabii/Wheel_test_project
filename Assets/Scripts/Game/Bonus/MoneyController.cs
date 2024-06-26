using System.Collections;
using System.Collections.Generic;
using Game.Item;
using Game.Settings;
using UnityEngine;

namespace Core
{
    public interface IMoneyController
    {
        void AddMoney(int count);
    }

    public class MoneyController : IMoneyController
    {
        public void AddMoney(int count)
        {
            Debug.Log($"Money is added. Count: {count}");
        }
    }
}


