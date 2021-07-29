using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the brach instrument.
/// </summary>
public class Canvas_Brach : MonoBehaviour {

    public Canvas_Chord ChordMenu { get; private set; }
    Canvas_Strings strings;

    private void Awake() {
        ChordMenu = GetComponentInChildren<Canvas_Chord>();
        strings = GetComponentInChildren<Canvas_Strings>();
    }
}
