using UnityEngine;
using System.Collections;
using MKGlowSystem;

public class MKGlowScript : MonoBehaviour {


    private MKGlow mkGlow;
    
    void Awake () {
        mkGlow = this.GetComponent<MKGlow>();
        InitGlowSystem();
    }

    private void InitGlowSystem()
    {
        mkGlow.BlurIterations = 5;
        mkGlow.BlurOffset = 0.25f;
        mkGlow.Samples = 4;
        mkGlow.GlowIntensity = 0.3f;
        mkGlow.BlurSpread = 0.25f;

        mkGlow.GlowType = MKGlowType.Selective;
        mkGlow.GlowQuality = MKGlowQuality.High;
        //currentRoom0GlowColor = 0;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
