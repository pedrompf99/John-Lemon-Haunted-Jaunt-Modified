using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Ghost;
    [SerializeField] private List<GameObject> SpawnablePlaces;
    [SerializeField] private List<GameObject> UIHearts;
    private List<GameObject> GhostsInScene;
    private bool CanSpawnGhosts = false;
    private int GhostsToSpawn = 1;
    
    private float fadeDuration = 1f;
    private float displayImageDuration = 1f;
    
    [SerializeField] private CanvasGroup winImageCanvasGroup;
    [SerializeField] private CanvasGroup loseImageCanvasGroup;
    [SerializeField] private GameObject door;

    public bool m_playerLostGame, m_playerWonGame;
    float m_Timer;

    private void Start()
    {
        GhostsInScene = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerLostGame)
        {
            LoseGame();
        } else if (m_playerWonGame)
        {
            WinGame();
        } else if (CanSpawnGhosts)
        {
            if(GhostsInScene.Count == 0)
            {
                if (GhostsToSpawn < 4)
                {
                    for(int i = 0; i < GhostsToSpawn; i++)
                    {
                        GameObject SpawnableGhost = Instantiate(Ghost, SpawnablePlaces[Random.Range(0, SpawnablePlaces.Count)].transform);
                        GhostsInScene.Add(SpawnableGhost);
                    }
                    GhostsToSpawn++;
                }
                else
                {
                    door.GetComponent<Animator>().SetBool("open", true);
                    GameObject.Find("JohnLemon").GetComponent<PlayerMovement>().DoorIsOpen();
                    CanSpawnGhosts = false;
                }
                
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

    public void LoseLife(int life)
    {
        UIHearts[life - 1].SetActive(false);
    }

    void WinGame ()
    {
        m_Timer += Time.deltaTime;

        winImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if(m_Timer > fadeDuration + displayImageDuration)
        {
            SceneManager.LoadScene("Demo1");
        }
    }
    
    void LoseGame ()
    {
        m_Timer += Time.deltaTime;

        loseImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if(m_Timer > fadeDuration + displayImageDuration)
        {
            SceneManager.LoadScene("Demo1");
        }
    }
}
