using UnityEngine;
using System.Collections;

public class SpawnTileEmitterScript : MonoBehaviour {




	// Use this for initialization
	void Awake () {
        this.GetComponent<ParticleSystem>().startColor = Color.red;
	}

    void Start()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(4.0f);

        GameObject.Destroy(this.gameObject);
    }
			

    public void SetColour(Color _color)
    {
        this.GetComponent<ParticleSystem>().startColor = _color;
    }

}
