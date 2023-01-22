using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileReader : MonoBehaviour
{
    #region Properties

    public Tilemap tilemap;

    [Header("Hallway Tiles")]
    public List<Tile> middle = new List<Tile>();
    public List<Tile> sideWall = new List<Tile>();
    public List<Tile> straight = new List<Tile>();
    public List<Tile> corner = new List<Tile>();
    public List<Tile> deadEnd = new List<Tile>();

    #endregion

    #region TILE READING

    public Tiles.Tile GetTileAt(int x, int y)
    {
        Vector3Int pos = new Vector3Int(x, y);

        Tile tile = tilemap.GetTile(pos) as Tile;


        // Check if the tile is Empty
        if (tile == null)
        {
            // No tile set, return empty
            Tiles.Tile empty = new Tiles.Tile(Tiles.Type.empty, Tiles.Orientation.left);
            return empty;
        }

        // If there is a Tile, check what kind

        #region Tile Check
        // STRAIGHT
        if (tile == straight.Contains(tile))
        {
            // Check Orientation
            if (tile == straight[0]) // Vertical
            {
                return new Tiles.Tile(Tiles.Type.straight, Tiles.Orientation.up);
            }
            else                    // Horizontal
            {
                return new Tiles.Tile(Tiles.Type.straight, Tiles.Orientation.left);
            }
        }

        // SIDEWALL
        if (tile == sideWall.Contains(tile))
        {
            // Check Orientation
            if (tile == sideWall[0])
            { return new Tiles.Tile(Tiles.Type.sideWall, Tiles.Orientation.left); } // Left
            else if (tile == sideWall[1])
            { return new Tiles.Tile(Tiles.Type.sideWall, Tiles.Orientation.up); }   // Up
            else if (tile == sideWall[2])
            { return new Tiles.Tile(Tiles.Type.sideWall, Tiles.Orientation.right); } // Right
            else
            { return new Tiles.Tile(Tiles.Type.sideWall, Tiles.Orientation.down); }  // Down
        }

        // CORNER
        if (tile == corner.Contains(tile))
        {
            // Check Orientation    (first wall clockwise)
            if (tile == corner[0])
            { return new Tiles.Tile(Tiles.Type.corner, Tiles.Orientation.left); } // Left
            else if (tile == corner[1])
            { return new Tiles.Tile(Tiles.Type.corner, Tiles.Orientation.up); }   // Up
            else if (tile == corner[2])
            { return new Tiles.Tile(Tiles.Type.corner, Tiles.Orientation.right); } // Right
            else
            { return new Tiles.Tile(Tiles.Type.corner, Tiles.Orientation.down); }  // Down
        }

        // DEAD END
        if (tile == deadEnd.Contains(tile))
        {
            // Check Orientation
            if (tile == deadEnd[0])
            { return new Tiles.Tile(Tiles.Type.deadEnd, Tiles.Orientation.left); } // Left
            else if (tile == deadEnd[1])
            { return new Tiles.Tile(Tiles.Type.deadEnd, Tiles.Orientation.up); }   // Up
            else if (tile == deadEnd[2])
            { return new Tiles.Tile(Tiles.Type.deadEnd, Tiles.Orientation.right); } // Right
            else
            { return new Tiles.Tile(Tiles.Type.deadEnd, Tiles.Orientation.down); }  // Down
        }
        #endregion

        // Default, if it's nothing else, it's middle piece

        // MIDDLE SECTION
        return new Tiles.Tile(Tiles.Type.middle, Tiles.Orientation.left);
    }

    #endregion
}
