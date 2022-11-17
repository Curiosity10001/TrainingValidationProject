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
    [SerializeField] float moveInAir;
    [SerializeField] bool isMoving;
    Vector3 moveDirection;
    Rigidbody rgbd;
    float axisX;
    float axisZ;
    Quaternion rotationLookAt;
    Quaternion turnSpeed;
    
    
    //float parentX;=> deleted was used for obsolete movement code
    //float parentY;=> deleted was used for obsolete movement code
    //float parentZ;=> deleted was used for obsolete movement code
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
    [SerializeField] Transform frontLeftRay;
    [SerializeField] Transform backRightRay;
    [SerializeField] Transform backLeftRay;


    Vector3 onTheGround;
    RaycastHit touchGround1;
    RaycastHit touchGround2;
    RaycastHit touchGround3;
    RaycastHit touchGround4;
    Ray rayToGround1;
    Ray rayToGround2;
    Ray rayToGround3;
    Ray rayToGround4;
    bool HitGround1;
    bool HitGround2;
    bool HitGround3;
    bool HitGround4;
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
        if (isGrounded && !isJumping && !isFalling)
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
        //the condition makes it possible for the player to turn while moving toward direction of movement but stay in the last angle registred on Idle
        if (axisX != 0 || axisZ != 0)
        {
            //to rotate body to wanted point relative to camera 
            rotationLookAt = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            rgbd.MoveRotation(rotationLookAt);

            //to regulate the speed of rotation
            turnSpeed = Quaternion.RotateTowards(rgbd.rotation, rotationLookAt, rotationSpeed * Time.fixedDeltaTime);
            rgbd.rotation = turnSpeed;
        }

        //Direction of movement relative to Camera
        moveDirection = Camera.main.transform.right * axisX + Camera.main.transform.forward * axisZ;

        // As long as nothing impact velocity the rigidbodystays on ground
        moveDirection.y = rgbd.velocity.y;
        if(isMoving)
        {
        //No parent movement
        rgbd.velocity = new Vector3(moveDirection.x * speed, moveDirection.y, moveDirection.z * speed);
        }

        //the condition makes it possible for the player to turn while moving toward direction of movement but stay in the last angle registred on Idle
        if (axisX != 0 || axisZ != 0)
        { 
            //to rotate body to wanted point relative to camera 
            rotationLookAt = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            rgbd.MoveRotation(rotationLookAt);

            //to regulate the speed of rotation
            turnSpeed = Quaternion.RotateTowards(rgbd.rotation, rotationLookAt, rotationSpeed * Time.fixedDeltaTime);
            rgbd.rotation = turnSpeed;
        }

        #region Obsolete movement code and Reason
        //* Tried another way to correct movement on moving plateform : did not work properly for intended
        /* GetComponentInParent<Rigidbody>().velocity = new Vector3(parentX, parentY, parentZ);

         //Condition to be able to stay  unmoving on moving plateform
         if ((parentX != 0 || parentZ != 0) && axisX == 0 && axisZ == 0)
         {
             rgbd.velocity = GetComponentInParent<Rigidbody>().velocity;
         }

         //Condition to have correct movement on static plateform
         else if (parentX == 0 && parentZ == 0)
         {
             // rgbd velocity 
             rgbd.velocity = new Vector3(moveDirection.x * speed, moveDirection.y, moveDirection.z * speed);
         }
         //Condition to have correct movement on moving plateform
         else if (parentX != 0 && parentZ != 0 && isGrounded)
         {
             rgbd.velocity = new Vector3(moveDirection.x * speed,moveDirection.y, moveDirection.z * speed) + GetComponentInParent<Rigidbody>().velocity;
         }*/
        #endregion

    }

    //this is to make sure it fall if no contact with ground
    private void OnCollisionStay(Collision collision)
    {
        //if Grounded transform of player becomes child to plateform : Resolve issue of staying on Moving plateforms
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.SetParent(collision.gameObject.transform, true);
            isGrounded = true;
            isMoving = true;
        }


    }


    private void OnCollisionExit(Collision collision)
    {
        //Resolves the falling and corrects physics - before the player could walk on Air now he falls as he should
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            //Becomes independent GameObject with no parent
             transform.SetParent(null, true);
        }

    }

    public void Grounded()
    {
        // Ray to detect Ground
        rayToGround1 = new Ray(frontRightRay.position, Vector3.down);
        rayToGround2 = new Ray(frontLeftRay.position, Vector3.down);
        rayToGround3 = new Ray(backRightRay.position, Vector3.down);
        rayToGround4 = new Ray(backLeftRay.position, Vector3.down);

        //Bool to  know that a collider has been hit 
        HitGround1 = Physics.Raycast(rayToGround1, out touchGround1, maxDistance, LayerMask.GetMask("Ground"));
        HitGround2 = Physics.Raycast(rayToGround2, out touchGround2, maxDistance, LayerMask.GetMask("Ground"));
        HitGround3 = Physics.Raycast(rayToGround3, out touchGround3, maxDistance, LayerMask.GetMask("Ground"));
        HitGround4 = Physics.Raycast(rayToGround4, out touchGround4, maxDistance, LayerMask.GetMask("Ground"));

        //If  hit get the information of hitposition and get middle position
        if (HitGround1 && HitGround2 && HitGround3 && HitGround4)
        {
            onTheGround = (touchGround1.point + touchGround2.point + touchGround3.point + touchGround4.point) / 4;
        }


        // Rigid body Y position comparison to know if grounded /falling /jumping  (+- 0.1 is tolerance)
        if ((rgbd.position.y <= onTheGround.y + 0.2f && rgbd.position.y >= onTheGround.y - 0.2f))
        {
            
            isJumping = false;
            isFalling = false;
        }
        else
        {
            isGrounded = false;
        }

    }



    void JumpAndFall()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //transform.SetParent(null, true);=> deleted was used for obsolete movement code
            isJumping = true;
            isMoving = true;
            rgbd.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            lastJump = Time.time;
            isGrounded = false;
            isFalling = false;

        }


        //Jump +land (can move in air for jump duration)
        if (rgbd.position.y >= onTheGround.y + 0.2f && timer > lastJump && timer < lastJump + jumpDuration && rgbd.velocity.y != 0)
        {
            // transform.SetParent(null, true);=> deleted was used for obsolete movement code
            isJumping = true;
            isMoving = true;
            isFalling = false;
            isGrounded = false;
            rgbd.AddForce(Vector3.down * downForce);

        }

        //
        if (rgbd.position.y >= onTheGround.y + 0.3f && timer > lastJump && timer > lastJump + jumpDuration + moveInAir && rgbd.velocity.y != 0)
        {
            // transform.SetParent(null, true);=> deleted was used for obsolete movement code
            isJumping = false;
            isMoving = false;
            isFalling = false;
            isGrounded = true;
            rgbd.AddForce(Vector3.down * downForce);

        }

        //Fall+land can move
        if ((rgbd.position.y >= onTheGround.y + 0.3f || rgbd.position.y <= onTheGround.y - 0.3f) && timer > lastJump + jumpDuration && !isJumping && timer <= startFall+moveInAir )
        {
            //transform.SetParent(null, true);=> deleted was used for obsolete movement code
            isMoving = true;
            startFall = Time.time;
            rgbd.AddForce(Vector3.down * downForce);
            isFalling = true;
            isGrounded = false;
            isJumping = false;
        }

        //Fall+land cannot move
        if (((rgbd.position.y >= onTheGround.y + 0.3f || rgbd.position.y <= onTheGround.y - 0.3f) && timer > lastJump + jumpDuration && !isJumping && timer >= startFall + moveInAir))
        {
            //transform.SetParent(null, true);=> deleted was used for obsolete movement code
            isMoving = false;
            startFall = Time.time;
            rgbd.AddForce(Vector3.down * downForce);
            isFalling = true;
            isGrounded = false;
            isJumping = false;
        }

        if ((rgbd.position.y <= onTheGround.y - 60f) || timer >= startFall + 60f && isFalling)
        {
            //destroys self
            destroy.DestroyPlayer(gameObject);
        }


    }




}

