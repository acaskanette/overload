using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]

public class OverloadCharacterControllor : MonoBehaviour {
    

    // Speed
    [SerializeField]
    private const float MAX_SPEED = 5f;        // How fast I can go
    [SerializeField]
    private float currentSpeed = 0.0f;         // How fast I am going
    [SerializeField]
    private float acceleration = 1.0f;         // How fast I accelerate while holding the button

    const float STOPPING_DISTANCE = 0.53f;     // Distance between player and target tile before they stop

    private Vector3 moveDirection;             // of travel

    Vector3 targetPosition;                    // Where I clicked to move


	// Use this for initialization
	void Start () {

        targetPosition = Vector3.zero;
        moveDirection = Vector3.zero;
	
	}
	
	// Update is called once per frame
	void Update () {
	
        // Get Mouse-click
        if (Input.GetMouseButtonDown(0))
        {
            // Figure out what block I just clicked
            Ray rayToTile = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(rayToTile, out hit, 100))
            {
                if (hit.collider.tag == "Tile")
                {
                    print("Tile hit!");
                    //CharacterController controller = GetComponent<CharacterController>();
                   
                    
                    //if (controller.isGrounded)
                    //{
                        
                        // Capture information about the tile hit
                        targetPosition = hit.point;
                        print("Hit Point: " + hit.point);
                       // print(targetTile);

                        currentSpeed = 0.0f;
                        moveDirection = (targetPosition - gameObject.transform.position);
                        moveDirection.Normalize();
                       // print(moveDirection);
   
                    //}
                    
                }
            }
                      
        }

        
        if (targetPosition != Vector3.zero && ((targetPosition - gameObject.transform.position).magnitude > STOPPING_DISTANCE))
        {
        // If distance between me and target tile is not low
          //  print("accelerating....");
            moveDirection = (targetPosition - gameObject.transform.position);
            moveDirection.Normalize();
            currentSpeed += acceleration;            
        }        
        else {
        // otherwise, it is low so slow the eff down
            //print("deceleration");
            currentSpeed = 0.0f;
            targetPosition = Vector3.zero;
        }

        moveDirection *= currentSpeed;
        if (moveDirection.magnitude > MAX_SPEED)
        {
            moveDirection.Normalize();
            moveDirection *= MAX_SPEED;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, MAX_SPEED);
                
        // print("Target Point: " + targetPosition + "  My position: " + gameObject.transform.position + "   Mag: " + (targetPosition - transform.position).magnitude);


        moveDirection.y = 0;    // no vertical movement
        gameObject.GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
        


	}
}
