using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Vector3 moveDirection;
    Rigidbody rgbd;

    [Header(" Public Parameter for multi-scrip")]
    public float timer = 0;

    [Header("Movement & Rotation Parameters")]
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 0.5f;

    float axisX;
    float axisZ;
    Quaternion rotationLookAt;
    Quaternion turnSpeed;


    [Header("Jump & Fall parameter")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isJumping = false;
    [SerializeField] float jumpDuration = 0.03f;

    [SerializeField] float downForce = 2.5f;
    [SerializeField] bool isFalling = false;


    float lastJump;
    float startFall;



    [Header("Floor Detection")]
    [SerializeField] bool isGrounded = false;
    [SerializeField] float maxDistance = 20f;
    [SerializeField] Transform frontRightRay;
   

    Vector3 onTheGround;
    RaycastHit touchGround1;
    Ray rayToGround1;
    bool HitGround1;
    


    [Header("Animation && Animator")]
    public Animator animator;

    #region GameObjectDestroy
    GameOverScript destroy;
    #endregion



    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
        timer += Time.deltaTime;
        destroy = FindObjectOfType<GameOverScript>();

    }
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        axisZ = Input.GetAxis("Vertical");
        axisX = Input.GetAxis("Horizontal");

        AnimationStateActivation();
        Grounded();
        JumpAndFall();
        Debug.Log("on the ground value " + onTheGround .y);
        Debug.Log("pinlang y value " + moveDirection.y);
        

    }
    void AnimationStateActivation()
    {
        //Axis values to know if jog or run if playing with gamepad (values are 0/1 if on keyboard so run only)
        animator.SetFloat("X", axisX);
        animator.SetFloat("Y", axisZ);

        //Pre existing bools to reuse for animator
        animator.SetBool("isFalling", isFalling);
        


        // Axis state for Idle/Moving when not jumping
        if (isGrounded && !isJumping && !isFalling )
        {
            animator.SetBool("isMoving", true);
        }

        // to prevent  Idle/Moving when Falling
        if (isFalling)
        {
            animator.SetBool("isMoving", false);
        }
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isJumping");
        }
    }
    private void FixedUpdate()
    {
        //Direction of movement relative to Camera
        moveDirection = Camera.main.transform.right * axisX + Camera.main.transform.forward*axisZ;

        // As long as nothing impact velocity the rigidbodystays on ground
        moveDirection.y = rgbd.velocity.y;

        // rgbd velocity 
        rgbd.velocity = new Vector3(moveDirection.x * speed, moveDirection.y, moveDirection.z * speed) ;
        Debug.Log("rgbd y position " + rgbd.position.y);

        //the condition makes it possible for the player to turn while moving toward direction of movement but stay in the last angle registred on Idle
        if(rgbd.velocity.x != 0 || rgbd.velocity.y != 0)
        {
            //to rotate body to wanted point relative to camera 
            rotationLookAt = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            rgbd.MoveRotation(rotationLookAt);

            //to regulate the speed of rotation
            turnSpeed = Quaternion.RotateTowards(rgbd.rotation, rotationLookAt, rotationSpeed * Time.fixedDeltaTime);
            rgbd.rotation = turnSpeed;
        }
        
  

    }

    public void Grounded()
    {
        // Rays to detect Ground
        rayToGround1 = new Ray(frontRightRay.position, Vector3.down);
      

        //Bool to  know that a collider has been hit 
        HitGround1 = Physics.Raycast(rayToGround1, out touchGround1, maxDistance, LayerMask.GetMask("Ground"));
       
        //If  hit get the information of the center of the 4 points : it will be our comparison point to know if we are grounded
        if (HitGround1 )
        {
            onTheGround = touchGround1.point;
        }

        // Rigid body Y position comparison to know if grounded /falling /jumping  (+- 0.1 is tolerance)
        if (rgbd.position.y <= onTheGround.y + 0.1f && rgbd.position.y >= onTheGround.y  - 0.1f)
        {
            isGrounded = true;
            isJumping = false;
            isFalling = false;
        }
        else
        {
            rgbd.AddForce(Vector3.down * downForce);  
            isGrounded = false;
        }   
        
    }
    void JumpAndFall()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            rgbd.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            lastJump = Time.time;
            isGrounded = false;
            isFalling = false;
            
        }

         //Jump +land
        if (rgbd.position.y >= onTheGround.y + 0.2f && timer > lastJump && timer < lastJump + jumpDuration && rgbd.velocity.y != 0)
        {
            isJumping = true;
            isFalling = false;
            isGrounded = false;
           
        }

        //Fall+land
        if (((rgbd.position.y >= onTheGround.y + 0.2f  || rgbd.position.y <= onTheGround.y - 0.2f ) && timer > lastJump + jumpDuration && !isJumping))
        {
            startFall = Time.time;
            rgbd.AddForce(Vector3.down * downForce);
            isFalling = true;
            isGrounded = false;
            isJumping = false;
        }

        if ((rgbd.position.y <= onTheGround.y  - 15f) || timer >= startFall + 40f && isFalling )
        {
            //destroys self
            destroy.DestroyPlayer(gameObject);
        }


    }

   
    

}

