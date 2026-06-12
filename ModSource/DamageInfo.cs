using UnityEngine;

namespace BloodLabMod.Core
{
    public struct DamageInfo
    {
        public Transform Target;
        public Vector3 HitPoint;
        public Vector3 HitNormal;
        public float Damage;
        public string BodyPart;
        public GameObject Source;
    }
}
