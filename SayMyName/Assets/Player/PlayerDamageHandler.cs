using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [SerializeField] private IntEvent damagePlayerEvent;
    [SerializeField] private VoidEvent playerDied;

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
        playerDied.Raise();
    }
}
