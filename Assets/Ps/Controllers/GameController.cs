/**
 * Copyright 2012 Douglas Linder
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using n.Core;
using Ps.Views.Game;
using Ps.Model;
using System;

namespace Ps .Controllers
{
	public class GameController : nController 
	{
    private GameState _state;
    
    public GameController() {
      _state = _stateFactory.State.As<GameState>();
    }

    public void Reset() {
      _stateFactory.Reset();
    }

		public void Index() {
      Launch<Index>();
		}
    
    public GameStateViewModel Start(GameObject parent) {
      if (!_state.Activated)
        _state.Reset();
      _state.Init(parent);
      var model = new GameStateViewModel(_state, _state.Camera());
      return model;
    }

    public UIStateViewModel UI() {
      var model = new UIStateViewModel() {
        Camera = _state.UiCamera(),
        Game = _state
      };
      return model;
    }

    /** Update to the next tick of the gamestate */
    public void Tick(float seconds) {
      _state.Update(seconds);
    }

    /** Check for and enqueue events */
    public void Poll() {
      _state.Poll();
    }

    /** Win view */
    public void Win() {
      _state.Score.Winner = WinState.PLAYER;
      Launch<Scoreboard>();
    }

    /** Win view */
    public void Lose() {
      _state.Score.Winner = WinState.AI;
      Launch<Scoreboard>();
    }

    /** Get the current score */
    public Score Score() {
      if (_state == null)
        return null;
      return _state.Score;
    }

    /** Return activation status */
    public bool Activated() {
      var rtn = _state.Activated;
      _state.Activated = true;
      return rtn;
    }

    /** Maybe add a high score? */
    public void UpdateScore(int points) {
      if (_state.Score == null) 
        _state.Score = new Score();
      _state.Score.HighScores.Update(points);
    }

    /** Return a list of high scores */
    public HighScores HighScores() {
      _state.Score.HighScores.Load();
      return _state.Score.HighScores;
    }
	}
}
