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

namespace Ps.Views.Game
{
	public class Index : MonoBehaviour 
	{
    private nGraphicsPipe _pipe;
    
    private GameController _controller;
    
		void Start() {

      _controller = Pongstar.Get<GameController>();
      var model = _controller.Start(this.gameObject);
      
      /* setup camera */
      if (_pipe == null) {
        model.Camera.Active = true;
        _pipe = model.Camera.Pipe;
      }

      /* setup drawables */
      _pipe.Drawables.Clear();
      foreach (var c in model.Items) {
        _pipe.Drawables.Add(c);
      }
		}

		void Update() {
      var state = _controller.Activated();
      if (!state) {
        Start();
      }
      var delta = Time.deltaTime;
      _controller.Tick(delta);
      reasonLife += delta;
      noticeDone += delta;
		}

    /** Label display values */
    private float reasonLife = 2f;
    private float reasonSpan = 1f;
    private int reasonScore = 0;
    private string reasonMsg = "";

    private float noticeSpan = 2f;
    private float noticeDone = 0f;

    /** UI top and bottom */
    private bool _uiSetup = false;
    //private Rect[] _uiTop = new Rect[1];
    private Texture[] _uiTex = new Texture[1];
    private LData _msg;

    void OnGUI() {

      /** Events all happen here! */
      _controller.Poll();

      /** Draw the top and bottom ui */
      if (!_uiSetup) 
        _uiTex [0] = (Texture)Resources.Load("top");
      UITools.RepeatStrip(_uiTex [0], 18, nLayout.Distance(0, nEdge.LEFT), nLayout.Distance(0, nEdge.TOP));

      /* All the top elements */
      var score = _controller.Score();
      var block = new Rect(nLayout.Distance(3f), 0f, nLayout.PaddedFullWidth(3f), nLayout.Distance(10f));

      _msg = LPack.GameTopPlayer(score);
      GUI.Label(block, _msg.Msg, _msg.Style);

      _msg = LPack.GameTopAi(score);
      GUI.Label(block, _msg.Msg, _msg.Style);

      _msg = LPack.GameTopScore(score);
      GUI.Label(block, _msg.Msg, _msg.Style);

      if (score.LastPoints != 0) {
        reasonLife = 0f;
        reasonMsg = score.LastMesg;
        reasonScore = score.LastPoints;
        score.Update(0, "");
      }
      if (reasonLife < reasonSpan) {
        var topoff = nLayout.Distance(12.0f, nEdge.TOP);
        var coordinate = nLayout.Distance(2f);
        var factor = 1.0f - (reasonLife / reasonSpan);
        var sdisplay = reasonScore > 0 ? "+" + reasonScore : "" + reasonScore;
        var message = String.Format("{0} {1}", reasonMsg, sdisplay);
        GUI.Label(new Rect(coordinate, topoff, 2f, 0.5f), message, LPack.GameNoticeStyle(factor));
      } 

      if (noticeDone < noticeSpan) {
        var factor = 1f - noticeDone / noticeSpan;
        var data = LPack.GameTouchHere(factor);
        GUI.Label(new Rect(0, nLayout.Distance(10f, nEdge.BOTTOM), nLayout.PaddedFullWidth(0), nLayout.Distance(10)), data.Msg, data.Style);
      }
    }
	}
}