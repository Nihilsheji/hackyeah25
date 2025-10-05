using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

public class AimTarget : MonoBehaviour
{
    public bool IsAimedAt { get; private set; }

    [SerializeField] private EmissionController emissionController;
    [SerializeField] private GameObjectEvent OnDestroyEvent;

    public void OnInAimCone()
    {
        if (IsAimedAt == true)
            return;

        emissionController.IncreaseEmission(1f);
        IsAimedAt = true;
    }

    public void OnOutOfAimCone()
    {
        if(IsAimedAt == false)
            return;

        emissionController.DecreaseEmission(0f);
        IsAimedAt = false;
    }

    public void OnDestroy()
    {
        OnDestroyEvent.Raise(gameObject);
    }
}
