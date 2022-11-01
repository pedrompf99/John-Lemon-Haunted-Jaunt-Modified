using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class Patrol : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private GameObject death_vfx;
    [SerializeField]
    private GameObject projectile_vfx;
    private NavMeshAgent agent;
    private GameObject player;
    public float minDistance = 5f;
    public float rotationRadius = 2f;
    private bool isFixedOnPlayer = false;
    private float angle;


    void Start()
    {
        angle = Random.Range(0, 360);
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (isFixedOnPlayer)
            {
                this.GetComponent<Animator>().SetBool("IsShooting", true);
                yield return new WaitForSeconds(.5f);
                this.GetComponent<Animator>().SetBool("IsShooting", false);
                yield return new WaitForSeconds(Random.Range(2,8));
            }
            yield return new WaitForSeconds(Random.Range(1,2));
        }
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
        if (!isFixedOnPlayer)
        {
            Vector3 newDestination = player.transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * rotationRadius, 0, Mathf.Sin(Mathf.Deg2Rad * angle) * rotationRadius);
            agent.destination = newDestination;
            isFixedOnPlayer = true;
        } else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                angle = (angle + Random.Range(-60, 60)) % 360;
                Vector3 newDestination = player.transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * rotationRadius, 0, Mathf.Sin(Mathf.Deg2Rad * angle) * rotationRadius);
                agent.destination = newDestination;
            }
        }
    }

    void ShootPlayer()
    {
        GameObject shot = GameObject.Instantiate(projectile_vfx, this.transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        shot.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position).normalized * 3, ForceMode.Impulse);
    }

    void Update()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < minDistance)
        {
            RaycastHit hit;
            Physics.Raycast(this.transform.position, (player.transform.position - this.transform.position).normalized, out hit, Mathf.Infinity);
            if (hit.collider != null && hit.collider.tag.Equals("Player"))
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