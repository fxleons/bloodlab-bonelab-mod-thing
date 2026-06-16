using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BloodLabMod.Core
{
    public class BonelabDamageIntegration : MonoBehaviour
    {
        private static BonelabDamageIntegration instance;
        private static bool waitingForScene;
        private float scanTimer = 0f;
        private readonly HashSet<Rigidbody> sourceBodies = new HashSet<Rigidbody>();
        private readonly HashSet<Collider> ragdollColliders = new HashSet<Collider>();
        private readonly Dictionary<int, float> recentHits = new Dictionary<int, float>();
        private static readonly string[] sourceKeywords = new[]
        {
            "bullet", "projectile", "shell", "grenade", "rocket", "arrow", "knife", "sword", "bat", "club",
            "axe", "hammer", "machete", "dagger", "spear", "weapon", "melee", "knife", "punch", "hand"
        };

        public static void Initialize()
        {
            if (instance != null) return;

            if (SceneManager.GetActiveScene().isLoaded)
            {
                CreateInstance();
                return;
            }

            if (waitingForScene) return;
            SceneManager.sceneLoaded += OnSceneLoaded;
            waitingForScene = true;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (instance != null) return;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            waitingForScene = false;
            CreateInstance();
        }

        private static void CreateInstance()
        {
            var go = new GameObject("BloodLab_BonelabDamageIntegration");
            Object.DontDestroyOnLoad(go);
            instance = go.AddComponent<BonelabDamageIntegration>();
        }

        private void Update()
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer > 0f) return;
            scanTimer = 0.5f;
            ScanDamageSources();
            ScanRagdollTargets();
            CleanupRecentHits();
        }

        private void ScanDamageSources()
        {
            foreach (var body in Object.FindObjectsOfType<Rigidbody>())
            {
                if (body == null || body.isKinematic) continue;
                if (sourceBodies.Contains(body)) continue;
                if (!IsLikelyDamageSource(body)) continue;

                var source = body.gameObject.GetComponent<BloodHitSource>();
                if (source == null)
                {
                    source = body.gameObject.AddComponent<BloodHitSource>();
                    source.Initialize(this);
                }
                sourceBodies.Add(body);
            }
        }

        private void ScanRagdollTargets()
        {
            foreach (var body in Object.FindObjectsOfType<Rigidbody>())
            {
                if (body == null || body.isKinematic) continue;
                var root = FindPotentialRagdollRoot(body.transform);
                if (root == null) continue;

                var colliders = root.GetComponentsInChildren<Collider>(true);
                foreach (var collider in colliders)
                {
                    if (collider == null || ragdollColliders.Contains(collider)) continue;
                    ragdollColliders.Add(collider);
                }
            }
        }

        private bool IsLikelyDamageSource(Rigidbody body)
        {
            var go = body.gameObject;
            if (go == null) return false;
            var name = go.name.ToLower();
            if (ContainsKeyword(name, sourceKeywords)) return true;
            if (body.velocity.sqrMagnitude > 4f) return true;
            if (body.mass > 1.0f && body.velocity.sqrMagnitude > 1f) return true;
            return false;
        }

        private bool ContainsKeyword(string text, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (text.Contains(keyword)) return true;
            }
            return false;
        }

        internal bool IsRagdollTarget(Collider collider)
        {
            return collider != null && ragdollColliders.Contains(collider);
        }

        internal void ProcessHit(Collider sourceCollider, Collider hitCollider, Vector3 hitPoint, Vector3 hitNormal, Vector3 relativeVelocity, GameObject sourceObject)
        {
            if (hitCollider == null || sourceCollider == null || hitCollider.gameObject == sourceObject) return;
            if (!IsRagdollTarget(hitCollider)) return;

            var key = (sourceObject.GetInstanceID() * 397) ^ hitCollider.GetInstanceID();
            if (recentHits.TryGetValue(key, out var lastTime) && Time.time - lastTime < 0.15f)
                return;
            recentHits[key] = Time.time;

            var root = FindPotentialRagdollRoot(hitCollider.transform);
            if (root == null) return;

            var bodyPart = BodyPartIdentifier.Identify(root, hitCollider.transform);
            var damage = Mathf.Clamp(relativeVelocity.magnitude * 2.0f, 0.5f, 10f);
            HookManager.OnEntityHit(root, hitPoint, hitNormal, damage, bodyPart, sourceObject);
        }

        private void CleanupRecentHits()
        {
            var now = Time.time;
            var keys = new List<int>(recentHits.Keys);
            foreach (var key in keys)
            {
                if (now - recentHits[key] > 0.25f)
                    recentHits.Remove(key);
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

                if (lowerName.Contains("ragdoll") || lowerName.Contains("pelvis") || lowerName.Contains("hips") || lowerName.Contains("spine") || lowerName.Contains("torso") || lowerName.Contains("body"))
                    return current;

                current = current.parent;
            }
            return lastCandidate;
        }
    }
}
