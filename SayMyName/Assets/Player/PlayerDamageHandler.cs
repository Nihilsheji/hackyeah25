using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [SerializeField] private IntEvent damagePlayerEvent;
    [SerializeField] private VoidEvent playerDied;
    [SerializeField] private BoolVariable isPlayerImmortal;

    private bool _isPlayerDead;

    private void OnEnable()
    {
        damagePlayerEvent.Register(handleDamagePlayerEvent);
    }

    private void OnDisable()
    {
        damagePlayerEvent.Unregister(handleDamagePlayerEvent);
    }

    private void handleDamagePlayerEvent()
    {
        if (isPlayerImmortal.Value == true)
            return;

        if (_isPlayerDead == true)
            return;

        _isPlayerDead = true;
        playerDied.Raise();
    }
}
