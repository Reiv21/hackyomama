using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [HideInInspector]
    public static LevelManager instance;
    public Tile.SerializableTile[] localSpecialTiles;
    public static Tile.SerializableTile[] specialTiles;
    public void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple LevelManagers detected, destroying this one");
            Destroy(gameObject);
            return;
        }

        specialTiles = localSpecialTiles;
        instance = this;
    }

    public List<Sprite> tileSprites;

    public void PlayButton()
    {
        FloodManager.instance.StartTicking();
        BuildingManager.canBuild = false;
    }
}
