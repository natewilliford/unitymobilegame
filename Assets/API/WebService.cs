using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Networking;
using System;

public class WebService : MonoBehaviour {

    //private const string ENDPOINT = "http://mobileappserver-186401.appspot.com";
    private const string ENDPOINT = "http://localhost:8080";

    void Start () {
        //Debug.Log("Starting network request");
        //StartCoroutine(RegisterRequest());
        
        //Debug.Log("unique id: " + SystemInfo.deviceUniqueIdentifier);
    }

    public void Register() {
        StartCoroutine(RegisterRequest());
    }

    private IEnumerator RegisterRequest() {
        WWWForm form = new WWWForm();

        //form.AddField("json", "{\"poops\": \"farts\", \"fieldthatdoesntexist\": \"bla\"}");
        RegisterRequest registerRequest = new RegisterRequest();
        //registerRequest.uniqueDeviceId = SystemInfo.deviceUniqueIdentifier;
        registerRequest.email = "nathan.williford@gmail.com";
        registerRequest.password = "123abc";
        
        form.AddField("json", JsonUtility.ToJson(registerRequest));
        using (UnityWebRequest www = UnityWebRequest.Post(ENDPOINT + "/register", form)) {
            yield return www.Send();

            if (www.isError) {
                Debug.Log(www.error);
            } else {
                RegisterResponse resp = JsonUtility.FromJson<RegisterResponse>(www.downloadHandler.text);

                if (!String.IsNullOrEmpty(resp.error)) {
                    Debug.Log("Error resp: " + resp.error);
                } else {


                    Debug.Log("the user id is: " + resp.user.userId);
                    

                }
            }
        }
    }

    public void Login() {
        StartCoroutine(LoginRequest());
    }

    private IEnumerator LoginRequest() {
        WWWForm form = new WWWForm();

        //form.AddField("json", "{\"poops\": \"farts\", \"fieldthatdoesntexist\": \"bla\"}");
        LoginRequest request = new LoginRequest();
        //registerRequest.uniqueDeviceId = SystemInfo.deviceUniqueIdentifier;
        request.email = "nathan.williford@gmail.com";
        request.password = "123abc";

        form.AddField("json", JsonUtility.ToJson(request));
        using (UnityWebRequest www = UnityWebRequest.Post(ENDPOINT + "/login", form)) {
            yield return www.Send();

            if (www.isError) {
                Debug.Log(www.error);
            } else {
                LoginResponse resp = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
                Debug.Log(www.downloadHandler.text);
                if (resp == null) {
                    Debug.Log("Could not parse response: " + www.downloadHandler.text);
                    yield break;
                }

                if (!String.IsNullOrEmpty(resp.error) || resp.token == null) {
                    PlayerPrefs.DeleteKey("login_token");
                    if (!String.IsNullOrEmpty(resp.error)) {
                        Debug.Log("Error resp: " + resp.error);
                    }
                    if (resp.token == null) {
                        Debug.Log("No token in response.");
                    }
                } else {
                    
                    //Debug.Log("the user id is: " + resp.user.userId);
                    Debug.Log(resp.token);
                    PlayerPrefs.SetString("login_token", resp.token);

                }
            }
        }
    }

    public void Sync() {
        StartCoroutine(SyncRequest());
    }

    private IEnumerator SyncRequest() {
        String loginToken = PlayerPrefs.GetString("login_token");
        if (loginToken == null || loginToken.Length < 1) {
            Debug.Log("No login token");
            yield break;
        }

        SyncRequest syncRequest = new SyncRequest();
        syncRequest.token = loginToken;
        WWWForm form = new WWWForm();
        form.AddField("json", JsonUtility.ToJson(syncRequest));
        using (UnityWebRequest www = UnityWebRequest.Post(ENDPOINT + "/sync", form)) {
            yield return www.Send();
            Debug.Log(www.downloadHandler.text);

            if (www.isError) {
                Debug.Log(www.error);

            } else if (www.responseCode != 200) {
                Debug.Log("Failed with response code: " + www.responseCode);
            } else {
                SyncResponse resp = JsonUtility.FromJson<SyncResponse>(www.downloadHandler.text);
                if (resp.error != null) {
                    Debug.Log("error: " + resp.error);
                } else if (resp.gameState == null) {
                    Debug.Log("no game state returned.");
                } else {
                    Debug.Log("got game state");
                }


            };
        }
    }

    public void Buy() {
        StartCoroutine(BuyRequest());
    }

    private IEnumerator BuyRequest() {
        String loginToken = PlayerPrefs.GetString("login_token");
        if (loginToken == null || loginToken.Length < 1) {
            Debug.Log("No login token");
            yield break;
        }

        BuyRequest buyRequest = new BuyRequest();
        buyRequest.token = loginToken;

        buyRequest.buildingType = Constants.Buildings.TYPE_FARM;

        WWWForm form = new WWWForm();

        Debug.Log("json request: " + JsonUtility.ToJson(buyRequest));

        form.AddField("json", JsonUtility.ToJson(buyRequest));
        using (UnityWebRequest www = UnityWebRequest.Post(ENDPOINT + "/buy", form)) {
            yield return www.Send();
            Debug.Log("text: " + www.downloadHandler.text);

            if (www.isError) {
                Debug.Log(www.error);

            } else if (www.responseCode != 200) {
                Debug.Log("Failed with response code: " + www.responseCode);
            } else {
                BuyResponse resp = JsonUtility.FromJson<BuyResponse>(www.downloadHandler.text);
                if (resp == null) {
                    Debug.Log("null resp");
                    yield break;
                }

                if (resp.error != null) {
                    Debug.Log("error: " + resp.error);
                } else if (resp.building == null) {
                    Debug.Log("no building returned");

                } else {
                    Debug.Log("Bought building: " + resp.building.id + " last collected: " + resp.building.lastCollected);
                }


            };
        }
    }


}

[Serializable]
public class GenericRequest {

}

[Serializable]
public class AuthorizedRequest : GenericRequest {
    public string token;
}

[Serializable]
public class RegisterRequest : GenericRequest {
    public string uniqueDeviceId;
    public string email;
    public string password;
}

[Serializable]
public class GenericResponse {
    public string error;
}

[Serializable]
public class RegisterResponse : GenericResponse{
    public ApiUser user;
    public string token;
}

[Serializable]
public class ApiUser {
    public string userId;
    public string email;
}


[Serializable]
class LoginRequest : GenericRequest {
    public string email;
    public string password;
}

[Serializable]
class LoginResponse : GenericResponse {
    public ApiUser user;
    public string token;
}



[Serializable]
class SyncRequest : AuthorizedRequest {

}

[Serializable]
class SyncResponse : GenericResponse {
    public GameState gameState;
}

[Serializable]
class BuyRequest : AuthorizedRequest {
    public int buildingType;
}

[Serializable]
class BuyResponse : GenericResponse {
    public Building building;
}