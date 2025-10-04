using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    public void Die()
    {
        Destroy(gameObject);
    }
}
