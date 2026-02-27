using UnityEngine;

public class PinguinWarriorBehavior : MonoBehaviour
{  
    private UnityEngine.AI.NavMeshAgent m_agent;

     private IglooBehavior iglooBehavior; 
     private Rigidbody rb;

    [SerializeField] private float attack_distance = 3f;

    [SerializeField] private float deathForce = 1f;
    [SerializeField] private float upwardForce = 2f;

    private GameObject m_script;
    
    private Transform iglooT;
    private Transform targetT;

    private bool isDead = false;

    [SerializeField] private float attack_damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        this.targetT = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;

        transform.LookAt(targetT);
        float distance = Vector3.Distance(targetT.position, transform.position);
        
        if(distance <= attack_distance)
        {
            m_agent.isStopped = true;
            Attack();
        } else
        {
            m_agent.isStopped = false;
            m_agent.SetDestination(targetT.position);
        }
    }

    public void Init(Transform iglooT, IglooBehavior iglooBehavior)
    {
        this.iglooT = iglooT;
        this.iglooBehavior = iglooBehavior;
    }

    public void Die(Vector3 hitDirection)
    {
        if (isDead) return;
        isDead = true;

        // Stop IA
        m_agent.isStopped = true;
        m_agent.enabled = false;

        // Activer la physique
        rb.isKinematic = false;

        // Optionnel : désactiver collider si tu veux éviter bug
        // col.enabled = false;
        iglooBehavior.PinguinDied(this.gameObject);

        // Appliquer projection
        Vector3 force = (hitDirection.normalized * deathForce) + (Vector3.up * upwardForce);
        rb.AddForce(force, ForceMode.Impulse);

        // Rotation aléatoire pour effet dramatique
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        // Détruire après 3 secondes
        //Destroy(gameObject, 3f);
    }

    public void Attack()
    {
        //animation d'attaque
        // yeti.isAttack(attackDamage)
    }
}