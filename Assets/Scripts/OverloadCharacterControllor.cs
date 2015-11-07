using UnityEngine;
using System.Collections;



public class OverloadCharacterControllor : MonoBehaviour {
    

    // Speed    
    private const float MAX_SPEED = 5f;        // How fast I can go
    [SerializeField]
    private float currentSpeed = 0.0f;         // How fast I am going
    [SerializeField]
    private float acceleration = 1.0f;         // How fast I accelerate

    [SerializeField]
    private float jumpSpeed = 20.0f;           // How fast I jump
   // private float jumpVelocity = 0.0f;         // Capture vertical speed
    [SerializeField]
    private float gravity = 9.8f;             // How fast I fall


    const float LOWEST_FLOOR_HEIGHT = 1.28f;

    const float STOPPING_DISTANCE = 0.15f;     // Distance between player and target tile before they stop

    private Vector3 moveDirection;             // of travel 
    private Vector3 previousMoveDirection;     
    private Vector3 moveVelocity;              // What gets added to my rigidbody's velocity each frame   

    Vector3 targetPosition;                    // Where I clicked to move

    [SerializeField]
    private Animator animator;                 // The Animator for the character
    
    bool jumping;                              // Am I jumping right now?

	// Use this for initialization
	void Start () {

        targetPosition = Vector3.zero;
        moveDirection = Vector3.zero;        
        jumping = false;                        
	
	}
	
	// Update is called once per frame
	void Update () {

        print("Transform forward: " + transform.forward);
        float angle = 0.0f;

        CharacterController controller = GetComponent<CharacterController>();

        if (controller.isGrounded)
        {

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            jumping = false;
            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.Normalize();
            moveDirection *= MAX_SPEED;

            Debug.Log("movedir: " + moveDirection);

            Vector3 scaledPos = transform.position;
            scaledPos.Scale(new Vector3(1, 0, 1));
            //   Vector3 
            Vector3 u = scaledPos - (scaledPos + moveDirection);
            angle = Mathf.Atan2(u.z, u.x);
       //     float angle = Vector3.Angle(scaledPos, scaledPos + moveDirection);
            print("angle of direction: " + (angle));

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
                jumping = true;
            }

        }

        animator.SetBool("isJumping", jumping);
        if (!jumping) 
            animator.SetFloat("moveSpeed", moveDirection.magnitude);
        else
            animator.SetFloat("moveSpeed", 0.0f);

        // Turn to face, using Quaternion Lerp
        //print(transform.rotation.y);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up),1.0f);

        moveDirection.y -= gravity * Time.deltaTime;        
               
        controller.Move(moveDirection * Time.deltaTime);


        


	}    


}
