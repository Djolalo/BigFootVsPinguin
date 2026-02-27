using UnityEngine;

public class PinguinBehavior : MonoBehaviour
{  
    private UnityEngine.AI.NavMeshAgent m_agent;
    [SerializeField]
    private float attack_distance;
    
    private Transform iglooT;
    private Transform targetT;

    private IglooBehavior iglooBehavior; 


    private float last_chrono;
    [SerializeField]
    private float range_of_stranding;
    [SerializeField]
    private float range_of_detection;
    [SerializeField]
    private float bipolarTime;
    [SerializeField]
    private float range_of_diseaperance;

    private bool triggeredDetection = false;

    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        last_chrono = Time.time;
        this.targetT = GameObject.FindWithTag("Player").transform;
        UpdateTarget();
    }

    public void Init(Transform iglooT,  IglooBehavior iglooBehavior)
    {
        this.iglooT = iglooT; 
        this.iglooBehavior = iglooBehavior;
    }


    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        if (!triggeredDetection)
        {
            bool yeti_Detected = Vector3.Distance(m_agent.transform.position, targetT.position) 
                            < range_of_detection;
            //Le pingouin est trop loin du yeti et le pingouin n'a pas changé de direction depuis trop longtemps
            if( !yeti_Detected && Time.time - last_chrono > bipolarTime)
            {
                last_chrono = Time.time;
                UpdateTarget();
            } 
            //rien à faire du chrono, c'est la guerre
            if(yeti_Detected){
                m_agent.isStopped = false;
                m_agent.destination = iglooT.position;
                triggeredDetection = true;
                transform.LookAt(iglooT);
            }
        }
        else
        {
            if(Vector3.Distance(iglooT.position, transform.position) < range_of_diseaperance)
            {
                iglooBehavior.Wave();
            }
        }
        
    }

    private void UpdateTarget()
    {
        float random_x = Random.Range(-1f,1f)*Mathf.PI;
        Vector3 new_targ_pos = new Vector3(iglooT.position.x + range_of_stranding * Mathf.Cos(random_x),iglooT.position.y,iglooT.position.z + range_of_stranding * Mathf.Sin(random_x));
        m_agent.destination = new_targ_pos;
        transform.LookAt(new_targ_pos);
    }

    public void DiedPinguin()
    {
        isDead = true;
        m_agent.isStopped = true;
        //iglooBehavior.PinguinDied();
        //faire le ragdoll
    }
}