﻿using UnityEngine;
using System.Collections;

public class EnemyExplosionScript : MonoBehaviour {

    
    void Awake()
    {
        this.GetComponent<ParticleSystem>().startColor = Color.red;
    }

    void Start()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(3.0f);

        GameObject.Destroy(this.gameObject);
    }


    public void SetColour(Color _color)
    {
        this.GetComponent<ParticleSystem>().startColor = _color;
    }

}
