
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class bot : MonoBehaviour
{
    StarterAssets.FirstPersonController p1;
    Hybrid8Test freq;
    
    public NavMeshAgent agent;
    public Transform joueur;
    public LayerMask lesol, lejoueur;

    //patrouille
    public Vector3 pointDePatrouille;
    bool patrouille;
    public float distancePatrouille;
    public float zoneDePatrouille;

    //attaque
    public float tempsAvantAttaque;
    bool estAttaque;

    //Etat
    public float vison, distanceAttaque;
    public bool joueurDansVision, joueurDansDistanceAttaque;

    //anitation
    Animator animator;

    public bool perdu = false;
    public GameObject lecan;
    public GameObject joueur1;
    public GameObject bital;
    public bool enAttack = false;
    public bool finAppraisal = false;
    [SerializeField]
    private AudioSource marcheSon;
    [SerializeField]
    private AudioSource attackSon;


    private void Awake()
    {
        joueur = GameObject.Find("Joueur").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lecan.SetActive(false);
        p1 = joueur1.GetComponent<StarterAssets.FirstPersonController>();
        freq = bital.GetComponent<Hybrid8Test>();
        //marcheSon = GetComponent<AudioSource>();


    }
    private void Update()
    {
        
        // joueur distance d'attaque et vision
        joueurDansVision = Physics.CheckSphere(transform.position, vison, lejoueur);
        joueurDansDistanceAttaque = Physics.CheckSphere(transform.position, distanceAttaque, lejoueur);

        //Debug.Log(joueur.position);

        //agent.destination = joueur.position;

        if (!joueurDansVision && !joueurDansDistanceAttaque) Patrouille();
        if (joueurDansVision && !joueurDansDistanceAttaque) Poursuite();
        if (joueurDansVision && joueurDansDistanceAttaque) AttaqueJoueur();

        //animer
        if (!freq.stresser)
        {
            animator.SetFloat("vitesse", agent.velocity.magnitude);
            
        }
        else
        {
            animator.SetFloat("vitesse", (agent.velocity.magnitude * 1.4f));
           // Debug.Log("vitesse stresser"+(float)(agent.velocity.magnitude + (agent.velocity.magnitude * 1.4f)));
        }
        
        //Debug.Log("nom" + agent.name + "vitesse" + agent.velocity.magnitude);

    }
    private void Patrouille()
    {
 
        if (!patrouille) Recherchepatrouille();

        if (patrouille)
        {
            agent.SetDestination(pointDePatrouille);
            //FindObjectOfType<AudioManager>().Play("marcheb");
            if (!marcheSon.isPlaying)
            {
                marcheSon.Play();
            }
           
            //Debug.Log("il bouge "+agent.name);
        }
            

        //calcul la distance des trajets
        Vector3 distanceEntre2Points = transform.position - pointDePatrouille;

        // est arrivé sur le point de patrouille
        if (distanceEntre2Points.magnitude < 1f)
        {
            Debug.Log("il est arrivé"+agent.name);
            patrouille = false;
        }
            
    }
    private void Recherchepatrouille() 
    {
        
        //Debug.Log("patpatrouille");
        float randomZ = Random.Range(-distancePatrouille, distancePatrouille);
        float randomX = Random.Range(-distancePatrouille, distancePatrouille);

        if (agent.name != "Foxp")
        {
            pointDePatrouille = new Vector3(randomX, transform.position.y, randomZ);
            Debug.Log("patpatrouille Pas foxp"+pointDePatrouille);

        }

        if (p1.Popup && agent.name == "Foxp")
        {
            Debug.Log("je suis foxp et jattack");
            agent.transform.position = new Vector3(-12f,-4.75f,8.5f);
            pointDePatrouille = new Vector3(4f, 1f, 12f);
            //Debug.Log("popup " + p1.Popup + "nom " + agent.name + "la pos " + agent.transform.position);
            p1.Popup = false;
            enAttack = true;
        }
        else if (agent.name == "Foxp" && enAttack)
        {
            Debug.Log("patpatrouille foxp");
            finAppraisal=true;
            // Debug.Log("------popup " + p1.Popup + "nom " + agent.name + "la pos " + agent.transform.position);
            pointDePatrouille = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        }

       


        // Debug.Log(pointDePatrouille);
        //dans la limite du terrain alors patrouille
        if (Physics.Raycast(pointDePatrouille, -transform.up,2f, lesol) && pointDePatrouille.x>-zoneDePatrouille && pointDePatrouille.x < zoneDePatrouille && pointDePatrouille.z > -zoneDePatrouille && pointDePatrouille.z < zoneDePatrouille)
        {
           // Debug.Log("je verifie");
            patrouille = true;
        }

    }
    private void Poursuite()
    {
        agent.SetDestination(joueur.position);
       // Debug.Log("je suis");
    }
    private void AttaqueJoueur()
    {
        // Debug.Log("jatt");
        if (!perdu)
        {
            attackSon.Play();
            perdu = true;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //lecan.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            perdu = true;
           // Debug.Log("collision");
            lecan.SetActive(true);
        }
    }

}
