using System.Collections.Generic;
using UnityEngine;
using static Tile;

public class Level {
    [System.Serializable]
    public struct Path {
        public Vector2 direction;
        public Vector2 position;
    }


    public int width;
    public int height;

    public SerializableTile[,] tiles;

    public List<Path> paths;

    public Level(int width, int height) {

        this.width = width;
        this.height = height;
        tiles = new SerializableTile[width, height];
    }

}
