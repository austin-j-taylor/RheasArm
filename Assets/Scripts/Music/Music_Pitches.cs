using UnityEngine;
using UnityEditor;

/// <summary>
/// Contains information about the pitches of notes.
/// </summary>
public static class Music_Pitches {

    private const float P1 = 1;
    private const float M2 = 1.1224620220651190097605127326989f;
    private const float m3 = 1.1892070194965630274713177915311f;
    private const float M3 = 1.2599210474815920154602607695883f;
    private const float P4 = 1.3348395569852491499302820519093f;
    private const float D5 = 1.4142132879962817094351623082756f;
    private const float P5 = 1.4983067406247706646444384647374f;
    private const float A5 = 1.5874008506812788962548007534431f;
    private const float M6 = 1.6817926074512585924313217055212f;
    private const float M7 = 1.8877483701900731426894004256464f;
    private const float P8 = 2;

    // The audio sources on strings 3-4 are one octave higher, 5 is two octaves higher
    public static readonly float[] dimPitches = { P1, D5, P1, m3, P1 };
    public static readonly float[] minPitches = { P1, P5, P1, m3, P1 };
    public static readonly float[] majPitches = { P1, P5, P1, M3, P1 };
    public static readonly float[] augPitches = { P1, A5, P1, M3, P1 };
    public static readonly float[] majorPitches = {
        P1,
        M2,
        M3,
        P4,
        P5,
        M6,
        M7
    };

    public enum ChordInterval { I, ii, iii, IV, V, vi, viio };
    public enum ChordQuality { dim, min, maj, aug, none };
    public enum ChordOctave { One, Two };

}