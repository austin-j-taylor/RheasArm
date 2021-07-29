using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static Music_Pitches;

public class Canvas_Strings : MonoBehaviour {

    private Transform headerElements;
    private Canvas_StringElement[] elements;

    private void Awake() {
        headerElements = transform.Find("Elements");
        elements = headerElements.GetComponentsInChildren<Canvas_StringElement>();
        elements[0].Initialize(KeyCode.Alpha1, 0);
        elements[1].Initialize(KeyCode.Alpha2, 1);
        elements[2].Initialize(KeyCode.Alpha3, 2);
        elements[3].Initialize(KeyCode.Alpha4, 3);
        elements[4].Initialize(KeyCode.Alpha5, 4);
    }

}
