using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour {

    public Button OpenButton;

	// Use this for initialization
	void Start () {
        var clickEvent = new Button.ButtonClickedEvent();
        clickEvent.AddListener(() =>
        {
            NativeAPI.presentNativeView();
        });
        OpenButton.onClick = clickEvent;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
