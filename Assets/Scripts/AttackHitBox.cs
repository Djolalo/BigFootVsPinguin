using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float force = 500f;

    private void OnTriggerEnter(Collider other)
    {
        // 🐧 PINGOUIN
        if (other.CompareTag("Pinguin"))
        {
            Rigidbody rb = other.attachedRigidbody;

            if (rb != null)
            {
                Vector3 direction = (other.transform.position - transform.root.position).normalized;
                Vector3 knockback = (direction + Vector3.up).normalized;

                rb.AddForce(knockback * force, ForceMode.Impulse);
            }

            // Appel Die() si présent
            if (other.TryGetComponent<PinguinWarriorBehavior>(out var warrior))
            {
                Vector3 direction = other.transform.position - transform.root.position;
                warrior.Die(direction);
            }
            else if (other.TryGetComponent<PinguinBehavior>(out var basic))
            {   
                Vector3 direction = other.transform.position - transform.root.position;
                basic.Die(direction);
            }

            return;
        }

        // 🏠 IGLOO
        if (other.CompareTag("Igloo"))
        {
            if (other.TryGetComponent<IglooBehavior>(out var igloo))
            {
                igloo.Destroyed();
            }
        }
    }
}