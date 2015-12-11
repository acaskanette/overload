using UnityEngine;
using System.Collections;

/*public class AIStateChase : AIState

{

    private const float ARRIVE_DISTANCE = 0.025f;   // Squared distance space

    GameObject player;
    Vector3 playerPosition;

    GameObject me;
    Vector3 myPosition;

    Vector3 targetPosition;

    FloorScript floorScript;

    Vector3 moveDirection = Vector3.zero;
    enum Direction {Up, Down, Left, Right, None};
    Vector3[] directions = {   new Vector3(0.0f, 0.0f, 1.0f), 
                               new Vector3(0.0f, 0.0f, -1.0f), 
                               new Vector3(-1.0f, 0.0f, 0.0f), 
                               new Vector3(1.0f, 0.0f, 0.0f), 
                               new Vector3(0.0f, 0.0f, 0.0f) };
    Direction travelDirection;


    public override void OnEnterState(GameObject _me)
    {
        player = GameObject.FindWithTag("Player");
        me = _me;
        floorScript = GameObject.FindWithTag("Manager").GetComponent<FloorScript>();
        travelDirection = Direction.Right;
    }


    // What I call every time I'm doing something in this state
    public override void UpdateState()
    {

        // If I have arrived at my target position
        if ((me.transform.position - targetPosition).sqrMagnitude < ARRIVE_DISTANCE)
        {
            // Find my next position
            FindNextPosition();
        }

        // Move toward my target position

    }


    // What I need to do when leaving this state
    public override void OnExitState()
    {

    }

    void FindNextPosition()
    {
     
        //float deltaZ = player.transform.position.z - me.transform.position.z;
        //float deltaX = player.transform.position.x - me.transform.position.x;

        playerPosition = player.transform.position;
        myPosition = me.transform.position;

        
        // Find out if I have passed the player along my axis of travel        
        switch (travelDirection)
        {
            case Direction.None:                
                break;
            case Direction.Up:                
                // myX >= theirX
                if (myPosition.x >= playerPosition.x)
                { // if I passed them on this axis
                    // Right or Left
                    travelDirection = (playerPosition.z > myPosition.z) ? Direction.Right : Direction.Left;
                }
                break;
            case Direction.Down:
                // myX <= theirX
                if (myPosition.x <= playerPosition.x)
                {
                    // Right or Left
                    travelDirection = (playerPosition.z > myPosition.z) ? Direction.Right : Direction.Left;
                }
                break;
            case Direction.Left:
                // myZ <= theirZ
                if (myPosition.z <= playerPosition.z)
                {
                    // Up or Down
                    travelDirection = (playerPosition.x > myPosition.x) ? Direction.Up : Direction.Down;
                }
                break;
            case Direction.Right:
                // myZ >= theirZ
                if (myPosition.z >= playerPosition.z)
                {
                    // Up or Down
                    travelDirection = (playerPosition.x > myPosition.x) ? Direction.Up : Direction.Down;
                }
                break;
        }

        // Set my movedirection
        moveDirection = directions[(int)travelDirection];
        
        // Set next targetPosition

        // Finding my Tile
        RaycastHit hit;
        Vector2 myTileIndices = Vector2.zero;
        if (Physics.Raycast(me.transform.position, -Vector3.up, out hit))
        {
            if (hit.collider.tag == "Tile")
            {
                myTileIndices = floorScript.GetTileIndicesByGameObject(hit.collider.gameObject);
            }
        }
        
        switch (travelDirection)
        {
            case Direction.Up:
                myTileIndices = myTileIndices + new Vector2(0, -1);
                break;
            case Direction.Down:
                myTileIndices = myTileIndices + new Vector2(0, 1);
                break;
            case Direction.Left:
                myTileIndices = myTileIndices + new Vector2(-1, 0);
                break;
            case Direction.Right:
                myTileIndices = myTileIndices + new Vector2(1, 0);
                break;
        }

        targetPosition = (floorScript.GetTileByIndex((int)(myTileIndices.x), (int)(myTileIndices.y))).transform.position;


                              

    }

    public bool CheckAxisEqual(float _axisPosition)
    {
        return true;
    }

}
*/