using UnityEngine;

public class CheckNetworkManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (GameObject.FindGameObjectWithTag("NetworkManager"))
        {
            Destroy(GameObject.FindGameObjectWithTag("NetworkManager"));
        }
		
	}
	
	
}
