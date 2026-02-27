using UnityEngine;

public class PinguinWarriorBehavior : MonoBehaviour
{  
    private UnityEngine.AI.NavMeshAgent m_agent;

     private IglooBehavior iglooBehavior; 

    [SerializeField] private float attack_distance = 3f;

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

    public void DiedPinguin()
    {
        isDead = true;
        m_agent.isStopped = true;
        //iglooBehavior.PinguinDied();
        //faire le ragdoll
    }

    public void Attack()
    {
        //animation d'attaque
        // yeti.isAttack(attackDamage)
    }
}