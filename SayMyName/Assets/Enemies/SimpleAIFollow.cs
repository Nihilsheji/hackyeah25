using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAIFollowScript : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The object to follow (usually the player)

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 2f; // Stop when this close to target

    [Header("Optional: Use NavMesh")]
    public bool useNavMesh = false;
    private UnityEngine.AI.NavMeshAgent navAgent;

    void Start()
    {
        // If using NavMesh, get the component
        if (useNavMesh)
        {
            navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent != null)
            {
                navAgent.speed = moveSpeed;
                navAgent.stoppingDistance = stoppingDistance;
            }
        }
    }

    void Update()
    {
        if (target == null) return;

        if (useNavMesh && navAgent != null)
        {
            // NavMesh AI (handles pathfinding automatically)
            navAgent.SetDestination(target.position);
        }
        else
        {
            // Simple direct movement
            SimpleFollow();
        }
    }

    void SimpleFollow()
    {
        // Calculate distance to target
        float distance = Vector3.Distance(transform.position, target.position);

        // Only move if beyond stopping distance
        if (distance > stoppingDistance)
        {
            // Calculate direction to target
            Vector3 direction = (target.position - transform.position).normalized;

            // Move towards target
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Rotate to face target
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = lookRotation;
        }
    }
}
