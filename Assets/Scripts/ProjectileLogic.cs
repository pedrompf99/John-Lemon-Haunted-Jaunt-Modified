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
        }
    }
}
