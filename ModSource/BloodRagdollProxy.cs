using UnityEngine;

namespace BloodLabMod.Core
{
    // Proxy attached to ragdoll colliders to detect hits without altering physics.
    public class BloodRagdollProxy : MonoBehaviour
    {
        private Transform rootTransform;
        private const float minVelocityForImpact = 0.5f;

        public void Initialize(Transform root)
        {
            rootTransform = root;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (rootTransform == null) return;
            if (collision.contacts.Length == 0) return;

            var contact = collision.contacts[0];
            var hitPoint = contact.point;
            var hitNormal = contact.normal;
            var hitVelocity = collision.relativeVelocity.magnitude;
            var bodyPart = BodyPartIdentifier.Identify(rootTransform, transform);
            var damage = Mathf.Clamp(hitVelocity * 5f, 0.5f, 8f);

            if (hitVelocity < minVelocityForImpact) return;
            TriggerRagdollBlood(hitPoint, hitNormal, damage, bodyPart, collision.gameObject);
        }

        private void TriggerRagdollBlood(Vector3 hitPoint, Vector3 hitNormal, float damage, string bodyPart, GameObject source)
        {
            var targetEntity = rootTransform;
            HookManager.OnEntityHit(targetEntity, hitPoint, hitNormal, damage, bodyPart, source);
        }
    }
}
