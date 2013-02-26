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
using Ps.Model.Ai;

namespace Ps.Model.Object
{
  public class Paddle : DrawableModel 
  {
    public Paddle(GameState parent) : base(parent) {}

    public float[] Position = new float[2];

    public float[] Size = new float[2] { 9.8f, 2f };

    private nAnim _bounce = null;

    /** Speed of the paddle */
    public float Speed { get; set; }

    /** Temporary marker for ball location */
    private nQuad _display = null;

    /** AI profile, if any for this paddle */
    public IProfile Ai { get; set; }

    /** bounds to observe */
    private Rect _bounds;

    public override nIDrawable Display { 
      get { 
        if (_display == null) {
          _display = new nQuad();
          var x = Size[0] / 2;
          var y = Size[1] / 2;
          _display = new nQuad();
          _display.Data.Texture = (Texture) Resources.Load("Paddle");
          _display.Data.Points.Set(new float[8] { x, y, x, -y, -x, -y, -x, y });
          _display.Data.UV.Set(new float[8] { 1, 1, 1, 0, 0, 0, 0, 1 });
          _display.Data.Depth = 5;
          _bounds = _parent.GameBounds();
        }
        return _display;
      } 
    }

    /** Move left */
    public void MoveLeft(float d) {
      d = d > Speed ? Speed : d;
      Position[0] -= d;
      if ((Position[0] - Size[0] / 2f) < _bounds.xMin) 
        Position[0] = _bounds.xMin + Size[0] / 2f;
    }

    /** Move right */
    public void MoveRight(float d) {
      d = d > Speed ? Speed : d;
      Position[0] += d;
      if ((Position[0] + Size[0] / 2f) > _bounds.xMax) 
        Position[0] = _bounds.xMax - Size[0] / 2f;
    }

    /** Update the ball state */
    public void Update(float seconds) {
      if (_bounce != null) {
        _bounce.Update(seconds);
        Position[1] = _bounce.Value;
        if (_bounce.Finished)
          _bounce = null;
      }

      if (Ai != null) 
        Ai.Update(_parent.Ball, this);
     
      if (_display != null) 
        _display.Data.Position.Set(Position);
    }

    /** If the ball intersects this paddle */
    public bool Intersects(Ball b) {
      var rectA = new Rect(b.Position [0], b.Position [1], b.Width, b.Height / 3);
      var rectB = new Rect(Position [0], Position [1], Size [0], Size [1]);

      // For rotated test, see:
      // http://stackoverflow.com/questions/115426/algorithm-to-detect-intersection-of-two-rectangles
      if ((Math.Abs(rectA.x - rectB.x) < (Math.Abs(rectA.width + rectB.width) / 2)) && 
        (Math.Abs(rectA.y - rectB.y) < (Math.Abs(rectA.height + rectB.height) / 2))) {

        // Also add some bounce, because its shiny
        if (_bounce == null) {
          _bounce = new nAnim(_display.Data) {
            Duration = 0.3f,
            Magnitude = Math.Sign(_parent.Ball.Velocity[1]) * 1.5f
          };
        }

        return true;
      }
      return false;
    }
  }

  public class nAnim 
  {
    public nAnim(nSprite target) {
      Target = target;
      _originalValue = Base();
    }

    public nSprite Target { get; set; }

    public bool Finished { 
      get {
        return _secondsPassed >= Duration;
      }
    }

    public float Duration { get; set; }

    public float Magnitude { get; set; }

    /** Current value */
    public float Value { get; set; }

    private float _secondsPassed = 0;

    private float _originalValue = 0;

    public void Update(float seconds) {
      _secondsPassed += seconds;
      Value = _originalValue + Offset(_secondsPassed) * Magnitude;
    }

    public float Offset(float seconds) {
      var split = Duration / 4.0f;
      float result;
      if (seconds < split) {
        var a = 0;
        var b = 5.0f * Magnitude / Duration;
        result = a + b * seconds;
      }
      else {
        var a = 5.0f * Magnitude / 4.0f;
        var b = 5.0f * Magnitude / (-4.0f * Duration);
        result = a + b * seconds;
      }
      return result * Math.Sign(_originalValue);
    }

    public float Base() {
      return Target.Position[1];
    }
  }
}
