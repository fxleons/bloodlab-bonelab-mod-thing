using UnityEngine;

namespace BloodLabMod.Core
{
    // Component controlling a single droplet's physics and lifecycle
    public class BloodDroplet : MonoBehaviour
    {
        private Rigidbody rb;
        public float Volume { get; private set; }
        public Vector3 LastNormal { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsSettled { get; private set; }

        public void Initialize(Vector3 initialVelocity, float volume)
        {
            if (rb == null) rb = gameObject.GetComponent<Rigidbody>();
            if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.velocity = initialVelocity;
            rb.mass = Mathf.Clamp(volume*0.1f, 0.01f, 5f);
            Volume = Mathf.Clamp(volume, 0.001f, 1f);
            IsSettled = false;
            LastNormal = Vector3.up;
        }

        public void Simulate(float dt)
        {
            // simple physics handled by Rigidbody; check for low velocity to settle
            if (rb == null) return;
            if (!IsSettled && rb.velocity.sqrMagnitude < 0.01f && transform.position.y < 10f)
            {
                // settle
                IsSettled = true;
                // try to get collision normal from last contact via a short spherecast
                if (Physics.Raycast(transform.position, -Vector3.up, out var hit, 0.5f))
                    LastNormal = hit.normal;
                rb.isKinematic = true;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.contacts.Length > 0)
                LastNormal = collision.contacts[0].normal;
            // bounce slightly
            if (rb != null)
            {
                rb.velocity = rb.velocity * 0.3f + collision.contacts[0].normal * -0.2f;
            }
        }
    }
}
