using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Patrol : MonoBehaviour
{

    public Transform[] points;
    private NavMeshAgent agent;
    private GameObject player;
    public float minDistance = 5f;
    public float rotationRadius = 2f;
    private bool isFixedOnPlayer = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        int nextPoint = Random.Range(0, points.Length - 1);

        // Set the agent to go to the currently selected destination.
        if (isFixedOnPlayer)
        {
            agent.destination = points[nextPoint].position;
            isFixedOnPlayer = false;
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                agent.destination = points[nextPoint].position;
        }
        

    }

    void GotoNextPointNearPlayer()
    {
        Vector3 newDestination = player.transform.position + new Vector3(Mathf.Cos(Random.Range(0, 360)) * rotationRadius, 0, Mathf.Sin(Random.Range(0, 360)) * rotationRadius);
        if (!isFixedOnPlayer)
        {
            agent.destination = newDestination;
            isFixedOnPlayer = true;
        } else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
                agent.destination = newDestination;
        }
    }

    void Update()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < minDistance)
        {
            RaycastHit hit;
            Physics.Raycast(this.transform.position, (player.transform.position - this.transform.position).normalized, out hit, Mathf.Infinity);
            if (hit.collider.tag.Equals("Player"))
            {
                this.transform.LookAt(hit.collider.transform);
                GotoNextPointNearPlayer();
                return;
            }
        }
        // Choose the next destination point when the agent gets
        // close to the current one.
        GotoNextPoint();
            

    }
}