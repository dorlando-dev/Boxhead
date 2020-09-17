﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FrameLord.StateManager;

public class StateActionGamePlayerDied : State
{
    protected override void OnLeaveState()
    {
        GameManager.Instance.RespawnPlayer();
    }
}