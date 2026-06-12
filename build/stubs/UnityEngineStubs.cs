using System;
using System.Numerics;
using System.Collections.Generic;

namespace UnityEngine
{
    public struct Vector3
    {
        public float x; public float y; public float z;
        public Vector3(float x,float y,float z){this.x=x;this.y=y;this.z=z;}
        public static Vector3 zero => new Vector3(0,0,0);
        public static Vector3 one => new Vector3(1,1,1);
        public static Vector3 up => new Vector3(0,1,0);
        public float sqrMagnitude => x*x + y*y + z*z;
        public float magnitude => (float)Math.Sqrt(sqrMagnitude);
        public static Vector3 operator -(Vector3 a) => new Vector3(-a.x,-a.y,-a.z);
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x+b.x,a.y+b.y,a.z+b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x-b.x,a.y-b.y,a.z-b.z);
        public static Vector3 operator *(Vector3 a, float s) => new Vector3(a.x*s,a.y*s,a.z*s);
        public static float Distance(Vector3 a, Vector3 b){var dx=a.x-b.x;var dy=a.y-b.y;var dz=a.z-b.z;return (float)Math.Sqrt(dx*dx+dy*dy+dz*dz);}    
    }

    public struct Quaternion { public static Quaternion identity => new Quaternion(); public static Quaternion LookRotation(Vector3 v) => identity; }

    public enum PrimitiveType { Sphere, Capsule, Cylinder, Cube, Plane, Quad }

    public class Shader : Object { public Shader(string name) { } public static Shader Find(string name) => new Shader(name); }

    public class Object
    {
        public string name = "Object";
        public int GetInstanceID() => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this);
        public static void DontDestroyOnLoad(Object obj) {}
        public static GameObject Instantiate(GameObject prefab, Transform parent) => GameObject.Instantiate(prefab, parent);
        public static T[] FindObjectsOfType<T>() where T : Object { return new T[0]; }
    }
    public static class Application { public static string dataPath => System.IO.Directory.GetCurrentDirectory(); }

    public static partial class ObjectEx
    {
        public static void DontDestroyOnLoad(Object obj) { }
        public static GameObject Instantiate(GameObject prefab, Transform parent) => GameObject.Instantiate(prefab, parent);
    }

    public class Component : Object
    {
        public Transform transform = new Transform();
        public GameObject gameObject = new GameObject();
        public T GetComponent<T>() where T : Component { return gameObject?.GetComponent<T>(); }
        public T[] GetComponentsInChildren<T>(bool includeInactive = true) where T : Component { return gameObject?.GetComponentsInChildren<T>(includeInactive) ?? new T[0]; }
    }

    public class GameObject : Object
    {
        public Transform transform = new Transform();
        public bool activeSelf = true;
        private List<Component> components = new List<Component>();
        public GameObject() { transform = new Transform(); transform.parent = null; transform.gameObject = this; }
        public GameObject(string name){ this.name = name; transform = new Transform(); transform.parent = null; transform.gameObject = this; }
        public void SetActive(bool v){ activeSelf = v; }
        public T GetComponent<T>() where T : Component { foreach(var c in components) if (c is T t) return t; return null; }
        public T AddComponent<T>() where T : Component, new() { var c = new T(); c.gameObject = this; c.transform = this.transform; components.Add(c); return c; }
        public T[] GetComponentsInChildren<T>(bool includeInactive = true) where T : Component
        {
            var results = new List<T>();
            CollectChildren(this.transform, results);
            return results.ToArray();
        }
        private void CollectChildren<T>(Transform current, List<T> results) where T : Component
        {
            if (current == null) return;
            foreach (var c in components)
                if (c is T t)
                    results.Add(t);
            if (current.children != null)
            {
                foreach (var child in current.children)
                    CollectChildren(child, results);
            }
        }
        public static T Instantiate<T>(T prefab) where T : GameObject { return (T)Activator.CreateInstance(typeof(T))!; }
        public static GameObject Instantiate(GameObject prefab) => new GameObject(prefab?.name ?? "GO");
        public static GameObject Instantiate(GameObject prefab, Transform parent)
        {
            var go = new GameObject(prefab?.name ?? "GO");
            go.transform.parent = parent;
            return go;
        }
        public static GameObject CreatePrimitive(PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.Sphere: return new GameObject("Sphere");
                case PrimitiveType.Capsule: return new GameObject("Capsule");
                case PrimitiveType.Cylinder: return new GameObject("Cylinder");
                case PrimitiveType.Cube: return new GameObject("Cube");
                case PrimitiveType.Plane: return new GameObject("Plane");
                case PrimitiveType.Quad: return new GameObject("Quad");
                default: return new GameObject("Primitive");
            }
        }
        public static void Destroy(GameObject go) {}
        public static void Destroy(GameObject go, float t) {}
    }

    public class Transform : Object
    {
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Transform parent = null;
        public List<Transform> children = new List<Transform>();
        public Vector3 localScale = new Vector3(1,1,1);
        public GameObject gameObject;
        public Vector3 TransformPoint(Vector3 p) => position + p;
        public Vector3 InverseTransformPoint(Vector3 p) => new Vector3(p.x - position.x, p.y - position.y, p.z - position.z);
        public void SetParent(Transform p, bool worldPositionStays = true)
        {
            if (parent != null) parent.children.Remove(this);
            parent = p;
            if (parent != null) parent.children.Add(this);
        }
        public T GetComponent<T>() where T : Component { return gameObject?.GetComponent<T>(); }
        public T[] GetComponentsInChildren<T>(bool includeInactive = true) where T : Component { return gameObject?.GetComponentsInChildren<T>(includeInactive) ?? new T[0]; }
    }

    public class MonoBehaviour : Component { }

    public class Rigidbody : Component {
        public bool useGravity; public bool isKinematic; public Vector3 velocity; public float mass;
    }

    public class Collider : Component {
        public Rigidbody attachedRigidbody;
        public bool isTrigger;
    }

    public class Animator : Component { }

    public class Renderer : Component { public Material material = new Material(); }
    public class Material { public Color color = new Color(1,0,0,1); public Material() {} public Material(Shader shader) {} }

    public struct Color { public float r,g,b,a; public Color(float r,float g,float b,float a){this.r=r;this.g=g;this.b=b;this.a=a;} public static Color black => new Color(0,0,0,1); public static Color Lerp(Color a, Color b, float t) => new Color(a.r*(1-t)+b.r*t, a.g*(1-t)+b.g*t, a.b*(1-t)+b.b*t, a.a*(1-t)+b.a*t); }

    public static class Resources { public static T Load<T>(string path) where T: class { return default; } }

    public static class Time { public static float deltaTime => 0.016f; public static float time => deltaTime; }

    public static class Random
    {
        private static readonly System.Random rnd = new System.Random(0);
        public static Vector3 insideUnitSphere => new Vector3((float)(rnd.NextDouble()-0.5),(float)(rnd.NextDouble()-0.5),(float)(rnd.NextDouble()-0.5));
        public static Vector3 onUnitSphere => insideUnitSphere;
        public static Quaternion rotation => Quaternion.identity;
        public static float Range(float a,float b){ return a + (float)rnd.NextDouble()*(b-a); }
    }

    public static class Mathf
    {
        public static int CeilToInt(float v) => (int)System.Math.Ceiling(v);
        public static float Clamp01(float v) => v<0?0:v>1?1:v;
        public static float Clamp(float v,float a,float b) => v<a?a:v>b?b:v;
        public static float Max(float a,float b) => a>b?a:b;
        public static float Lerp(float a, float b, float t) => a + (b-a)*t;
    }

    public class Collision { public ContactPoint[] contacts = new ContactPoint[0]; public Vector3 relativeVelocity; public GameObject gameObject; public Collider collider; }
    public struct ContactPoint { public Vector3 point; public Vector3 normal; }

    public struct RaycastHit { public Vector3 point; public Vector3 normal; }
    public static class Physics { public static bool Raycast(Vector3 origin, Vector3 dir, out RaycastHit hit, float max) { hit = new RaycastHit(); return false; } }

    public static class JsonUtility
    {
        public static T FromJson<T>(string json) => System.Text.Json.JsonSerializer.Deserialize<T>(json);
        public static string ToJson<T>(T obj, bool pretty = false) => System.Text.Json.JsonSerializer.Serialize(obj);
    }

    
}
