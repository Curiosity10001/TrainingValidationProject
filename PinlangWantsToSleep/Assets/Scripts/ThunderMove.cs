using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderMove : MonoBehaviour
{
    bool hitPlayer = false;
    bool hitOther = false;

    PlayerMove timer;
    float SpawnTime;
    float hit;

    GameObject explosion;
    Explosion newPos;
    Rigidbody rgbdThunder;

    private void Awake()
    {
        newPos = FindObjectOfType<Explosion>();
        explosion = GameObject.Find("Explosion");
        timer = FindObjectOfType<PlayerMove>();
        rgbdThunder = GetComponent<Rigidbody>();


        if (explosion.activeInHierarchy)
        {
            explosion.SetActive(false);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ( hitOther )
        {
            if(timer.timer >= hit + 15)
            {
            Destroy(gameObject);
            }                     
        } else if (hitPlayer)
        {
            if(timer.timer >= hit + 5)
            {
                newPos.NewTransform(gameObject.transform);
                explosion.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {  
            hit = Time.time;
            hitPlayer = true; 
        }

        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {          
            hit = Time.time;
            hitOther = true;        
        }

        Debug.Log("hitOther" + hitOther + "hit" + hit);
    }
}
