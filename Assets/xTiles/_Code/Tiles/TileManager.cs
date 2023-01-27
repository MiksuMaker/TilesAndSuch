using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region PROPERTIES

    private TilePainter painter;
    private TileReader reader;

    public int gridRows = 10;
    public int gridColumns = 10;

    // 2D Tile Array
    Tiles.Tile[,] tileGrid;



    #endregion


    #region SETUP
    private void Awake()
    {
        painter = GetComponent<TilePainter>();
        reader = GetComponent<TileReader>();
    }

    private void Start()
    {
        // Set up the tile Array
        SetUpTileArray();

        // Print the array
    }


    private void SetUpTileArray()
    {
        // Initialize the Array
        tileGrid = new Tiles.Tile[gridRows, gridColumns];

        // Set the tile types
        for (int x = 0; x < gridRows; x++)
        {
            for (int y = 0; y < gridColumns; y++)
            {
                // Check if there already is a tile
                Tiles.Tile tile = reader.GetTileAt(x, y);
                if (tile.type != Tiles.Type.empty)
                {
                    // There is already a tile
                    // -> Get Tile Type
                    // --> Save the Tile Type into the TileArray
                    //SetNewTileAt(type, x, y);

                    Debug.Log("Tiletype " + tile.type.ToString() + " at " + x + "," + y);
                    Debug.DrawRay(new Vector3(x * 2, 0, y * 2), Vector3.up, Color.green, 2f);
                }
                else
                {
                    // It's empty
                    // -> Put a random tile there
                    //SetRandomTileAt(x, y);            // DEACTIVATED
                }
            }
        }
    }
    #endregion




}




static public class Tiles
{
    public enum Type
    {
        empty, middle, sideWall, straight, corner, deadEnd
    }

    public enum Orientation
    {
        left, up, right, down
    }

    public struct Tile
    {
        // Variables
        public Type type;
        public Orientation dir;

        // Constructor
        public Tile(Type type, Orientation dir)
        {
            this.type = type;
            this.dir = dir;
        }

    }
}
