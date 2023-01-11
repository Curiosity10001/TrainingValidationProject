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
    [SerializeField] float speedFree = 5f;
    [SerializeField] float deltaTimeDirectionchange = 5f;

    ScoreManager sleepyLangScore;
    AudioSource bell;

    public bool captured = false;
   

    float time;
    PlayerMove timer;
    Vector3 randomPos;
   
    #endregion


    private void Awake()
    {
        sleepyLangScore = FindObjectOfType<ScoreManager>();
        animator = GetComponent<Animator>();
        timer = FindObjectOfType<PlayerMove>();
        parent = GetComponentInParent<Transform>();
        initPos = parent.transform.position;
        bell = GetComponent<AudioSource>();
    }
    Vector3 initPos;
    Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!captured)
        {
            freeMove();
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
        //Gives the direction of translation
        gameObject.transform.Translate(((randomPos + initPos ) - parent.transform.position).normalized * speedFree * Time.deltaTime, Space.World);

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
    private void OnDestroy()
    {
        sleepyLangScore.Sleepiness();
    }

    public void PlayBell()
    {
        bell.Play();
    }
}
