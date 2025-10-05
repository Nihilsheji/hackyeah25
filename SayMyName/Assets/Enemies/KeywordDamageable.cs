using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordDamageable : MonoBehaviour, IKeywordReactable
{
    [SerializeField] private AimTarget aimTarget;

    public void OnKeywordRecognized()
    {
        if (aimTarget.IsAimedAt == false)
            return;

        Destroy(gameObject);
    }
}
