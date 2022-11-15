using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardBestowment : MonoBehaviour
{
    #region Animation parameters
    [Header("Animation && Animator")]
    Animator animator;
    #endregion

    #region Movement parameters
    [Header("Movement")]
    [SerializeField] float speedCaptured = 4.5f;
    [SerializeField] float speedFree = 5f;
    [SerializeField] float deltaTimeDirectionchange = 5f;
    Transform player;


    bool captured = false;
    float time;
    PlayerMove timer;
    Vector3 randomPos;
    Vector3 initPos;
    Transform parent;
    #endregion


    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        timer = FindObjectOfType<PlayerMove>();
        parent = GetComponentInParent<Transform>();
        initPos = parent.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        freeMove();

        //Condition to follow player = Capture
        if (captured)
        {
            PlayerFollow();
        }

        
    }

    void freeMove()
    {
        // Every deltatimedirectionChange we generate new position to go to
        if (timer.timer >= time + deltaTimeDirectionchange && !captured)
        {
            time = Time.time;

            //Sphere of 2m Unity
            randomPos = Random.insideUnitSphere * 2;

            //Constraints height movement to less than radius of sphere
            randomPos.y = Random.Range(-0.3f,0.5f); 
        }
        gameObject.transform.Translate(((randomPos + initPos ) - parent.transform.position).normalized * speedFree * Time.deltaTime, Space.World);

    }
    private void PlayerFollow()
    {
        //To make it follow player
        gameObject.transform.Translate((player.transform.position + parent.transform.position).normalized * speedCaptured * Time.deltaTime, Space.World);
    }
    void OnCollisionEnter(Collision collision)
    {
        //Trigger capture and captured animation
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            captured = true;
            animator.SetBool("Captured", captured);
        }
        
    }
    
}
