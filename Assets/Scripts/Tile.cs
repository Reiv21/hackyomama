
using System;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum TileType {
        // no water here,
        // sprites are set in the levelmanager instance (using ints not enums bc unity is shit)
        Grass,
        House,
        Building2,
        Tree,
    }

    [Serializable]
    public struct SerializableTile {
        public int x, y;
        public int waterLevel;
        public int heightLevel;

        public TileType type;
    }

    public int x, y;
    public int waterLevel;
    public int heightLevel;
    public TileType type = TileType.Grass;


    public void UpdateTile() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (LevelManager.instance.tileSprites.Count - 1 < (int)type) {
            Debug.LogError("Tile type not added to tileSprites list.");
            spriteRenderer.sprite = LevelManager.instance.tileSprites[0];
        } else {
            spriteRenderer.sprite = LevelManager.instance.tileSprites[(int)type];
        }

        if (type == TileType.Grass) {
            spriteRenderer.color = Color.Lerp(new Color(0.4f, 0.65f, 0.4f), Color.white, heightLevel / 150f);
        }
        else
        {
            spriteRenderer.color = Color.white;

        }
        if (type == TileType.House) {
            if (waterLevel > 0) {
                type = TileType.Grass;
                LevelManager.instance.houseCount--;
                UpdateTile();
                if (LevelManager.instance.houseCount == 0) {
                    GameOver.instance.Lost();
                }
            }
        }
    }

    public SerializableTile ToSerializableTile() {
        return new SerializableTile {
            x = x,
            y = y,
            waterLevel = waterLevel,
            heightLevel = heightLevel,
            type = type
        };
    }

    public void FromSerializableTile(SerializableTile serializableTile) {
        x = serializableTile.x;
        y = serializableTile.y;
        waterLevel = serializableTile.waterLevel;
        heightLevel = serializableTile.heightLevel;
        type = serializableTile.type;
        UpdateTile();
    }
}
