using System.Collections.Generic;
using UnityEngine;

public class IglooBehavior : MonoBehaviour
{

    public GameObject pinguin_warriors;

    private List<GameObject> pinguins = new List<GameObject>();
    [SerializeField]
    private GameObject pingu_sentry; 

    // Start is called before the first frame update
    [SerializeField]
    private int nb_vague = 0;

    private int nb_pinguin = 8;

    private bool SendWave = false;

    [SerializeField]
    private int nbSentinels;
    
    [SerializeField]
    private Vector3 offset_entry = new Vector3(0,0,-5);

    private bool WaveAlreadyTriggered = false;

    void Awake()
    {
        if(pinguins == null) pinguins = new List<GameObject>();
    }
    void Start()
    {
        //creating pingus at the start
        for(int i = 0 ; i<nbSentinels; i++){
            SpawnSentry();
        } 
    }

    // Update is called once per frame
    void Update()
    { 
        if(pinguins == null) return;

        // Uni tyEngine.Debug.Log(pinguins.Count);
        if (SendWave && pinguins.Count == 0)
        {
            int i = 0;
            nb_pinguin = (nb_vague == 0 )? nb_pinguin: nb_pinguin << 1;
            while( i++ < nb_pinguin)
            {
                SpawnWarrior();
            }
            nb_vague ++;
            if(nb_vague > 3)
            {
                SendWave = false;
                nb_vague = 0;
            }
        }
    }

    private void SpawnSentry()
    {
        GameObject pingu = (GameObject)Instantiate(pingu_sentry, this.transform.position + offset_entry, this.transform.rotation);
        PinguinBehavior pb = pingu.GetComponent<PinguinBehavior>();
        pb.Init(this.transform, this);
        pinguins.Add(pingu);

    }

    private void SpawnWarrior()
    {
        GameObject pingu = (GameObject)Instantiate(pinguin_warriors, this.transform.position + offset_entry, this.transform.rotation);
        PinguinWarriorBehavior pb = pingu.GetComponent<PinguinWarriorBehavior>();
        pb.Init(this.transform, this);
        pinguins.Add(pingu);

    }

    public void PinguinDied(GameObject pinguin)
    {
        if(pinguins.Contains(pinguin)) pinguins.Remove(pinguin);
    }

    public void Wave()
    {
        if (!WaveAlreadyTriggered)
        {
            WaveAlreadyTriggered = true;
            foreach(GameObject pingu in pinguins)
            {
                Destroy(pingu);
            }
            pinguins.Clear();
            SendWave = true;
        }

    }
}
