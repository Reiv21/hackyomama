using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [HideInInspector]
    public static LevelManager instance;

    public void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple LevelManagers detected, destroying this one");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public List<Sprite> tileSprites;
}
