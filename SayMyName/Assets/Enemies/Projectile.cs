using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class Projectile : MonoBehaviour
{
    [SerializeField] private IntEvent damagePlayerEvent;

    public int damageAmount = 1;
    public float lifetime = 5f; // Auto-destroy after this time
    public GameObject hitEffectPrefab; // Optional hit effect
    public bool Slowdown = false;
    public float SlowdownMulti = 0.5f;

    void Start()
    {
        // Destroy projectile after lifetime
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            damagePlayerEvent.Raise(damageAmount);
        }
    }
}
