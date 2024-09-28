using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FloodManager : MonoBehaviour {


    GridManager gridManager;
    public List<GameObject> waterTiles;
    public List<Sprite> sprites;
    public GameObject waterTilePrefab;


    [HideInInspector]
    public static FloodManager instance;


    public void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple FloodManagers detected, destroying this one");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }



    private void OnDrawGizmos() {
        if (gridManager == null) {
            return;
        }

        // Debug water connections
        for (int x = 0; x < gridManager.width; x++) {
            for (int y = 0; y < gridManager.height; y++) {
                Tile tile = gridManager.tiles[x, y];
                if (tile.waterLevel > 0) {
                    Gizmos.color = new Color(0.4f, 0.67f, 1f, 1.0f);
                } else {
                    Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                }
                Gizmos.DrawSphere(new Vector3(x, y, 0), 0.1f);
            }
        }
    }

    private void OnGUI() {

        // Draw water level and height text
        for (int x = 0; x < gridManager.width; x++) {
            for (int y = 0; y < gridManager.height; y++) {
                Tile tile = gridManager.tiles[x, y];
                Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(x, y, 0));
                GUI.Label(new Rect(pos.x, Screen.height - pos.y, 100, 100), tile.waterLevel.ToString());
                GUI.Label(new Rect(pos.x, Screen.height - pos.y + 10, 100, 100), tile.heightLevel.ToString());
            }
        }

    }


    public void Start() {
        gridManager = GridManager.instance;

        int width = gridManager.width;
        int height = gridManager.height;

        for (int x = 0; x < width - 1; x++) {
            for (int y = 0; y < height - 1; y++) {
                // GameObject tileObject = Instantiate(waterTilePrefab, new Vector3(x + .25f, y + .25f, 0), Quaternion.identity);
                GameObject tileObject = Instantiate(waterTilePrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                tileObject.transform.SetParent(transform);
                waterTiles.Add(tileObject);
            }
        }
        UpdateWaterTiles();
    }

    struct MarchingTile {
        public bool x, xx, y, xy;
    }

    MarchingTile GetAt(int x, int y) {
        x = Mathf.Clamp(x, 0, gridManager.width - 2);
        y = Mathf.Clamp(y, 0, gridManager.height - 2);
        try {
            return new MarchingTile {
                x = gridManager.tiles[x, y].waterLevel > 0,
                xx = gridManager.tiles[x + 1, y].waterLevel > 0,
                y = gridManager.tiles[x, y + 1].waterLevel > 0,
                xy = gridManager.tiles[x + 1, y + 1].waterLevel > 0
            };
        } catch { }

        return new MarchingTile();
    }

    MarchingTile RotateCW(MarchingTile mt) {
        return new MarchingTile {
            x = mt.xx,
            y = mt.x,
            xy = mt.y,
            xx = mt.xy
        };
    }

    bool IsEqual(MarchingTile a, MarchingTile b) {
        return a.x == b.x && a.xx == b.xx && a.y == b.y && a.xy == b.xy;
    }

    int IsEqualAllRotations(MarchingTile a, MarchingTile b) {
        if (IsEqual(a, b)) {
            return 0;
        }
        if (IsEqual(RotateCW(a), b)) {
            return 1;
        }
        if (IsEqual(RotateCW(RotateCW(a)), b)) {
            return 2;
        }
        if (IsEqual(RotateCW(RotateCW(RotateCW(a))), b)) {
            return 3;
        }
        return -1;
    }

    int MatchTile(MarchingTile mt) {
        // jakie gowno what the sigma
        var possible = new List<MarchingTile>{
            new() { x = false, xx = false, y = false, xy = false },
            new() { x = true, xx = false, y = false, xy = false },
            new() { x = true, xx = true, y = false, xy = false },
            new() { x = true, xx = true, y = true, xy = false },
            new() { x = true, xx = true, y = true, xy = true },
            new() { x = false, xx = true, y = true, xy = false },
        };

        for (int i = 0; i < possible.Count; i++) {
            var eq = IsEqualAllRotations(mt, possible[i]);
            if (eq != -1) {
                return eq * 10 + i;
            }
            if (i == 5) {
                print("co sie odjebalo" + eq);
            }
        }

        return 0; // ?????
    }

    public void UpdateWaterTiles() {
        if (gridManager == null) {
            return;
        }
        for (int i = 0; i < waterTiles.Count; i++) {
            int x = i / (gridManager.width - 1);
            int y = i % (gridManager.height - 1);

            MarchingTile mt = GetAt(x, y);
            int match = MatchTile(mt);
            var sr = waterTiles[i].GetComponent<SpriteRenderer>();
            sr.sprite = sprites[match % 10];
            sr.transform.rotation = Quaternion.Euler(0, 0, match / 10 * 90);
        }
    }

    private void Update() {
        UpdateWaterTiles();
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            for (int x = 0; x < gridManager.width; x++) {
                for (int y = 0; y < gridManager.height; y++) {
                    gridManager.tiles[x, y].waterLevel = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Tick();
        }
    }
    struct TempTile {
        public int x, y, waterLevel;
    };

    public void StartTicking() {
        InvokeRepeating(nameof(Tick), 0, 0.5f);
    }

    void Tick() {
        if (GameOver.instance.isGameOver) {
            CancelInvoke(nameof(Tick));
            return;
        }
        List<Tile.SerializableTile> buffer = new();
        for (int x = 0; x < gridManager.width; x++) {
            for (int y = 0; y < gridManager.height; y++) {
                Tile tile = gridManager.tiles[x, y];
                if (!(tile.waterLevel > 0)) { continue; }
                var directions = new List<Vector2> {
                    new(1, 0),
                    new(-1, 0),
                    new(0, 1),
                    new(0, -1),
                };
                List<Tile> floodableNeighbors = new();
                for (int i = 0; i < 4; i++) {
                    int nx = x + (int)directions[i].x;
                    int ny = y + (int)directions[i].y;
                    if (gridManager.IsOOB(nx, ny)) {
                        continue;
                    }
                    Tile neighbor = gridManager.tiles[nx, ny];
                    if (neighbor.heightLevel > tile.heightLevel + (0.5f * tile.waterLevel)) {
                        continue;
                    }
                    floodableNeighbors.Add(neighbor);
                }

                foreach (var neighbor in floodableNeighbors) {
                    var n = neighbor.ToSerializableTile();
                    if (n.waterLevel == 0) {
                        n.waterLevel = (int)(tile.waterLevel * (2f / 3f) * (1f / floodableNeighbors.Count));
                    } else {
                        float frac = 1f / 3f;
                        // Push 1/3 of water to the smaller one
                        if (n.waterLevel < tile.waterLevel) {
                            int diff = tile.waterLevel - n.waterLevel;
                            n.waterLevel += (int)(diff * frac);
                            tile.waterLevel -= (int)(diff * frac);
                        } else {
                            int diff = n.waterLevel - tile.waterLevel;
                            n.waterLevel -= (int)(diff * frac);
                            tile.waterLevel += (int)(diff * frac);
                        }
                    }
                    buffer.Add(n);
                }
            }
        }

        int endLevel = 0;

        foreach (var tile in buffer) {
            if (gridManager.tiles[tile.x, tile.y].waterLevel == tile.waterLevel) {
                continue;
            }
            gridManager.tiles[tile.x, tile.y].waterLevel = tile.waterLevel;
            gridManager.tiles[tile.x, tile.y].UpdateTile();
            endLevel++;
        }

        if (endLevel == 0) {
            CancelInvoke(nameof(Tick));
            GameOver.instance.Win();
            return;
        }
    }
}
