using System.Collections;
using System.Collections.Generic;
using Game.Wheel;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    [Inject] private readonly IWheelController _wheelController;
    void Start()
    {
        _wheelController.PrepareWheel();
    }

}
