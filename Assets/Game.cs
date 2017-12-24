using UnityEngine;
using System.Collections;
using System;


public class Game : MonoBehaviour, IObserver<TouchGesture> {

    

    private WebService webService;


    private TouchManager touchManager;

    private BuildingBehavior placingBuildingBehavior;

    public BuildingBehavior farmPrefab;
    

    void Start () {
        webService = gameObject.GetComponent<WebService>();

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

        if (placingBuildingBehavior != null) {
            if (gesture.type == TouchGestureType.Tap) {
                //Debug.Log("Tap at " + gesture.endPosition);
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.endPosition), Vector2.zero);
                if (hitInfo) {

                    if (hitInfo.transform.CompareTag("Confirm")) {
                        ConfirmPlacement();
                    } else if (hitInfo.transform.CompareTag("Cancel")) {
                        CancelPlacement();
                    }
                }
            } else if (gesture.type == TouchGestureType.Drag) {

                //Debug.Log("drag at " + gesture.endPosition);
                Vector2 endPoint = Camera.main.ScreenToWorldPoint(gesture.endPosition);
                Vector2 placePoint = new Vector2(endPoint.x, endPoint.y + 0.3f);

                if (placingBuildingBehavior.IsDragging()) {
                    placingBuildingBehavior.gameObject.transform.position = placePoint;
                } else {

                    if (placingBuildingBehavior != null) {
                        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.startPosition), Vector2.zero);
                        if (hitInfo && hitInfo.collider.gameObject == placingBuildingBehavior.gameObject) {
                            placingBuildingBehavior.gameObject.transform.position = placePoint;
                            placingBuildingBehavior.SetDragging(true);
                        }
                    }
                }

            } else if (gesture.type == TouchGestureType.DragEnd) {
                if (placingBuildingBehavior != null) {
                    placingBuildingBehavior.SetDragging(false);
                }
            }
        }


    }

    public void PlaceBuilding() {
        Debug.Log("place buliding");
        if (placingBuildingBehavior != null) {
            Debug.Log("canceling previous");
            CancelPlacement();
        }

        placingBuildingBehavior = Instantiate< BuildingBehavior>(farmPrefab);
        if (placingBuildingBehavior.gameObject != null) {
            Debug.Log("got a building to place");
        } else {
            Debug.Log("it's broken, yo");
        }
    }

    private void CancelPlacement() {
        Destroy(placingBuildingBehavior.gameObject);
        placingBuildingBehavior = null;
    }
    private void ConfirmPlacement() {
        placingBuildingBehavior.ConfirmPlacement();
        placingBuildingBehavior = null;
    }
}