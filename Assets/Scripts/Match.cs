﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
  public int game = 0;
  public int turn = 0;
  public int side = 0;

  public Ruleset ruleset;
  public List<Side> turnOrder = new List<Side>();
  public Dictionary<Side, int> games = new Dictionary<Side, int>();
  
  public delegate void GameBeganEvent(Match m, int game, Board b, Side[] sides);
  public delegate void GameEndEvent(Match m, Board b, Side winner);
  public delegate void MatchBeganEvent(Match m, Side[] sides);
  public delegate void MatchEndEvent(Match m, Side winner);
  public delegate void TurnEvent(Match m, int turn, Side[] sides);
  public delegate bool ValidMovePredicate(Board b, Tile t, Piece p);

  public event TurnEvent OnTurnBegan;
  public event TurnEvent OnTurnEnded;

  public event GameBeganEvent OnGameBegan;
  public event GameEndEvent OnGameEnded;

  public event MatchBeganEvent OnMatchBegan;
  public event MatchEndEvent OnMatchEnded;

  public event ValidMovePredicate OnMoveAttempted;

  public Board board { get; set; }

  public void RegisterSide(Side s)
  {
    turnOrder.Add(s);
  }

  public void BeginTurn()
  {
    side = 0;
    OnTurnBegan?.Invoke(this, turn, turnOrder.ToArray());
  }

  public void EndTurn()
  {
    OnTurnEnded?.Invoke(this, turn++, turnOrder.ToArray());
  }

  public void BeginMatch()
  {
    OnMatchBegan?.Invoke(this, turnOrder.ToArray());
    game = 0;
  }

  public void EndMatch(Side winner)
  {
    OnMatchEnded?.Invoke(this, winner);
  }

  public void BeginGame()
  {
    game++;
    OnGameBegan?.Invoke(this, game, board, turnOrder.ToArray());
    turn = 0;
  }

  public void EndGame(Side winner)
  {
    games[winner]++;
    OnGameEnded?.Invoke(this, board, winner);
  }

  public bool RegisterMove(Side s, Tile t, Piece p)
  {
    if (OnMoveAttempted?.Invoke(board, t, p)  == false)
    {
      return false;
    }

    t.piece = p;
    OnTurnEnded(this, turn, turnOrder.ToArray());
    return true;
  }
  
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}