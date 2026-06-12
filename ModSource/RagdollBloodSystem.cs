using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    // Detects ragdoll bodies and attaches blood hit proxies without modifying physics.
    public class RagdollBloodSystem : MonoBehaviour
    {
        private static RagdollBloodSystem instance;
        private readonly List<Collider> registeredColliders = new List<Collider>();
        private float scanCooldown = 0.5f;

        public static void Initialize()
        {
            if (instance != null) return;
            var go = new GameObject("BloodLab_RagdollBloodSystem");
            Object.DontDestroyOnLoad(go);
            instance = go.AddComponent<RagdollBloodSystem>();
        }

        private void Update()
        {
            scanCooldown -= Time.deltaTime;
            if (scanCooldown > 0f) return;
            scanCooldown = 2f;
            CleanupDestroyedColliders();
            ScanForRagdollBodies();
        }

        private void CleanupDestroyedColliders()
        {
            for (int i = registeredColliders.Count - 1; i >= 0; --i)
            {
                if (registeredColliders[i] == null)
                    registeredColliders.RemoveAt(i);
            }
        }

        private void ScanForRagdollBodies()
        {
            var bodies = Object.FindObjectsOfType<Rigidbody>();
            foreach (var body in bodies)
            {
                if (body == null || body.isKinematic) continue;
                var root = FindPotentialRagdollRoot(body.transform);
                if (root == null) continue;
                AttachProxies(root);
            }
        }

        private Transform FindPotentialRagdollRoot(Transform current)
        {
            Transform lastCandidate = null;
            while (current != null)
            {
                var lowerName = current.name.ToLower();
                if (current.GetComponent<Animator>() != null)
                    lastCandidate = current;

                if (lowerName.Contains("ragdoll") || lowerName.Contains("pelvis") || lowerName.Contains("hips") || lowerName.Contains("spine"))
                    return current;

                current = current.parent;
            }
            return lastCandidate ?? null;
        }

        private void AttachProxies(Transform root)
        {
            var colliders = root.GetComponentsInChildren<Collider>(true);
            foreach (var collider in colliders)
            {
                if (collider == null || collider.attachedRigidbody == null) continue;
                RegisterCollider(collider, root);
            }
        }

        private void RegisterCollider(Collider collider, Transform root)
        {
            if (registeredColliders.Contains(collider)) return;
            if (collider.GetComponent<BloodRagdollProxy>() != null)
            {
                registeredColliders.Add(collider);
                return;
            }

            var proxy = collider.gameObject.AddComponent<BloodRagdollProxy>();
            proxy.Initialize(root);
            registeredColliders.Add(collider);
        }
    }
}
