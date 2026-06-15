using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    // Manages delayed ground pools spawned beneath severe NPC ragdoll hits.
    public static class BloodPoolManager
    {
        private const int HighWoundCountThreshold = 6;
        private const float HighWoundDamageThreshold = 6f;
        private const float RaycastHeight = 0.5f;
        private const float RaycastDistance = 2f;

        private static readonly List<PendingPool> pendingPools = new List<PendingPool>(16);
        private static readonly List<BloodPool> activePools = new List<BloodPool>(16);

        public static void Initialize() { }

        public static void UpdateAll(float dt)
        {
            if (!BloodConfig.EnableBloodPools)
                return;

            for (int i = pendingPools.Count - 1; i >= 0; --i)
            {
                var pending = pendingPools[i];
                if (pending.Entity == null)
                {
                    pendingPools.RemoveAt(i);
                    continue;
                }

                if (!pending.HasMetSpawnCondition())
                    continue;

                pending.Elapsed += dt;
                if (pending.Elapsed < BloodConfig.PoolDelay)
                    continue;

                if (activePools.Count >= BloodConfig.MaxActivePools)
                {
                    // Keep candidate alive in case pools free up later.
                    continue;
                }

                SpawnPool(pending);
                pendingPools.RemoveAt(i);
            }

            for (int i = 0; i < activePools.Count; ++i)
            {
                activePools[i].Update(dt);
            }
        }

        public static void TrackHit(Transform entity, string bodyPart, float damage, GameObject hitSource)
        {
            if (!BloodConfig.EnableBloodPools || entity == null)
                return;

            int instanceId = entity.GetInstanceID();
            var pending = FindPending(instanceId);
            if (pending == null)
            {
                pending = new PendingPool(entity);
                pendingPools.Add(pending);
            }

            pending.RecordHit(bodyPart, damage);
        }

        private static PendingPool FindPending(int instanceId)
        {
            for (int i = 0; i < pendingPools.Count; ++i)
            {
                if (pendingPools[i].EntityId == instanceId)
                    return pendingPools[i];
            }
            return null;
        }

        private static void SpawnPool(PendingPool pending)
        {
            if (pending.Entity == null)
                return;

            var anchorPosition = pending.Entity.position + Vector3.up * 0.1f;
            var poolPosition = anchorPosition;
            var poolNormal = Vector3.up;

            if (Physics.Raycast(anchorPosition + Vector3.up * RaycastHeight, Vector3.down, out var hit, RaycastDistance, ~0, QueryTriggerInteraction.Ignore))
            {
                poolPosition = hit.point;
                poolNormal = hit.normal;
            }
            else if (Physics.Raycast(anchorPosition, Vector3.down, out hit, RaycastDistance, ~0, QueryTriggerInteraction.Ignore))
            {
                poolPosition = hit.point;
                poolNormal = hit.normal;
            }

            var poolGO = PoolManager.Get("BloodPrefabs/BloodPuddle");
            if (poolGO == null)
                return;

            poolGO.transform.SetParent(null, true);
            poolGO.transform.position = poolPosition + poolNormal * 0.01f;
            poolGO.transform.rotation = Quaternion.LookRotation(poolNormal);
            poolGO.SetActive(true);

            var pool = new BloodPool(poolGO, BloodConfig.PoolMaxSize, BloodConfig.PoolGrowthSpeed);
            activePools.Add(pool);
        }

        private class PendingPool
        {
            public Transform Entity { get; }
            public int EntityId { get; }
            public int HitCount { get; private set; }
            public float TotalDamage { get; private set; }
            public bool HasHeadshot { get; private set; }
            public float Elapsed { get; set; }

            public PendingPool(Transform entity)
            {
                Entity = entity;
                EntityId = entity.GetInstanceID();
                HitCount = 0;
                TotalDamage = 0f;
                HasHeadshot = false;
                Elapsed = 0f;
            }

            public void RecordHit(string bodyPart, float damage)
            {
                HitCount += 1;
                TotalDamage += damage;
                if (!HasHeadshot && bodyPart != null && bodyPart.ToLower().Contains("head"))
                {
                    HasHeadshot = true;
                }
            }

            public bool HasMetSpawnCondition()
            {
                if (BloodConfig.HeadshotsOnly)
                    return HasHeadshot;

                return HasHeadshot || (HitCount >= HighWoundCountThreshold && TotalDamage >= HighWoundDamageThreshold);
            }
        }

        private class BloodPool
        {
            private readonly GameObject poolGO;
            private readonly float targetSize;
            private readonly float growthSpeed;
            private float currentSize;

            public BloodPool(GameObject poolGO, float targetSize, float growthSpeed)
            {
                this.poolGO = poolGO;
                this.targetSize = Mathf.Max(0.01f, targetSize);
                this.growthSpeed = Mathf.Max(0.01f, growthSpeed);
                currentSize = 0.01f;
                UpdateScale();
            }

            public void Update(float dt)
            {
                if (poolGO == null)
                    return;

                if (currentSize < targetSize)
                {
                    currentSize = Mathf.MoveTowards(currentSize, targetSize, growthSpeed * dt);
                    UpdateScale();
                }
            }

            private void UpdateScale()
            {
                if (poolGO == null)
                    return;

                var scale = new Vector3(currentSize, 1f, currentSize);
                poolGO.transform.localScale = scale;
            }
        }
    }
}
