using UnityEngine;
using System.Collections;
using UnityEngine.Networking;



public class OverloadCharacterControllor : NetworkBehaviour
{
  // Speed    
  private const float MAX_SPEED = 5f;        // How fast I can go
  [SerializeField]
  private float currentSpeed = 0.0f;         // How fast I am going
  [SerializeField]
  private float acceleration = 1.0f;         // How fast I accelerate

  [SerializeField]
  private float rotSpeed = 5.0F;
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

  private StateManager stateManager;


  void Awake()
  {
      GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerA");
      if (players.Length > 1)
      {
          players[1].tag = "PlayerB";
      }        
  }

  // Use this for initialization
  void Start() {

    targetPosition = Vector3.zero;
    moveDirection = Vector3.zero;
    jumping = false;
    stateManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StateManager>();
    
    

  }

  // Update is called once per frame
  void Update() {

    //print("Transform forward: " + transform.forward);
    //float angle = 0.0f;

    if (stateManager.currentState == StateManager.GameState.PLAYING_STATE) {

      CharacterController controller = GetComponent<CharacterController>();

      if (controller.isGrounded && !animator.GetBool("hasDied")) {

        char playerLetter = tag[6];
        float horizontal = Input.GetAxis("Horizontal" + playerLetter);
        float vertical = Input.GetAxis("Vertical" + playerLetter);
        // print("H:" + horizontal + "  V:" + vertical);

        jumping = false;
        moveDirection = new Vector3(horizontal, 0, vertical);
        // moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.Normalize();
        moveDirection *= MAX_SPEED;

        // Rotate in the direction the character is moving by slerping towards the correct transform direction (forward,
        // back, right, left) based on WS (vertical) and AD (horizontal) key input
        // Vertical rotation
        if (vertical > 0)
          transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), rotSpeed * Time.deltaTime);
        else if (vertical < 0)
          transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), rotSpeed * Time.deltaTime);
        // Horizontal rotation
        if (horizontal > 0)
          transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), rotSpeed * Time.deltaTime);
        else if (horizontal < 0)
          transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), rotSpeed * Time.deltaTime);

        if (Input.GetButton("Jump"+playerLetter)) {
          moveDirection.y = jumpSpeed;
          jumping = true;
        }
      }

      animator.SetBool("isJumping", jumping);
      if (!jumping)
        animator.SetFloat("moveSpeed", moveDirection.magnitude);
      else
        animator.SetFloat("moveSpeed", 0.0f);

      moveDirection.y -= gravity * Time.deltaTime;

      if (animator.GetBool("hasDied"))
        moveDirection = Vector3.zero;

      controller.Move(moveDirection * Time.deltaTime);
    }
  }
}