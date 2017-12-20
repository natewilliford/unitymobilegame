using UnityEngine;
using System.Collections;
using System;

enum GameMode {
    Normal,
    Placing,
}

public class Game : MonoBehaviour, IObserver<TouchGesture> {

    

    private WebService webService;

    private GameMode gameMode;
    private TouchManager touchManager;

    public GameObject placingBuilding;

    void Start () {
        webService = gameObject.GetComponent<WebService>();
        gameMode = GameMode.Normal;
        touchManager = new TouchManager();
        touchManager.Subscribe(this);


        
    }
	
	void Update () {
        touchManager.UpdateTouches();
	}
    

    public void Login() {
        webService.Login();
    }

    public void Register() {
        webService.Register();
    }

    public void Sync() {
        webService.Sync();
    }

    public void Buy() {
        webService.Buy();
    }

    void IObserver<TouchGesture>.OnCompleted() {}

    void IObserver<TouchGesture>.OnError(Exception exception) {}

    void IObserver<TouchGesture>.OnNext(TouchGesture gesture) {
        switch(gesture.type) {
            case TouchGestureType.Tap:
                Debug.Log("Tap detected");
                break;
            case TouchGestureType.Drag:
                Debug.Log("Drag detected");
                break;
            case TouchGestureType.DragEnd:
                Debug.Log("Drag end detected");
                break;
        }

        if (gesture.type == TouchGestureType.Drag) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(gesture.endPosition);
            placingBuilding.transform.position = worldPoint;
            placingBuilding.GetComponent<BuildingBehavior>().placing = true;
        } else if (gesture.type == TouchGestureType.DragEnd) {
            placingBuilding.GetComponent<BuildingBehavior>().placing = false;
        }


    }
}