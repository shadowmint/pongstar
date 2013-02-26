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
using System.Linq;
using n.Gfx;
using System;
using n.Utils;
using Ps.Controllers;
using n.Utils.Geom;
using n.Gfx.Anim;

namespace Ps.Model.Object
{

  /** Manages adding sparkles */

  /** Fade after use */

  public class SparkAnim : nIAnim
  {
    public bool Alive { get; private set; }

    private bool _ready = false;

    protected virtual void Setup(nSprite display) {
      display.Texture = (Texture) Resources.Load("star");
      display.Points.Set(new float[8] { 3, 3, 3, 0, 0, 0, 0, 3 });
      display.UV.Set(new float[8] { 1, 1, 1, 0, 0, 0, 0, 1 });
      display.Depth = 3f;
    }

    public void Update(nIDrawable Parent, nSprite[] sprites, float seconds) {

      var s = (Spark) Parent;
      var display = sprites [0];
      if (!_ready) 
        Setup(display);
      
      /* position */
      s.Position [0] += s.Velocity [0] * seconds;
      s.Position [1] += s.Velocity [1] * seconds;
      s.Rotation += s.Velocity [2] * seconds;
      s.Scale[0] += s.ScaleChange[0] * seconds;
      s.Scale[1] += s.ScaleChange[1] * seconds;
      
      /* transparency */
      s.Lived += seconds;
      Alive = s.Alive;
      var visibility = 0f;
      if (Alive) 
        visibility = 1.0f * (s.Lifespan - s.Lived) / s.Lifespan;
      s.Tint[3] = visibility;
      
      /* push changes into the display */
      if (display != null) {
        display.Color.Set(s.Tint);
        display.Rotation = s.Rotation;
        display.Position.Set(s.Position);
        display.Scale.Set(s.Scale);
      }
    }
  }
}
