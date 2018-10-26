﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Tile class provides an abstraction for the referential location of a tile on the game board
/// and it's state.
/// </summary>
public class Tile : MonoBehaviour
{
  /// <summary>
  /// The row on the game board that the tile resides on
  /// </summary>
  public int row;

  /// <summary>
  /// The column on the game board that the tile resides on
  /// </summary>
  public int column;

  /// <summary>Who occupies the tile, or null if tile is unoccupied. 
  /// When set, triggers the <see cref="OnPiecePlaced"/>event.</summary>
  /// <value>Backed by the private field, _piece</value>
  public Piece piece
  {
    get { return _piece; }
    set {
      _piece = value;
      OnPiecePlaced?.Invoke(this, value);
    }
  }
  private Piece _piece;

  /// <summary>
  /// Notifications for when a piece is placed.
  /// </summary>
  /// <param name="tile">The location for the piece</param>
  /// <param name="piece">The piece being placed</param>
  public delegate void PiecePlacedEvent(Tile tile, Piece piece);

  /// <summary>
  /// Fired whenever a piece is assigned to a tile. Also fires when
  /// null is assigned to the piece property of a tile.
  /// </summary>
  public event PiecePlacedEvent OnPiecePlaced;

  /// <summary>
  /// Responds to the collision-stay unity physics event. The
  /// method will try to put the tile to sleep for at least a 
  /// frame to avoid the tile from popping out of place unexpectedly.
  /// </summary>
  /// <param name="collision"></param>
  private void OnCollisionStay(Collision collision)
  {
    GetComponent<Rigidbody>().Sleep();
  }

#if UNITY_EDITOR
  /// <summary>
  /// A per-Tile map from Input Button names that double as descriptors
  /// for debug UI toggles. These map to messages that are relevant to
  /// the debug UI toggle (i.e. per-tile AI Scores).
  /// </summary>
  private Dictionary<string, string> registeredDebugStrings = new Dictionary<string, string>();

  /// <summary>
  /// Store a new message for a specific mode
  /// </summary>
  /// <param name="mode">The mode <seealso cref="HUD.debugMode"/></param>
  /// <param name="text">The message for the mode</param>
  public void RegisterDebug(string mode, string text)
  {
    registeredDebugStrings[mode] = text;
  }

  /// <summary>
  /// Fetches the message, if any, for the given mode
  /// </summary>
  /// <param name="mode">The mode <seealso cref="HUD.debugMode"/></param>
  /// <returns>The message if a message has been registered previously with 
  /// <see cref="RegisterDebug(string, string)"/> or null, otherwise.</returns>
  public string RegisteredDebugText(string mode)
  {
    string ret = null;
    registeredDebugStrings.TryGetValue(mode, out ret);
    return ret;
  }
#endif
}
