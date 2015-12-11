using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyScript : NetworkBehaviour {
  [SerializeField]
  private GameObject deathEffect; // Particle system on death

  public float orbitSpeed = 30.0f; // How fast the enemy moves to chase the player in orbit mode
  public float diveSpeed = 200.0f; // How fast the enemy dives towards a position in divebomb mode
  public float diveHeight = 5.0f;
  private Color colour; // Type of enemy, what colour it glows

  private GameObject player; // Reference to the player in the scene
  private Rigidbody rigidBody;
  private string owningSpawner; // "A" or "B"

  // Orbit always orbits
  // DivePinpoint flies upwards, saves player position, then enters DiveBomb
  // DiveBomb flies towards the player position that was saved
  public enum AIState { Orbit, DivePinpoint, DiveBomb, Dying }

  private AIState state;
  private Vector3 diveToPosition = Vector3.zero;
  private bool pinpointed = false;

  // Use this for initialization
  void Start() {
    owningSpawner = gameObject.transform.position.x < 0.0 ? "A" : "B";
    //print(owningSpawner + gameObject.transform.position);

    player = GameObject.FindGameObjectWithTag("Player" + owningSpawner);
    rigidBody = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void FixedUpdate() {
    switch (state) {
      case AIState.Orbit:
        Orbit();
        break;
      case AIState.DivePinpoint:
        DivePinpoint();
        break;
      case AIState.DiveBomb:
        DiveBomb();
        break;
      case AIState.Dying:
        Dying();
        break;
    }
  }

  void OnCollisionEnter(Collision other) {
    if (state == AIState.Orbit && other.collider.tag == "Tile")
      rigidBody.AddForce(Vector3.up * orbitSpeed * Time.deltaTime, ForceMode.Impulse);
  }

  void Orbit() {
    // Always look towards the player
    transform.LookAt(player.transform);
    // Orbit around the player on world-y axis
    transform.RotateAround(player.transform.position, Vector3.up, 20 * Time.deltaTime);
    // Add force to move towards the player (acceleration)
    rigidBody.AddForce(transform.forward * orbitSpeed * Time.deltaTime, ForceMode.Acceleration);

    // Send a force causing the enemy to come back down after flying upwards upon hitting a tile
    if (transform.position.y >= 3.0f)
      rigidBody.AddForce(Vector3.down * Mathf.Lerp(transform.position.y, 0.0f, Time.deltaTime), ForceMode.Force);
  }

  void DivePinpoint() {
    // Always look towards the player
    transform.LookAt(player.transform);
    // Send the enemy upwards to begin divebomb
    if (transform.position.y <= diveHeight)
      rigidBody.AddForce(Vector3.up * Mathf.Lerp(transform.position.y, diveHeight, Time.deltaTime), ForceMode.Force);
    else if (transform.position.y >= 7.5f)
      rigidBody.AddForce(Vector3.down * Mathf.Lerp(transform.position.y, 0.0f, Time.deltaTime), ForceMode.Force);

    if (!pinpointed && transform.position.y >= diveHeight - 1.0f) {
      //print("Pinpointing now");
      StartCoroutine(PinpointPlayer());
    }
  }

  void DiveBomb() {
    // Always look towards the player
    transform.LookAt(diveToPosition);
    rigidBody.AddForce(transform.forward * diveSpeed * Time.deltaTime, ForceMode.Force);

    // When arrived at dive position, go back to pinpointing
    float diff = (diveToPosition - transform.position).magnitude;
    if (diff <= 1.0F) {
      //print("Flying up");
      // Remove all forces so it just goes straight back up
      rigidBody.velocity = Vector3.zero;
      // Go back to pinpoint mode
      SetAIState(AIState.DivePinpoint);
    }
  }

  void Dying() {
    // Make color of cube flash
    colour.r = 0.75f + (0.25f * Mathf.Sin(Time.deltaTime));
    GetComponent<Renderer>().material.SetColor("_MKGlowColor", colour);
  }

  IEnumerator PinpointPlayer() {
    // Set pinpointed to true so this does not run multiple times
    pinpointed = true;
    // Wait for 2.5 seconds and then set the dive position to whereever the player was at this point
    yield return new WaitForSeconds(2.5F);
    diveToPosition = player.transform.position;
    //print("Diving");
    pinpointed = false;
    SetAIState(AIState.DiveBomb);
  }

  public void SetColour(Color newColour) {
    colour = newColour;
    GetComponent<Renderer>().material.SetColor("_MKGlowColor", colour);
  }

  public void SetAIState(AIState newState) {
    state = newState;

    if (state == AIState.Dying)
      SetColour(Color.red);
  }

  public void Kill() {
    // Update the player's score
    GameObject.FindWithTag("Manager").GetComponent<ScoreManager>().EnemyKilled();
    // Create an instance of the death effect
    GameObject deathEffectInstance = Instantiate(deathEffect, gameObject.transform.position, Quaternion.identity) as GameObject;
    deathEffectInstance.GetComponent<EnemyExplosionScript>().SetColour(colour);
    // Destroy the game object
    Destroy(gameObject);
  }
}
