using UnityEngine;
using System.Collections;

public class BuildingBehavior : MonoBehaviour {

    public bool placing;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent< SpriteRenderer > ();
    }
	
	// Update is called once per frame
	void Update () {
	    if (placing) {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0.75f;
            spriteRenderer.color = spriteColor;
        } else {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 1f;
            spriteRenderer.color = spriteColor;
        }
	}
}
