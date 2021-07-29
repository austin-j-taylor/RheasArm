using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GlobalResourceManager ResourceManager;

    void Awake() {
        ResourceManager = GetComponent<GlobalResourceManager>();
    }
}
