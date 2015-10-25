using UnityEngine;
using System.Collections;

public class AIState : MonoBehaviour {


    // Use this for initialization
	void Start () {
        OnEnterState(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        UpdateState();
	}

    // When I've started this state, what do I have to do
    public virtual void OnEnterState(GameObject _me)
    {

    }


    // What I call every time I'm doing something in this state
    public virtual void UpdateState()
    {
        
    }


    // What I need to do when leaving this state
    public virtual void OnExitState()
    {

    }

}



