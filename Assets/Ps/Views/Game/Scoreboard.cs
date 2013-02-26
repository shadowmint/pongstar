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
using n.Gfx;
using Ps.Controllers;
using Ps.Model;
using n.Core;
using System.Diagnostics;
using Ps.Tests.Model;
using Ps.Tests;
using System;
using n.Utils;
using System.Collections.Generic;
using Ps.Model.Object;
using System.Linq;

namespace Ps.Views.Game
{
  public class Scoreboard : MonoBehaviour 
  {
    private nGraphicsPipe _pipe;
    
    private GameController _controller;

    private Score _score;

    private Dictionary<int, HighScore> _highscores;

    private Collectables _c;

    private Field _f;

    private bool _ready = false;

    private float _timer = 0;

    void Start() {
      
      _controller = Pongstar.Get<GameController>();
      var model = _controller.UI();
      var sm = _controller.Score();
      if (sm != null)
        _score = sm.As<Score>();

      /* Fudge score for tests */
      else {
        _score = new Score();
        _score.Update(53223 + nRand.Int(100, 50), "Lucky");
        _score.Winner = WinState.PLAYER;
      }

      /* Add to high scores? */
      _controller.UpdateScore(_score.Points);
      _highscores = _controller.HighScores().Scores;

      /* Win noise~ */
      model.Game.Audio = new nAudio(this.gameObject);
      Ps.Model.Actions.Game.SetupAudio(model.Game.Audio);
      if (_score.Winner == WinState.AI)
        model.Game.Audio.Play(Ps.Model.Actions.Game.WIN_LOSE_CHANNEL, Ps.Model.Actions.Game.LOSE_SOUND, 1f);
      else
        model.Game.Audio.Play(Ps.Model.Actions.Game.WIN_LOSE_CHANNEL, Ps.Model.Actions.Game.WIN_SOUND, 1f);

      /* Background~ */
      _f = new Field(null, model.Camera);

      /* setup camera */
      if (_pipe == null) {
        model.Camera.Active = true;
        _pipe = model.Camera.Pipe;
        _pipe.Drawables.Add(_f.Display);
        
        _c = new Collectables(model.Game);
        _pipe.Drawables.Add(_c.Display);
      }

      _ready = true;
    }
    
    void Update() {
      if (_ready) {
        _c.Update(Time.deltaTime);
        _f.Update(Time.deltaTime);
        _timer += Time.deltaTime;
      }
    }
    
    void OnGUI() {

      if (!_ready) 
        return;

      if (_score == null)
        return;

      var top = nLayout.Distance(10f, nEdge.TOP);
      float offset = 0f;
      LData data;
      if (_score.Winner == WinState.PLAYER) {
        data = LPack.ScoreWinMessage(_score);
        offset = data.Style.CalcHeight(new GUIContent(data.Msg), nLayout.PaddedFullWidth(0f)) / 2;
        GUI.Label(new Rect(nLayout.Distance(15f), top + offset, nLayout.PaddedFullWidth(15f), 0.5f), data.Msg, data.Style);
      } else if (_score.Winner == WinState.AI) {
        data = LPack.ScoreLoseMessage(_score);
        offset = data.Style.CalcHeight(new GUIContent(data.Msg), nLayout.PaddedFullWidth(0f)) / 2;
        GUI.Label(new Rect(nLayout.Distance(15f), top + offset, nLayout.PaddedFullWidth(15f), 0.5f), data.Msg, data.Style);
      }

      top = offset + nLayout.Distance(30f);
      data = LPack.ScoreHighscoreHeader();
      GUI.Label(new Rect(0, top, nLayout.PaddedFullWidth(0), 0.5f), data.Msg, data.Style);

      data = LPack.ScoreHighscoreItem(_highscores[0], 0, _highscores.Count);
      var jump = data.Style.CalcHeight(new GUIContent(data.Msg), nLayout.PaddedFullWidth(0f)) * 1.3f;
      var root_offset = top + jump * 2;
      for (var i = 0; i < _highscores.Count; ++i) {
        data = LPack.ScoreHighscoreItem(_highscores[i], i, _highscores.Count);
        var rect = nLayout.EBlock(nLayout.Center(nAxis.X), root_offset + i * jump, nLayout.PaddedFullWidth(10), nLayout.Distance(10));
        GUI.Label(rect, data.Msg, data.Style);
      }

      data = LPack.Credits(_timer);
      var theight = data.Style.CalcHeight(new GUIContent(data.Msg), nLayout.PaddedFullWidth(0f));
      top = nLayout.Distance(0f, nEdge.BOTTOM) - theight;
      GUI.Label(new Rect(nLayout.Distance(10f), top, nLayout.PaddedFullWidth(0), 0.5f), data.Msg, data.Style);

      data = LPack.ScoreButton();
      if (GUI.Button(new Rect(
        nLayout.Center(nAxis.X) - nLayout.PaddedFullWidth(10) / 2, 
        nLayout.Distance(39, nEdge.BOTTOM), 
        nLayout.PaddedFullWidth(10), 
        nLayout.Distance(20)),
        data.Msg, data.Style)) {
        _controller.Reset();
        _controller.Index();
      }
    }
  }
}