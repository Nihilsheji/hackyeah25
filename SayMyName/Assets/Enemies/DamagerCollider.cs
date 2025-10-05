using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class DamagerCollider : MonoBehaviour
{
    [SerializeField] private IntEvent damagePlayerEvent;
    [SerializeField] private int damageAmount;
    [SerializeField] private AudioSource attackAudioSource;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(attackAudioSource != null) {  attackAudioSource.Play(); }
            damagePlayerEvent.Raise(damageAmount);
        }
    }
}
