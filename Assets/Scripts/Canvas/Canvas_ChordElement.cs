using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using static Music_Pitches;

/// <summary>
/// A single element of the chord menu representing a chord (e.g. ii_diminished)
/// </summary>
public class Canvas_ChordElement : MonoBehaviour {

    // Properties
    public ChordInterval Interval { get; set; }
    public ChordQuality Quality { get; set; }
    public ChordOctave Octave { get; set; }

    public RectTransform rectTransform;
    private Image imageQuality;
    private Button button;
    private Canvas_Chord parent;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        imageQuality = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void Initialize(ChordInterval interval, ChordQuality quality, ChordOctave octave, Canvas_Chord parent) {
        Interval = interval;
        Quality = quality;
        Octave = octave;
        this.parent = parent;

        switch (quality) {
            case ChordQuality.dim:
                imageQuality.sprite = GameManager.ResourceManager.Canvas_ChordElement_dim;
                break;
            case ChordQuality.min:
                imageQuality.sprite = GameManager.ResourceManager.Canvas_ChordElement_min;
                break;
            case ChordQuality.maj:
                imageQuality.sprite = GameManager.ResourceManager.Canvas_ChordElement_maj;
                break;
            case ChordQuality.aug:
                imageQuality.sprite = GameManager.ResourceManager.Canvas_ChordElement_aug;
                break;
            case ChordQuality.none:
                gameObject.SetActive(false);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }

    public void OnSelect() {
        //Debug.Log(Interval + ", " + Quality + ", " + Octave);
        parent.CurrentChord = this;
    }
}
