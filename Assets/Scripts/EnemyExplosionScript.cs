using UnityEngine;
using System.Collections;

public class EnemyExplosionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine(Death());

	}

    IEnumerator Death()
    {
        yield return new WaitForSeconds(3.0f);

        GameObject.Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
