using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Ghost;
    [SerializeField]
    private List<GameObject> SpawnablePlaces;
    private List<GameObject> GhostsInScene;
    private bool CanSpawnGhosts = false;
    private int GhostsToSpawn = 1;

    private void Start()
    {
        GhostsInScene = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSpawnGhosts)
        {
            if(GhostsInScene.Count == 0 && GhostsToSpawn < 4)
            {
                for(int i = 0; i < GhostsToSpawn; i++)
                {
                    GameObject SpawnableGhost = Instantiate(Ghost, SpawnablePlaces[Random.Range(0, SpawnablePlaces.Count)].transform);
                    GhostsInScene.Add(SpawnableGhost);
                }
                GhostsToSpawn++;
            }
        }
    }

    public void StartSpawningGhosts()
    {
        CanSpawnGhosts = true;
    }

    public void RemoveGhost(GameObject GhostToRemove)
    {
        GhostsInScene.Remove(GhostToRemove);
    }
}
