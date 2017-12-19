using UnityEngine;
using System.Collections;
using System;

enum GameMode {
    Normal,
    Placing,
}

public class Game : MonoBehaviour {

    

    private WebService webService;

    private GameMode gameMode;
    private TouchTracker touchTracker;

    void Start () {
        webService = gameObject.GetComponent<WebService>();
        gameMode = GameMode.Normal;
    }
	
	void Update () {

        UpdateTouches();
	}

    private void UpdateTouches() {

        Vector2 touchPosition = Vector2.zero;
        if (touchTracker == null) {
            bool downTouchDetected = false;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                //Debug.Log("Detected tap");
                downTouchDetected = true;
                touchPosition = Input.GetTouch(0).position;
            } else if (Input.GetMouseButtonDown(0)) {
                downTouchDetected = true;
                //Debug.Log("Detected click");
                touchPosition = Input.mousePosition;
            }

            if (downTouchDetected) {
                touchTracker = new TouchTracker();
                touchTracker.TrackTouchDown(touchPosition, DateTime.Now);
            }
        } else {

            bool touchContinuing = false;
            if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)) {
                touchTracker.TrackTouchDown(Input.GetTouch(0).position, DateTime.Now);
                touchContinuing = true;
            } else if (Input.GetMouseButton(0)) {
                touchTracker.TrackTouchDown(Input.mousePosition, DateTime.Now);
                touchContinuing = true;
            }

            if (touchContinuing) {
                if (touchTracker.touchState == TouchState.Dragging) {
                    HandleDrag(touchTracker);
                }
            } else {
                
                bool touchFinished = false;
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
                    touchTracker.TrackTouchDown(Input.GetTouch(0).position, DateTime.Now);
                    touchFinished = true;
                } else if (Input.GetMouseButtonUp(0)) {
                    touchTracker.TrackTouchDown(Input.mousePosition, DateTime.Now);
                    touchFinished = true;
                }

                if (touchFinished) {
                    if (touchTracker.touchState == TouchState.Tapping) {
                        HandleTap(touchTracker);
                    } else if (touchTracker.touchState == TouchState.Dragging) {
                        HandleFinisheDragging(touchTracker);
                    }
                    touchTracker = null;
                }
            }
        }


        //switch (gameMode) {
        //    case GameMode.Normal:
        //        UpdateNormalTouches();
        //        break;
        //    case GameMode.Placing:
        //        UpdatePlacingTouches();
        //        break;
        //}
        
    }

    private void HandleTap(TouchTracker touchTracker) {
        Debug.Log("HandleTap");
    }

    private void HandleDrag(TouchTracker touchTracker) {
        Debug.Log("HandleDrag");
    }

    private void HandleFinisheDragging(TouchTracker touchTracker) {
        Debug.Log("HandleFinisheDragging");
    }

    //private void UpdateNormalTouches() {
    //    // TODO: Support multitouch.



    //    if (tapDectected) {

    //        // 2D touches
    //        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(tapPosition), Vector2.zero);
    //        if (hitInfo) {
    //            Debug.Log("Raycast hit: " + hitInfo.transform.gameObject.name);
    //        }

    //        //// For 3D touches
    //        //Ray ray = Camera.main.ScreenPointToRay(tapPosition);
    //        //RaycastHit raycastHit;
    //        //if (Physics.Raycast(ray, out raycastHit)) {
    //        //    Debug.Log("Raycast hit: " + raycastHit.collider.tag);
    //        //}
    //    }
    //}

    private void UpdatePlacingTouches() {

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
}