using UnityEngine;

public class PinguinWarriorBehavior : MonoBehaviour
{  
    private UnityEngine.AI.NavMeshAgent m_agent;

     private IglooBehavior iglooBehavior; 

    private float attack_distance = 2f;

    private GameObject m_script;
    
    private Transform iglooT;
    private Transform targetT;

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        this.targetT = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(targetT.position, transform.position);
        
        if(distance <= attack_distance)
        {
            m_agent.isStopped = true;
            // attaquer
        } else
        {
            m_agent.isStopped = false;
            m_agent.SetDestination(targetT.position);
        }
    }

    public void init(Transform iglooT, IglooBehavior iglooBehavior)
    {
        this.iglooT = iglooT;
        this.iglooBehavior = iglooBehavior;
    }
}