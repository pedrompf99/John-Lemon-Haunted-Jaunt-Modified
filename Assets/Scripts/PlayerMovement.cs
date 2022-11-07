using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 20f;
    [SerializeField]
    private GameObject UIText;
    [SerializeField]
    private GameObject pickup_pan;
    [SerializeField]
    private GameObject pickup_vfx;
    [SerializeField]
    private GameObject player_pan;

    private TextMesh UITextMesh;
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Vector3 m_Movement;
    private Quaternion m_Rotation = Quaternion.identity;
    private bool has_pan = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody> ();
        UITextMesh = UIText.GetComponent<TextMesh>();
    }
    private void Update()
    {
        if (has_pan)
        {
            m_Animator.SetBool("IsAttacking", Input.GetMouseButton(0));
        } else
        {
            if(Vector3.Distance(this.transform.position, pickup_pan.transform.position) < 1.5f)
            {
                UIText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    has_pan = true;
                    Destroy(pickup_pan);
                    player_pan.SetActive(true);
                    UIText.SetActive(false);
                    pickup_vfx.SetActive(true);
                }
            } else
            {
                UIText.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();
        
        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        
        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }
    
    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }

    void PanReflection()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("ProjectileGhost");
        foreach(GameObject projectile in projectiles)
        {
            if (Vector3.Distance(player_pan.transform.position, projectile.transform.position) < 1.5f)
            {
                projectile.GetComponent<Rigidbody>().velocity = this.transform.forward * 3;
                var main = projectile.GetComponent<ParticleSystem>().main;
                main.startColor =
                    new ParticleSystem.MinMaxGradient(Color.red, Color.red);
                projectile.tag = "ProjectilePlayer";
            }
        }
    }
}
