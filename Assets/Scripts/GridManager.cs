using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour {

    [HideInInspector]
    public static GridManager instance;

    public int width;
    public int height;

    public GameObject tilePrefab;

    public Level level; // unused, future use is for loading

    public Tile[,] tiles;

    public SpriteRenderer selection;


    [SerializeField] BuildingManager buildingManager;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple GridManagers detected, destroying this one");
            Destroy(gameObject);
            return;
        }

        instance = this;
        tiles = new Tile[width, height];
    }

    void Start() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                Tile tile = tileObject.GetComponent<Tile>();
                tile.x = x;
                tile.y = y;
                tiles[x, y] = tile;
                tile.heightLevel = (int)((Mathf.PerlinNoise((float)x / width, (float)y / height) * 100f) * 0.5f);
                tile.UpdateTile();
            }
        }
        foreach (var tile in LevelManager.specialTiles) {
            tiles[tile.x, tile.y].type = tile.type;
            tiles[tile.x, tile.y].waterLevel = tile.waterLevel;
            if (tile.heightLevel != -1)
            {
                tiles[tile.x, tile.y].heightLevel = tile.heightLevel;
            }

            if (tile.type == Tile.TileType.House) {
                LevelManager.instance.housesStart++;
                LevelManager.instance.houseCount++;
            }
            tiles[tile.x, tile.y].UpdateTile();
        }
    }

    public bool IsOOB(int x, int y) {
        return x < 0 || x >= width || y < 0 || y >= height;
    }

    public bool HasSurroundingHouses(int x, int y) {
        if (IsOOB(x, y)) {
            return false;
        }
        if (tiles[x, y].type == Tile.TileType.House) {
            return true;
        }
        var directions = new Vector2[] {
            new (0, 1),
            new (0, -1),
            new (1, 0),
            new (-1, 0),
            new (1,1),
            new (-1,1),
            new (1,-1),
            new (-1,-1),
        };
        foreach (var dir in directions) {
            if (IsOOB(x + (int)dir.x, y + (int)dir.y)) {
                continue;
            }
            if (tiles[x + (int)dir.x, y + (int)dir.y].type == Tile.TileType.House) {
                return true;
            }
        }
        return false;
    }

    Vector2 lastMousePos;
    void FixedUpdate() {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.FloorToInt(mousePos.x + 0.5f);
        int y = Mathf.FloorToInt(mousePos.y + 0.5f);

        selection.transform.position = Vector3.Lerp(selection.transform.position, lastMousePos, 0.25f);
        selection.color = new Color(1, 1, 1, 0.5f);
        if (IsOOB(x, y)) {
            // OOB, hide
            selection.color = new Color(1, 1, 1, 0);
            return;
        }


        Tile tile = tiles[x, y];

        bool surr = HasSurroundingHouses(x, y);
        buildingManager.UpdateExpensiveSign(surr);

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() ) {
            if (!buildingManager.CanBuild(surr) || tile.type != Tile.TileType.Grass) return;

            PlaceTile(x, y, (Tile.TileType)buildingManager.selectedIndex);
            buildingManager.Build(surr);
            Debug.LogWarning("Buying expensive land? " + surr);
            if (buildingManager.selectedIndex == 2) {
                tile.heightLevel = 120;
                tile.type = Tile.TileType.Building2;
                Jukebox.instance.PlayPlace0();
                tile.UpdateTile();
            } else if (buildingManager.selectedIndex == 1) {
                tile.heightLevel = -10;
                tile.type = Tile.TileType.Bed;
                Jukebox.instance.PlayPlace1();
                tile.UpdateTile();
            }
        }

        lastMousePos = new Vector2(x, y);
    }


    public void PlaceTile(int x, int y, Tile.TileType type) {
        if (IsOOB(x, y)) {
            return;
        }
        tiles[x, y].type = type;
        tiles[x, y].UpdateTile();
    }
}
