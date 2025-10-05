using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordDamageable : MonoBehaviour, IKeywordReactable
{
    [SerializeField] private AimTarget isAimedAt;

    public void OnKeywordRecognized()
    {
        if (isAimedAt == false)
            return;

        Destroy(gameObject);
    }
}
