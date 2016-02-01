using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour {

    [SerializeField]
    private GUISkin skin;

	// Use this for initialization
	void Start () {
         
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.skin = skin;
    }
}
