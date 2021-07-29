using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Music_Pitches;

/// <summary>
/// Controls the menu that appears when right-clicking with the brach open.
/// </summary>
public class Canvas_Chord : MonoBehaviour {

    // Constants
    private const int menu_maxNumQualities = 6, menu_maxNumIntervals = 8;
    private const int music_numInterals = 7;
    private static readonly int[] interval_offsets = { 0, -1, -1, 0, 0, -1, -2 };

    // Type definitions
    public enum ChordState { Open, Closed };

    // Properties
    public ChordState State { get; private set; }

    // Fields
    //private ChordElement[,] elements = new ChordElement[menu_maxNumIntervals, menu_maxNumQualities];
    private RectTransform rectTransform;
    private Transform headerElements;
    private Canvas_ChordElement[] elements;
    public Canvas_ChordElement CurrentChord { get; set; }

    private float pixel_width, pixel_height, pixel_elementSize;

    private void Awake() {
        // Initialize elements
        rectTransform = GetComponent<RectTransform>();
        headerElements = transform.Find("Elements");
        elements = headerElements.GetComponentsInChildren<Canvas_ChordElement>();
    }

    private void Start() {
        for (int i = 0; i < menu_maxNumIntervals; i++) {
            for (int j = 0; j < menu_maxNumQualities; j++) {
                elements[i * menu_maxNumQualities + j].Initialize((ChordInterval)(i % music_numInterals), (ChordQuality)(interval_offsets[i % music_numInterals] + j), (ChordOctave)(i / music_numInterals), this);
            }
        }
        (int, int) initialCoords = ChordToCoords(ChordInterval.I, ChordQuality.maj, ChordOctave.One);
        CurrentChord = elements[initialCoords.Item1 * menu_maxNumQualities + initialCoords.Item2];
        pixel_width = rectTransform.rect.width;
        pixel_height = rectTransform.rect.height;
        pixel_elementSize = elements[0].GetComponent<RectTransform>().rect.width;
        Debug.Log(pixel_width + "," + pixel_height +", "+ pixel_elementSize);
        State_ToClose();
    }

    private void Update() {
        // Transitions
        switch (State) {
            case ChordState.Open:
                if (!Input.GetKey(KeyCode.Mouse1)) {
                    State_ToClose();
                }
                break;
            case ChordState.Closed:
                if (Input.GetKey(KeyCode.Mouse1)) {
                    State_ToOpen();
                }
                break;
        }

        // Actions
        switch (State) {
            case ChordState.Open:
                State_InOpen();
                break;
            case ChordState.Closed:
                State_InClose();
                break;
        }
    }

    private void State_ToOpen() {
        // Make the menu appear
        headerElements.gameObject.SetActive(true);

        // Move the menu so that the mouse is over the current chord
        float xPos = Input.mousePosition.x - Screen.width/2;
        float yPos = Input.mousePosition.y - Screen.height/2;

        xPos -= CurrentChord.rectTransform.anchoredPosition.x;
        yPos -= CurrentChord.rectTransform.anchoredPosition.y;
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);

        State = ChordState.Open;
    }
    private void State_ToClose() {
        // Make the menu disappear
        // Confirm the current selected chord
        headerElements.gameObject.SetActive(false);

        State = ChordState.Closed;
    }

    // Actions
    private void State_InOpen() {
        // Change the selected chord
        // Highlight the selected chord
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0) {
            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.y -= pixel_elementSize;
            rectTransform.anchoredPosition = newPos;
        } else if(scroll < 0) {
            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.y += pixel_elementSize;
            rectTransform.anchoredPosition = newPos;
        }
    }
    private void State_InClose() {
        // Scroll like when open
        // Change the selected chord
        // Highlight the selected chord
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0) {
            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.y -= pixel_elementSize;
            rectTransform.anchoredPosition = newPos;
            ScrollChord(1);
        } else if (scroll < 0) {
            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.y += pixel_elementSize;
            rectTransform.anchoredPosition = newPos;
            ScrollChord(-1);
        }
    }

    private (int, int) ChordToCoords(ChordInterval interval, ChordQuality quality, ChordOctave octave) {
        int i = (int)interval + 7 * (int)octave;
        int j = (int)quality - interval_offsets[(int)interval];

        return (i, j);
    }

    private void ScrollChord(int verticalChange) {
        (int, int) indices = ChordToCoords(CurrentChord.Interval, CurrentChord.Quality, CurrentChord.Octave);
        if((indices.Item1 > 0 && verticalChange < 0) || ((indices.Item1 < menu_maxNumIntervals - 1) && verticalChange > 0)) {
            indices.Item1 += verticalChange;
            CurrentChord = elements[indices.Item1 * menu_maxNumQualities + indices.Item2];
        }
    }
}