using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            tiles[tile.x, tile.y].heightLevel = tile.heightLevel;

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

        if (Input.GetMouseButtonDown(0)) {
            if ( !buildingManager.CanBuild() || tile.type != Tile.TileType.Grass) return;

            PlaceTile(x, y, (Tile.TileType)buildingManager.selectedIndex);
            buildingManager.Build();
            if (buildingManager.selectedIndex == 2) {
                tile.heightLevel = 120;
                tile.UpdateTile();
            } else if (buildingManager.selectedIndex == 1) {
                tile.heightLevel = -10;
                tile.UpdateTile();
            }
        } else if (Input.GetMouseButton(1)) {
            tile.waterLevel = 220;
            tile.UpdateTile();
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
