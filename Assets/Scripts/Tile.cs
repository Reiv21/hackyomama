
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

    public struct SerializableTile {
        public int x, y;
        public int waterLevel;

        public TileType type;
    }

    public int x, y;
    public int waterLevel;
    public TileType type = TileType.Grass;


    public void UpdateTile() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (LevelManager.instance.tileSprites.Count - 1 < (int)type) {
            Debug.LogError("Tile type not added to tileSprites list.");
            spriteRenderer.sprite = LevelManager.instance.tileSprites[0];
        } else {
            spriteRenderer.sprite = LevelManager.instance.tileSprites[(int)type];
        }
        if (waterLevel > 0)
            spriteRenderer.color = new Color(0.4f, 0.67f, 1, waterLevel / 250f);
    }

    public SerializableTile ToSerializableTile() {
        return new SerializableTile {
            x = x,
            y = y,
            waterLevel = waterLevel,
            type = type
        };
    }

    public void FromSerializableTile(SerializableTile serializableTile) {
        x = serializableTile.x;
        y = serializableTile.y;
        waterLevel = serializableTile.waterLevel;
        type = serializableTile.type;
        UpdateTile();
    }
}
