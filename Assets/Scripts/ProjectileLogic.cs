using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (this.tag.Equals("ProjectilePlayer") && other.tag.Equals("Ghost"))
        {
            other.GetComponent<Patrol>().Die();
            Destroy(this.gameObject);
        }
        if(this.tag.Equals("ProjectileGhost") && other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerMovement>().CheckInPlayerHit(this.gameObject);
        }
    }
}
