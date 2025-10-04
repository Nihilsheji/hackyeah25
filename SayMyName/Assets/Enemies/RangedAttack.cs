using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace Assets.Enemies
{
    internal class RangedAttack : MonoBehaviour
    {
        [Header("Projectile Settings")]
        public GameObject projectilePrefab; // Drag your projectile prefab here
        public Transform spawnPoint; // Where projectile spawns (optional)

        [Header("Attack Settings")]
        public float projectileSpeed = 20f;
        public float fireRate = 0.5f; // Time between shots
        public int damage = 10;
        public float projectileLifetime = 5.0f;

        private float nextUseTime = 0f;

        public void Update()
        {
            if (Time.time >= nextUseTime)
            {
                Shoot();

                // Start cooldown
                nextUseTime = Time.time + fireRate;
            }
            else
            {
                float timeRemaining = nextUseTime - Time.time;
            }
        }

        // Call this method to shoot (from any script)
        public void Shoot()
        {
            Debug.Log("Shooting");

            if (projectilePrefab == null)
            {
                Debug.LogError("No projectile prefab assigned!");
                return;
            }

            // Determine spawn position
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

            // Create projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation);

            GameObject.Destroy(projectile, projectileLifetime);

            // Set projectile velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * projectileSpeed;
            }

            // Pass damage to projectile
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.damageAmount = damage;
            }
        }
    }
}
