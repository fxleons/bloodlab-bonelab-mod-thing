using System.Collections.Generic;
using UnityEngine;

namespace BloodLabMod.Core
{
    public static class PoolManager
    {
        private static Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
        private static GameObject root;

        public static void Initialize()
        {
            root = new GameObject("BloodLab_Pools");
            Object.DontDestroyOnLoad(root);
        }

        public static GameObject Get(string prefabKey, int initialSize = 10)
        {
            if (!pools.ContainsKey(prefabKey))
            {
                pools[prefabKey] = new Queue<GameObject>();
            }

            var q = pools[prefabKey];
            if (q.Count == 0)
            {
                var prefab = RuntimeBloodAssets.CreatePrefab(prefabKey);
                if (prefab == null) return null;
                var go = Object.Instantiate(prefab, root.transform);
                go.SetActive(false);
                return go;
            }
            else
            {
                var obj = q.Dequeue();
                return obj;
            }
        }

        public static void Release(string prefabKey, GameObject obj)
        {
            if (obj == null) return;
            obj.SetActive(false);
            if (!pools.ContainsKey(prefabKey)) pools[prefabKey] = new Queue<GameObject>();
            pools[prefabKey].Enqueue(obj);
            obj.transform.SetParent(root.transform, false);
        }
    }
}
