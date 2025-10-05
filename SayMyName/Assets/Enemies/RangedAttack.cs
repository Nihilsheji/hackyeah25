using Sirenix.OdinInspector;
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
        public float projectileLifetime = 2.0f;
        public bool IsActiveImmediately = false;

        private bool IsActive = false;

        private float nextUseTime = 0f;

        private void Start()
        {
            if(this.IsActiveImmediately) {
                this.Activate();
            }
        }

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

        [Button]
        public void Activate()
        {
            this.IsActive = true;
        }

        [Button]
        public void Deactivate()
        {
            this.IsActive = false;
        }

        // Call this method to shoot (from any script)
        public void Shoot()
        {
            if (!IsActive) return;

            Debug.Log("Shooting");

            if (projectilePrefab == null)
            {
                Debug.LogError("No projectile prefab assigned!");
                return;
            }

            // Determine spawn position
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

            // Create projectile
            SpawnProjectile(spawnPos, transform.rotation, new Vector3(0, 0, 0));
            SpawnProjectile(spawnPos, transform.rotation, new Vector3(0, -5.0f, 0));
            SpawnProjectile(spawnPos, transform.rotation, new Vector3(0, -10.0f, 0));
            SpawnProjectile(spawnPos, transform.rotation, new Vector3(0, -20.0f, 0));
        }

        public void SpawnProjectile(Vector3 spawnPos, Quaternion rotation, Vector3 forceVectorOffset)
        {
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, rotation);

            GameObject.Destroy(projectile, projectileLifetime);

            // Set projectile velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.rotation * Vector3.forward * projectileSpeed + forceVectorOffset;
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
