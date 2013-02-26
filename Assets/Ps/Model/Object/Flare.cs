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
using Ps.Model.Events;

namespace Ps.Model.Object
{
  /** Screen flare we get when we have a point to either side */
  public class Flare : DrawableModel 
  {
    public Flare(GameState parent) : base(parent) {}

    /** The flare display item */
    private nQuad _display = null;

    /** Is this flare active? */
    private bool _active = false;

    /** Lifespan */
    private float _span = 3f;

    /** Life so far */
    private float _life = 0f;

    /** Activate this flare */
    public void Show() {
      _life = 0f;
      _active = true;
    }
    
    /** Update the ball state */
    public void Update(float seconds) {
      if (_active) {
        _life += seconds;
        var factor = _life / _span;
        if (factor >= 1.0f) {
          factor = 1.0f;
          _active = false;
        }
        if (_display != null)
          _display.Data.Color [3] = 1.0f - factor;
      }
    }

    public override nIDrawable Display { 
      get { 
        if (_display == null) {
          var size = _parent.Camera().ScreenBounds;
          var x = size.width / 2f;
          var y = size.height / 2f;
          _display = new nQuad();
          _display.Data.Points.Set(new float[8] { x, y, x, -y, -x, -y, -x, y });
          _display.Data.Color.Set(new float[4] { 0.01f, 0f, 0.03f, 0f });
          _display.Data.Depth = 0;
        }
        return _display;
      } 
    }
  }
	
}
