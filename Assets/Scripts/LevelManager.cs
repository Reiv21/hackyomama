using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [HideInInspector]
    public static LevelManager instance;
    public Tile.SerializableTile[] localSpecialTiles;
    public static Tile.SerializableTile[] specialTiles;

    [HideInInspector]
    public int housesStart = 0;
    [HideInInspector]
    public int houseCount = 0;

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

    public void PlayButton() {
        FloodManager.instance.StartTicking();
        BuildingManager.canBuild = false;
        Jukebox.instance.PlayClick();
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
