using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{

    void OnTriggerEnter (Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().m_playerWonGame = true;
        }
    }
}
