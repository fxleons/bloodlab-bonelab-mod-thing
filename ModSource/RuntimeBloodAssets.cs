using UnityEngine;

namespace BloodLabMod.Core
{
    public static class RuntimeBloodAssets
    {
        private static Material bloodMaterial;

        public static Material GetBloodMaterial()
        {
            if (bloodMaterial != null)
                return bloodMaterial;

            var shader = Shader.Find("Unlit/Color");
            if (shader == null)
                shader = Shader.Find("Standard");
            bloodMaterial = new Material(shader ?? Shader.Find("Sprites/Default"));
            bloodMaterial.color = ConfigManager.Settings.BloodColor;
            return bloodMaterial;
        }

        public static GameObject CreateBloodDecal()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = "BloodDecal";
            var rend = go.GetComponent<Renderer>();
            if (rend != null)
                rend.material = GetBloodMaterial();
            go.transform.localScale = Vector3.one * 0.25f;
            return go;
        }

        public static GameObject CreateBloodPuddle()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = "BloodPuddle";
            var rend = go.GetComponent<Renderer>();
            if (rend != null)
                rend.material = GetBloodMaterial();
            go.transform.localScale = Vector3.one * 0.5f;
            return go;
        }

        public static GameObject CreateBloodDroplet()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "BloodDroplet";
            var rend = go.GetComponent<Renderer>();
            if (rend != null)
                rend.material = GetBloodMaterial();
            go.transform.localScale = Vector3.one * 0.04f;
            return go;
        }

        public static GameObject CreatePrefab(string prefabKey)
        {
            if (prefabKey == "BloodDroplet")
                return CreateBloodDroplet();
            if (prefabKey == "BloodPuddle")
                return CreateBloodPuddle();
            if (prefabKey == "BloodDecal")
                return CreateBloodDecal();
            return null;
        }
    }
}
