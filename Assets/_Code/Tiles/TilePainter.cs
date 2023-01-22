using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    #region Properties

    public Tilemap tilemap;

    [Header("Hallway Tiles")]
    public List<Tile> middle = new List<Tile>();
    public List<Tile> sideWall = new List<Tile>();
    public List<Tile> straight = new List<Tile>();
    public List<Tile> corner = new List<Tile>();
    public List<Tile> deadEnd = new List<Tile>();

    //[Header("Water Tiles")]
    //public List<Tile> waterTile = new List<Tile>();

    //[Header("Sacrifice Tiles")]
    //public List<Tile> sacrificeTiles = new List<Tile>();

    #endregion

    #region PAINTING

    #endregion
}
