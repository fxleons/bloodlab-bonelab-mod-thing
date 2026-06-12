using UnityEngine;

namespace BloodLabMod.Core
{
    public class BloodHitSource : MonoBehaviour
    {
        private BonelabDamageIntegration integration;
        private Collider selfCollider;
        private Rigidbody selfRigidbody;

        public void Initialize(BonelabDamageIntegration owner)
        {
            integration = owner;
            selfCollider = GetComponent<Collider>();
            selfRigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (integration == null || collision.contacts.Length == 0) return;
            if (selfRigidbody == null) return;
            if (collision.collider == null) return;
            if (collision.collider.attachedRigidbody == selfRigidbody) return;

            var contact = collision.contacts[0];
            var hitPoint = contact.point;
            var hitNormal = contact.normal;
            var relativeVelocity = selfRigidbody.velocity - (collision.collider.attachedRigidbody != null ? collision.collider.attachedRigidbody.velocity : Vector3.zero);
            integration.ProcessHit(selfCollider, collision.collider, hitPoint, hitNormal, relativeVelocity, gameObject);
        }
    }
}
