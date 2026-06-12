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

        public static GameObject Get(string resourcePath, int initialSize = 10)
        {
            if (!pools.ContainsKey(resourcePath))
            {
                pools[resourcePath] = new Queue<GameObject>();
            }

            var q = pools[resourcePath];
            if (q.Count == 0)
            {
                var prefab = RuntimeBloodAssets.CreatePrefab(resourcePath);
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

        public static void Release(string resourcePath, GameObject obj)
        {
            if (obj == null) return;
            obj.SetActive(false);
            if (!pools.ContainsKey(resourcePath)) pools[resourcePath] = new Queue<GameObject>();
            pools[resourcePath].Enqueue(obj);
            obj.transform.SetParent(root.transform, false);
        }
    }
}
