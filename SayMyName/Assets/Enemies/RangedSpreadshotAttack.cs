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
    internal class RangedSpreadshotAttack : MonoBehaviour
    {
        [SerializeField] AudioSource attackAudioSource;

        [Header("Projectile Settings")]
        public GameObject projectilePrefab; // Drag your projectile prefab here
        public Transform spawnPoint; // Where projectile spawns (optional)

        [Header("Attack Settings")]
        public float projectileSpeed = 20f;
        public float fireRate = 0.5f; // Time between shots
        public int damage = 10;
        public float projectileLifetime = 5.0f;
        public bool IsActiveImmediately = false;
        public int projectileCount = 5;
        public float coneAngle = 30f; // Total cone angle in degrees

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

            if(attackAudioSource != null)
            {
                attackAudioSource.Play();
            }

            SpawnProjectile(spawnPos, Quaternion.identity);
            SpawnCone();
        }

        public void SpawnProjectile(Vector3 spawnPos, Quaternion rotation)
        {
            // Create projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, transform.rotation);

            GameObject.Destroy(projectile, projectileLifetime);

            // Set projectile velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = rotation * transform.forward * projectileSpeed;
            }

            // Pass damage to projectile
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.damageAmount = damage;
            }
        }

        // Main method to spawn projectiles in cone
        public void SpawnCone()
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
            Vector3 forward = spawnPoint != null ? spawnPoint.localRotation * Vector3.forward : transform.localRotation * Vector3.forward;

            for (int i = 1; i < projectileCount; i++)
            {
                // Calculate direction based on pattern
                Vector3 direction = GetRandomDirection(forward);

                SpawnProjectile(spawnPos, Quaternion.LookRotation(direction));
            }
        }

        Vector3 GetRandomDirection(Vector3 forward)
        {
            // Random angle within cone
            float randomAngle = UnityEngine.Random.Range(-coneAngle / 2f, coneAngle / 2f);

            // Random rotation around forward axis
            float randomRotation = UnityEngine.Random.Range(0f, 360f);

            // Apply rotations
            Quaternion angleRotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
            Quaternion spinRotation = Quaternion.AngleAxis(randomRotation, forward);

            return spinRotation * angleRotation * forward;
        }
    }
}
