using UnityEngine;
using System.Collections;

public class BuildingBehavior : MonoBehaviour {

    public bool placing;

    private SpriteRenderer spriteRenderer;

    private GameObject confirmButton;
    private GameObject cancelButton;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent< SpriteRenderer > ();

        foreach(Transform child in GetComponentInChildren<Transform>()){
            if (child.CompareTag("Confirm")) {
                confirmButton = child.gameObject;
            }
            if (child.CompareTag("Cancel")) {
                cancelButton = child.gameObject;
            }
            //if (child.) 
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if (placing) {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0.6f;
            spriteRenderer.color = spriteColor;
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
        } else {
            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 1f;
            spriteRenderer.color = spriteColor;
            confirmButton.SetActive(true);
            cancelButton.SetActive(true);
        }
	}
}
