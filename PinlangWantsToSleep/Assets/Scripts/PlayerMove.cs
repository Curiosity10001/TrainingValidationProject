using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    

    [Header(" Public Parameter for multi-scrip")]
    public float timer = 0;

    #region Player move parameters
    [Header("Movement & Rotation Parameters")]
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 0.5f;
    Vector3 moveDirection;
    Rigidbody rgbd;
    float axisX;
    float axisZ;
    Quaternion rotationLookAt;
    Quaternion turnSpeed;
    #endregion

    #region Jump parameters
    [Header("Jump & Fall parameter")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isJumping = false;
    [SerializeField] float jumpDuration = 0.03f;

    [SerializeField] float downForce = 2.5f;
    [SerializeField] bool isFalling = false;


    float lastJump;
    float startFall;

    #endregion

    #region Floor detection parameters
    [Header("Floor Detection")]
    [SerializeField] bool isGrounded = false;
    [SerializeField] float maxDistance = 20f;
    [SerializeField] Transform frontRightRay;
   

    Vector3 onTheGround;
    RaycastHit touchGround1;
    Ray rayToGround1;
    bool HitGround1;
    #endregion

    #region Animation paarmeters
    [Header("Animation && Animator")]
    public Animator animator;
    #endregion

    #region GameObjectDestroy
    GameOverScript destroy;
    #endregion

    #region Collider of player
    [Header("Collider interractions")]
    Collider playerCollider;
    #endregion

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
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

        //gets inputs for mouvement
        axisZ = Input.GetAxis("Vertical");
        axisX = Input.GetAxis("Horizontal");

        AnimationStateActivation();
        Grounded();
        JumpAndFall();
       
        

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

        //the condition makes it possible for the player to turn while moving toward direction of movement but stay in the last angle registred on Idle
        if(axisX != 0 || axisZ != 0)
        {
            //to rotate body to wanted point relative to camera 
            rotationLookAt = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            rgbd.MoveRotation(rotationLookAt);

            //to regulate the speed of rotation
            turnSpeed = Quaternion.RotateTowards(rgbd.rotation, rotationLookAt, rotationSpeed * Time.fixedDeltaTime);
            rgbd.rotation = turnSpeed;
        }
        
  

    }

    //this is to make sure it fall if no contact with ground
    private void OnCollisionEnter(Collision collision)
    {
        //if Grounded palyer becomes child to plateform : Resolve issue of staying on Moving plateforms
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.parent = collision.gameObject.transform;
            isGrounded = true;
        } 


    }

    private void OnCollisionExit(Collision collision)
    {
        //Resolves the falling and corrects physics - before the player could walk on Air now he falls as he should
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            //Becomes independent GameObject with no parent
            transform.parent = transform.transform;
        }
        
    }

    public void Grounded()
    {
        // Ray to detect Ground
        rayToGround1 = new Ray(frontRightRay.position, Vector3.down);
       

        //Bool to  know that a collider has been hit          
        HitGround1 = Physics.Raycast(rayToGround1, out touchGround1, maxDistance, LayerMask.GetMask("Ground"));
       
        //If  hit get the information of hitposition
        if (HitGround1 )
        {
            onTheGround = touchGround1.point;
        }

        // Rigid body Y position comparison to know if grounded /falling /jumping  (+- 0.1 is tolerance)
        if ((rgbd.position.y <= onTheGround.y + 0.1f && rgbd.position.y >= onTheGround.y - 0.1f) && isGrounded)
        {
            isJumping = false;
            isFalling = false;
        }
        else
        {
            rgbd.AddForce(Vector3.down * downForce);  // chosen Gravity during fall
            isGrounded = false;
        }   
        
    }
  


    void JumpAndFall()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            transform.parent = transform.transform;
            isJumping = true;
            rgbd.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            lastJump = Time.time;
            isGrounded = false;
            isFalling = false;
            
        }

         //Jump +land
        if (rgbd.position.y >= onTheGround.y + 0.2f && timer > lastJump && timer < lastJump + jumpDuration && rgbd.velocity.y != 0)
        {
            transform.parent = transform.transform;
            isJumping = true;
            isFalling = false;
            isGrounded = false;
           
        }

        //Fall+land
        if (((rgbd.position.y >= onTheGround.y + 0.2f  || rgbd.position.y <= onTheGround.y - 0.2f ) && timer > lastJump + jumpDuration && !isJumping))
        {
            transform.parent = transform.transform;
            startFall = Time.time;
            rgbd.AddForce(Vector3.down * downForce);
            isFalling = true;
            isGrounded = false;
            isJumping = false;
        }

        if ((rgbd.position.y <= onTheGround.y  - 60f) || timer >= startFall + 90f && isFalling )
        {
            //destroys self
            destroy.DestroyPlayer(gameObject);
        }


    }

   


}

