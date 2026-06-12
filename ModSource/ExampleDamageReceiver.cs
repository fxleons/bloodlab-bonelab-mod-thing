using UnityEngine;

namespace BloodLabMod.Core
{
    // Example component showing how a weapon or bullet can notify the mod
    public class ExampleDamageReceiver : MonoBehaviour
    {
        public Transform target;

        // Call this to simulate a hit for testing in the editor
        public void SimulateHit(Vector3 worldPos, Vector3 normal, float damage)
        {
            if (target == null) return;
            var bodyPart = BodyPartIdentifier.Identify(target, transform);
            HookManager.OnEntityHit(target, worldPos, normal, damage, bodyPart, this.gameObject);
        }
    }
}
