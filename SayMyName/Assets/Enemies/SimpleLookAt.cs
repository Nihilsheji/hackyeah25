using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Assets.Enemies
{
    internal class SimpleLookAt : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable cameraGameObjectVariable;

        [Header("Movement Settings")]
        public float rotationSpeed = 5f;

        public bool IsInitialized { get; private set; }
        public Transform aimTarget;


        private Transform target; // The object to follow (usually the player)
        private bool isActive = true;

        void Start() {}

        private void OnEnable()
        {
            if (cameraGameObjectVariable.Value == null)
                cameraGameObjectVariable.Changed.Register(OnCameraGameObjectVariableChanged);
            else
                OnCameraGameObjectVariableChanged();
        }

        private void OnDisable()
        {
            cameraGameObjectVariable.Changed.Unregister(OnCameraGameObjectVariableChanged);
        }

        private void OnCameraGameObjectVariableChanged()
        {
            target = cameraGameObjectVariable.Value.transform;
            IsInitialized = true;
            cameraGameObjectVariable.Changed.Unregister(OnCameraGameObjectVariableChanged);
        }

        [Button]
        public void Activate()
        {
            this.isActive = true;
        }

        [Button]
        public void Deactivate()
        {
            this.isActive = false;
        }

        void Update()
        {
            if (target == null) return;

            if (!isActive) return;

            // Simple direct movement
            SimpleRotate();
        }

        void SimpleRotate()
        {
            aimTarget.position = Vector3.Lerp(aimTarget.position, target.position, rotationSpeed * Time.deltaTime);
            transform.LookAt(aimTarget);
        }
    }
}
