using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.StateManager;

namespace SpaceInvaders
{
    public class ConIsGameOver : StateConnection
    {
        protected override void OnCheckCondition()
        {
            _isFinished = GameManager.Instance.IsGameOver();
        }
    }
}