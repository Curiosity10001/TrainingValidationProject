using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderMove : MonoBehaviour
{
    [SerializeField] float speedThunder;

    bool hitPlayer = false;
    bool hitOther = false;

    Animator animator;

    PlayerMove timer;
    float SpawnTime;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        timer = FindObjectOfType<PlayerMove>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("hitPlayer", hitPlayer);
        animator.SetBool("hitOther", hitOther);

        if (timer.timer >= SpawnTime + 30)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.down * speedThunder);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hitPlayer = true;
            Destroy(gameObject);
        }

        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            hitOther = true;
            Destroy(gameObject);
        }
    }
}
