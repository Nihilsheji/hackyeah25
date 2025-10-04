using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class DamagerCollider : MonoBehaviour
{
    [SerializeField] private IntEvent damagePlayerEvent;
    [SerializeField] private int damageAmount;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            damagePlayerEvent.Raise(damageAmount);
        }
    }
}
