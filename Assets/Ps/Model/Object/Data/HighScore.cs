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

namespace Ps.Model
{
	public class HighScore 
	{
    public int Points { get; set; }
    public string Date { get; set; }
	}

  public class HSKeys {
    public static string HS_RECORD_COUNT = "HS_RECORD_COUNT";
    public static string HS_POINTS_BASE = "HS_POINTS_";
    public static string HS_DATE_BASE = "HS_DATE_";
    public static int HS_MAX_SCORES = 6;
  }

  public class HighScores : nModel {

    public Dictionary<int,HighScore> Scores;

    /** Load any high scores we currently have */
    public void Load() {
      InitScore();
      Scores = new Dictionary<int, HighScore>();
      var count = PlayerPrefs.GetInt(HSKeys.HS_RECORD_COUNT);
      for (var i = 0; i < count; ++i) {
        var pnts = PlayerPrefs.GetInt(HSKeys.HS_POINTS_BASE + i);
        var date = PlayerPrefs.GetString(HSKeys.HS_DATE_BASE + i);
        var item = new HighScore() {
          Points = pnts,
          Date = date
        };
        Scores[i] = item;
      }
    } 

    /** Update score, and save */
    public void Update(int points) {
      Load();
      var queue = new Queue<HighScore>();
      var count = Scores.Keys.Count;
      var added = false;
      for (var i = 0; i < count; ++i) {
        if (!added && (Scores [i].Points < points)) {
          var item = new HighScore {
            Points = points,
            Date = DateTime.Now.ToShortDateString()
          };
          queue.Enqueue(item);
          added = true;
        }
        queue.Enqueue(Scores [i]);
      }
      if (!added && (queue.Count < HSKeys.HS_MAX_SCORES)) {
        var item = new HighScore {
          Points = points,
          Date = DateTime.Now.ToShortDateString()
        };
        queue.Enqueue(item);
      }

      Scores = new Dictionary<int, HighScore>();
      var counter = 0;
      while (counter < HSKeys.HS_MAX_SCORES) {
        if (queue.Count > 0) {
          var item = queue.Dequeue();
          Scores[counter] = item;
          ++counter;
        }
        else 
          break;
      }

      Save();
    }

    /** Save current scores */
    public void Save() {
      PlayerPrefs.SetInt(HSKeys.HS_RECORD_COUNT, Scores.Keys.Count);
      for (var i = 0; i < Scores.Keys.Count; ++i) {
        PlayerPrefs.SetInt(HSKeys.HS_POINTS_BASE + i, Scores[i].Points);
        PlayerPrefs.SetString(HSKeys.HS_DATE_BASE + i, Scores[i].Date);
      }
      PlayerPrefs.Save();
      // nLog.Debug("Saved {0} records", Scores.Keys.Count);
    }

    /** Init prefs, if we've never done this before */
    private void InitScore() {
      if (!PlayerPrefs.HasKey(HSKeys.HS_RECORD_COUNT)) {
        PlayerPrefs.SetInt(HSKeys.HS_RECORD_COUNT, 0);
      }
    }
  }
}
