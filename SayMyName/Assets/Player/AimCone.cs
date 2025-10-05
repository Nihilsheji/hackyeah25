using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class AimCone : MonoBehaviour
{
    [SerializeField] private float AimDistance;
    [SerializeField] private GameObjectVariable cameraGameObjectVariable;
    [SerializeField] private GameObjectEvent OnAimTargetDestroyedEvent;

    private List<AimTarget> aimTargets = new();
    private GameObject cameraGameObject;

    private bool IsInitialized;

    private void OnEnable()
    {
        OnAimTargetDestroyedEvent.Register(OnAimTargetDestroyed);

        if (cameraGameObjectVariable.Value == null)
            cameraGameObjectVariable.Changed.Register(OnCameraGameObjectVariableChanged);
        else
            OnCameraGameObjectVariableChanged();
    }

    private void OnDisable()
    {
        OnAimTargetDestroyedEvent.Unregister(OnAimTargetDestroyed);

        cameraGameObjectVariable.Changed.Unregister(OnCameraGameObjectVariableChanged);
    }

    private void OnCameraGameObjectVariableChanged()
    {
        cameraGameObject = cameraGameObjectVariable.Value;
        IsInitialized = true;
    }

    private void Update()
    {
        if (IsInitialized == false)
            return;

        for (int i = 0; i < aimTargets.Count; i++)
        {
            float distance = Vector3.Distance(cameraGameObject.transform.position, aimTargets[i].transform.position);
            if (distance <= AimDistance)
            {
                aimTargets[i].OnInAimCone();
            }
            else
            {
                aimTargets[i].OnOutOfAimCone();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AimTarget aimTarget;

        if (other.gameObject.TryGetComponent<AimTarget>(out aimTarget) && aimTargets.Contains(aimTarget) == false)
        {
            aimTargets.Add(aimTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        AimTarget aimTarget;

        if (other.gameObject.TryGetComponent<AimTarget>(out aimTarget) && aimTargets.Contains(aimTarget) == false)
        {
            aimTargets.Remove(aimTarget);
        }
    }

    private void OnAimTargetDestroyed(GameObject destroyed)
    {
        for (int i = 0; i < aimTargets.Count; i++)
        {
            if(aimTargets[i].gameObject == destroyed)
            {
                aimTargets.Remove(aimTargets[i]);
                return;
            }
        }
    }
}
