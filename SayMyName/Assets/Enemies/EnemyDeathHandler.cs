using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [SerializeField] MonoBehaviour deathClipPlayer;
    public void Die()
    {
        Debug.Log("Die called");
        if(deathClipPlayer != null)
        {
            Debug.Log("Instantiating death clip player");
            var deathPrefab = Instantiate(deathClipPlayer, null, true);
            Destroy(deathPrefab, 3.0f);
        }
        Destroy(gameObject);
    }
}
