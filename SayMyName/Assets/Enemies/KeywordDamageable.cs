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

        var deathHandler = gameObject.GetComponent<EnemyDeathHandler>();
        
        if(deathHandler != null )
        {
            deathHandler.Die();
        }
    }
}
