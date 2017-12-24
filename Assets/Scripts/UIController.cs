using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    //public GameObject settingsPanel;
    public GameObject storePanel;

    public Text goldText;

    private Game game;

	// Use this for initialization
	void Start () {
        game = GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public void OpenSettings() {
    //    Debug.Log("Open settings");
    //    settingsPanel.SetActive(true);
    //}

    public void OpenStorePanel() {
        storePanel.SetActive(true);
    }

    public void BuyFarm() {
        storePanel.SetActive(false);
        game.PlaceBuilding();
    }

    public void SetGold(long gold) {
        goldText.text = gold.ToString();
    }
}
