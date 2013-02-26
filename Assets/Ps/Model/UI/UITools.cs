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

namespace Ps.Model
{
	public class UITools 
	{
    /** Repeat a single texture of the given hight over the full width of the window */
    public static void RepeatStrip(Texture t, float height, float left, float top) {
      var h = nLayout.Distance(height);
      var w = t.width / t.height * h;
      var n = (int) Math.Ceiling((float) Screen.width / w);
      for (var i = 0; i < n; i++) {
        var r = new Rect(left + i * w, top, w, h);
        GUI.DrawTexture(r, t, ScaleMode.ScaleToFit, true);
      }
    }
  }
}
