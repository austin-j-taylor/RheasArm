using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using static Music_Pitches;

public class Canvas_StringElement : MonoBehaviour {

    private const float maxTimeInReplaying = 0.05f; // 50 ms

    public enum StringState { Silent, Playing, Replaying };
    private enum StrumState { NotStrumming, MouseBelow, MouseAbove, Striking };

    public StringState State { get; private set; }
    private StrumState Strumming { get; set; }

    private int stringIndex;
    private Canvas_Brach parent;
    private AudioSource source;
    private Button button;
    private KeyCode playKeyCode;

    private bool justStruckString;
    private float tInReplaying;

    private void Awake() {
        parent = GetComponentInParent<Canvas_Brach>();
        source = GetComponent<AudioSource>();
        button = GetComponent<Button>();
    }
    public void Initialize(KeyCode playKeyCode, int stringIndex) {
        this.playKeyCode = playKeyCode;
        this.stringIndex = stringIndex;

        State_ToSilent();
        Strumming = StrumState.NotStrumming;
    }

    private void Update() {
        // Strumming Transitions
        switch (Strumming) {
            case StrumState.NotStrumming:
                if (Input.GetKey(KeyCode.Mouse0)) {
                    if (Input.mousePosition.y - transform.position.y > 0) {
                        Strumming = StrumState.MouseAbove;
                    } else {
                        Strumming = StrumState.MouseBelow;
                    }
                }
                break;
            case StrumState.MouseBelow:
                if (!Input.GetKey(KeyCode.Mouse0)) {
                    Strumming = StrumState.NotStrumming;
                } else if (Input.mousePosition.y - transform.position.y > 0) {

                    Strumming = StrumState.Striking;
                }
                break;
            case StrumState.MouseAbove:
                if (!Input.GetKey(KeyCode.Mouse0)) {
                    Strumming = StrumState.NotStrumming;
                } else if (Input.mousePosition.y - transform.position.y <= 0) {
                    Strumming = StrumState.Striking;
                }
                break;
            case StrumState.Striking:
                if (!Input.GetKey(KeyCode.Mouse0)) {
                    Strumming = StrumState.NotStrumming;
                } else if (Input.mousePosition.y - transform.position.y > 0) {
                    Strumming = StrumState.MouseAbove;
                } else {
                    Strumming = StrumState.MouseBelow;
                }
                break;
        }
        // Actions
        switch (Strumming) {
            case StrumState.NotStrumming:
                justStruckString = false;
                break;
            case StrumState.MouseBelow:
                justStruckString = false;
                break;
            case StrumState.MouseAbove:
                justStruckString = false;
                break;
            case StrumState.Striking:
                justStruckString = true;
                break;
        }

        // State Transitions
        switch (State) {
            case StringState.Silent:
                if (HasStruckString()) {
                    State_ToPlaying();
                }
                break;
            case StringState.Playing:
                if (HasStruckString()) {
                    State_ToReplaying();
                } else if (!source.isPlaying) {
                    State_ToSilent();
                }
                break;
            case StringState.Replaying:
                if(tInReplaying > maxTimeInReplaying) {
                    State_ToPlaying();
                }
                break;
        }

        // Actions
        switch (State) {
            case StringState.Silent:
                State_InSilent();
                break;
            case StringState.Playing:
                State_InPlaying();
                break;
            case StringState.Replaying:
                State_InReplaying();
                break;
        }
    }

    // State Transitions
    private bool HasStruckString() {
        // Check if the mouse has crossed over the string between this frame and last frame while holding the button down
        return justStruckString || Input.GetKeyDown(playKeyCode);
    }
    private void State_ToSilent() {

        State = StringState.Silent;
    }
    private void State_ToPlaying() {
        // Update this pitch to reflect the chord menu
        // Should only update this specific string's pitch
        // move that from Strings to StringElement, get the cord qual/interval from here
        SetIntervalAndQuality(parent.ChordMenu.CurrentChord.Interval, parent.ChordMenu.CurrentChord.Quality, parent.ChordMenu.CurrentChord.Octave);
        source.Play();
        source.volume = 1;

        State = StringState.Playing;
    }
    private void State_ToReplaying() {
        tInReplaying = 0;

        State = StringState.Replaying;
    }
    // Actions
    private void State_InSilent() {

    }
    private void State_InPlaying() {
    }
    private void State_InReplaying() {
        // Need to briefly mute/fade out the string before playing it again to prevent popping noise
        source.volume *= 0.8f;
        tInReplaying += Time.deltaTime;
    }

    public void OnClick() {
        if (Input.GetKey(KeyCode.Mouse0)) // it's also called while right-clicking
            State_ToPlaying();
    }
    public void OnEnter() {
        if (Input.GetKey(KeyCode.Mouse0))
            State_ToPlaying();
    }

    public void SetPitch(float pitch) {
        source.pitch = pitch;
    }
    public void SetIntervalAndQuality(ChordInterval interval, ChordQuality quality, ChordOctave octave) {
        switch (quality) {
            case ChordQuality.dim:
                SetPitch(majorPitches[(int)interval] * (octave == ChordOctave.One ? 1 : 2) * dimPitches[stringIndex]);
                break;
            case ChordQuality.min:
                SetPitch(majorPitches[(int)interval] * (octave == ChordOctave.One ? 1 : 2) * minPitches[stringIndex]);
                break;
            case ChordQuality.maj:
                SetPitch(majorPitches[(int)interval] * (octave == ChordOctave.One ? 1 : 2) * majPitches[stringIndex]);
                break;
            case ChordQuality.aug:
                SetPitch(majorPitches[(int)interval] * (octave == ChordOctave.One ? 1 : 2) * augPitches[stringIndex]);
                break;
            case ChordQuality.none:
                break;
        }
    }
}
