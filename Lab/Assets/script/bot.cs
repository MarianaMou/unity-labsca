
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class bot : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform joueur;
    public LayerMask lesol, lejoueur;

    //patrouille
    public Vector3 pointDePatrouille;
    bool patrouille;
    public float distancePatrouille;

    //attaque
    public float tempsAvantAttaque;
    bool estAttaque;

    //Etat
    public float vison, distanceAttaque;
    public bool joueurDansVision, joueurDansDistanceAttaque;

    //anitation
    Animator animator;

    private void Awake()
    {
        joueur = GameObject.Find("Joueur").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


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
        animator.SetFloat("vitesse", agent.velocity.magnitude); 
    }
    private void Patrouille()
    {
        if (!patrouille) Recherchepatrouille();

        if (patrouille)
        {
            agent.SetDestination(pointDePatrouille);
            //Debug.Log("il bouge");
        }
            

        //calcul la distance des trajets
        Vector3 distanceEntre2Points = transform.position - pointDePatrouille;

        // est arrivé sur le point de patrouille
        if (distanceEntre2Points.magnitude < 1f)
        {
           // Debug.Log("il est arrivé");
            patrouille = false;
        }
            
    }
    private void Recherchepatrouille() 
    {
        //Debug.Log("patpatrouille");
        float randomZ = Random.Range(-distancePatrouille, distancePatrouille);
        float randomX = Random.Range(-distancePatrouille, distancePatrouille);

        pointDePatrouille = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
       // Debug.Log(pointDePatrouille);
        //dans la limite du terrain alors patrouille
        if (Physics.Raycast(pointDePatrouille, -transform.up,2f, lesol))
        {
           // Debug.Log("je verifie");
            patrouille = true;
        }

    }
    private void Poursuite()
    {
        Debug.Log("je suis");
    }
    private void AttaqueJoueur()
    {
        Debug.Log("jatt");
    }

}
