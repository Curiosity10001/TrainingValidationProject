using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderMove : MonoBehaviour
{
    bool hitPlayer = false;
    float thunderBirth;
    [SerializeField] float thunderLifeTime = 30;
    float playerHit;
    [SerializeField] float speed = 5f;
    Transform player;
    GameObject explosion;
    


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        thunderBirth = Time.time;
        explosion = GetComponentInChildren<GameObject>(true);
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= thunderBirth + thunderLifeTime)
        {
            if (!hitPlayer)
            {
                Destroy(gameObject);
            }
        }

        if (hitPlayer)
        {
            if(Time.time>= playerHit +0.05f)
            {
                FollowPlayer();
                explosion.SetActive(true);
            }

            if(Time.time >= playerHit + 0.5f)
            {
                Destroy(gameObject);
            }
        }

    }
    private void FixedUpdate()
    {
       
    }
    void FollowPlayer()
    {
        gameObject.transform.Translate( (gameObject.transform.position - player.position).normalized * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          
            hitPlayer = true;
            playerHit = Time.time;
        }

    }
   
}
