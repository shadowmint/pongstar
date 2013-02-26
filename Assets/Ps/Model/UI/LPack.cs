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
using System.Collections.Generic;
using n.Gfx;
using System;
using Ps.Model.Object;
using Ps.Model.Events;
using n.Utils;
using Ps.Model.Actions;
using Ps.Model.UI;

namespace Ps.Model
{
  public class LData 
  {
    public string Msg { get; set; }
    public GUIStyle Style { get; set; }
  }

	public class LPack 
	{
    /** Font settings */
    private static LFont _fonts = new LFont(); 

    /** Player points */
    public static LData GameTopPlayer(Score s) {
      var e = 0.8f * (float) s.Player / (float) (Config.WinScore - 1);
      var st = _fonts.Regular(5f, new Color(0.8f - e, 0.8f, 1f - e));
      st.alignment = TextAnchor.MiddleLeft;
      return new LData() {
        Msg = String.Format("Player: {0}", s.Player),
        Style = st
      };
    }

    /** AI points */
    public static LData GameTopAi(Score s) {
      var e = 0.8f * (float) s.Ai / (float) (Config.WinScore - 1);
      var st = _fonts.Regular(5f, new Color(0.8f, 0.8f - e, 1f - e));
      st.alignment = TextAnchor.MiddleRight;
      return new LData() {
        Msg = String.Format("Droid: {0}", s.Ai),
        Style = st
      };
    }

    /** Score for the top score section */
    public static LData GameTopScore(Score s) {
      var st = _fonts.Regular(5f, new Color(0.8f, 0.8f, 1.0f));
      st.alignment = TextAnchor.MiddleCenter;
      return new LData() {
        Msg = String.Format("Score: {0}", s.Points),
        Style = st
      };
    }

    /** Return the style for a notification in game */
    public static GUIStyle GameNoticeStyle(float factor) {
      return _fonts.Heavy(3f, new Color(0.8f, 1.0f, 1.0f, factor));
    }

    /** Touch here notice for game screen */
    public static LData GameTouchHere(float factor) {
      var st = _fonts.Regular(4f, new Color(1f, 1f, 1f, factor));
      st.alignment = TextAnchor.MiddleCenter;
      return new LData() {
        Msg = "Touch or click here to move the paddle!",
        Style = st
      };
    }

    /** Highscore win message */
    public static LData ScoreWinMessage(Score s) {
      var st = _fonts.Regular(6f, new Color(0.8f, 1.0f, 0.8f));
      st.alignment = TextAnchor.MiddleCenter;
      st.wordWrap = true;
      return new LData() {
        Msg = String.Format("You defeated Pong Droid and won with {0} points!", s.Points),
        Style = st
      };
    }

    /** Highscore lose message */
    public static LData ScoreLoseMessage(Score s) {
      var st = _fonts.Regular(6f, new Color(1f, 0.8f, 0.8f));
      st.alignment = TextAnchor.MiddleCenter;
      st.wordWrap = true;
      return new LData() {
        Msg = String.Format("You were defeated by Pong Droid and lost with {0} points", s.Points),
        Style = st
      };
    }

    /** Highscore header */
    public static LData ScoreHighscoreHeader() {
      var st = _fonts.Heavy(7f, new Color(0f, 0.9f, 0.0f));
      st.alignment = TextAnchor.MiddleCenter;
      return new LData() {
        Msg = String.Format("HIGH SCORES"),
        Style = st
      };
    }

    /** Returns the style and content for a single high score */
    public static LData ScoreHighscoreItem(HighScore s, int i, int total) {
      var factor = 1f - (float)i / (float)total;
      var size = 5f;
      if (nLayout.Distance(100) > Screen.width) {
        size = 3f;
      }
      var st = _fonts.Fixed(size, new Color(0.5f, factor, factor + 0.5f));
      st.alignment = TextAnchor.MiddleCenter;
      return new LData() {
        Msg = String.Format("{1} {0,10} POINTS", s.Points, s.Date),
        Style = st
      };
    }

    /** Return content and style for 'play again' button */
    public static LData ScoreButton() {
      var style = _fonts.Heavy(7f, new Color(0.8f, 0.8f, 1.0f));
      var skin = (GUISkin) Resources.Load("UISkin");
      var st = skin.button;
      st.fontSize = style.fontSize;
      st.normal.textColor = style.normal.textColor;
      return new LData() {
        Msg = "Play again?",
        Style = st
      };
    }

    /** Return text for the credits */
    public static LData Credits(float t) {
      var factor = (t % 4.0f);
      if (factor > 2.0f) 
        factor = 2.0f - (factor - 2.0f);
      factor = 0.3f * factor / 2f;
      var style = _fonts.Regular(3f, new Color(0.7f, 0.7f, 0.7f + factor));
      style.alignment = TextAnchor.LowerLeft;
      style.wordWrap = true;
      return new LData() {
        Msg = "by twitter.com/shadowmint\n" + 
              "http://github.com/shadowmint/pongstar/",
        Style = style,
      };
    }
  }

  /** Font bindings */
  internal class LFont {

    /** Fixed width font */
    public GUIStyle Fixed(float mm, Color c) {
      var font = (Font) Resources.Load("fonts/Droid-Mono");
      var rtn = new GUIStyle();
      rtn.font = font;
      rtn.fontSize = FontSize(mm);
      rtn.fontStyle =  FontStyle.Bold;
      rtn.normal.textColor = c;
      return rtn;
    }

    /** Regular font */
    public GUIStyle Regular(float mm, Color c) {
      var font = (Font) Resources.Load("fonts/Exo-Regular");
      var rtn = new GUIStyle();
      rtn.font = font;
      rtn.fontSize = FontSize(mm);
      rtn.normal.textColor = c;
      return rtn;
    }

    /** Heavy font */
    public GUIStyle Heavy(float mm, Color c) {
      var font = (Font) Resources.Load("fonts/Exo-Bold");
      var rtn = new GUIStyle();
      rtn.font = font;
      rtn.fontSize = FontSize(mm);
      rtn.normal.textColor = c;
      return rtn;
    }

    public int FontSize(float mm) {
      if (Metrics.Android) {
        if (Metrics.Height < 100) {
          return nLayout.FontSize(mm * 3.0f / 5.0f);
        }
      }
      return nLayout.FontSize(mm);
    }
  }
}
