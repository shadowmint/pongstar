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
  public class Ball : DrawableModel 
  {
    public Ball(GameState parent) : base(parent) {}

    private float[] _size = new float[2] { 5f, 5f };

    /** Where the ball currently is */
    public float[] Position = new float[2];
    
    /** Velocity of the ball */
    public float[] Velocity = new float[2] { 28.0f, 28.0f };

    /** Size of the ball */
    public float Width { get { return _size [0]; } }
    public float Height { get { return _size [1]; } }

    /** Temporary marker for ball location */
    private nQuad _ball = null;
    
    /** Update the ball state */
    public void Update(float seconds, EventHandler events) {
    
      /* Move */
      Position [0] += Velocity [0] * seconds; 
      Position [1] += Velocity [1] * seconds;
      
      /* Check for bouncing */
      if (Position [0] < _parent.Field.Bounds [0]) {
        Position [0] = _parent.Field.Bounds [0];
        Velocity [0] = -Velocity [0];
        var e = new WallHit(_parent, WallHitTarget.WALL_LEFT, Velocity, Position);
        events.Trigger(e);
      }
      if (Position [0] > _parent.Field.Bounds [2]) {
        Position [0] = _parent.Field.Bounds [2];
        Velocity [0] = -Velocity [0];
        var e = new WallHit(_parent, WallHitTarget.WALL_RIGHT, Velocity, Position);
        events.Trigger(e);
      }
      if (Position [1] < _parent.Field.Bounds [1]) {
        Position [1] = _parent.Field.Bounds [1];
        Velocity [1] = -Velocity [1];
        var e = new WallHit(_parent, WallHitTarget.WALL_BOTTOM, Velocity, Position);
        events.Trigger(e);
      }
      if (Position [1] > _parent.Field.Bounds [3]) {
        Position [1] = _parent.Field.Bounds [3];
        Velocity [1] = -Velocity [1];
        var e = new WallHit(_parent, WallHitTarget.WALL_TOP, Velocity, Position);
        events.Trigger(e);
      }

      if (_parent.AiPaddle.Intersects(this)) {
        var e = new PaddleHit(_parent, _parent.AiPaddle, Velocity, Position);
        events.Trigger(e);
        HandlePaddleHit(_parent.AiPaddle);
      }
      if (_parent.PlayerPaddle.Intersects(this)) {
        var e = new PaddleHit(_parent, _parent.PlayerPaddle, Velocity, Position);
        events.Trigger(e);
        HandlePaddleHit(_parent.PlayerPaddle);
      }

      if (_ball != null) {
        _ball.Data.Position.Set(Position);
        _ball.Data.Rotation += 0.5f;
      }
    }

    private void HandlePaddleHit(Paddle p) {
      var magic_number = 0.6f;
      var xdelta = (p.Position[0] - Position[0]) / (p.Size[0]);
      var factor = xdelta / magic_number;
      if (factor > 1f)
        factor = 1f;
      else if (factor < -1f)
        factor = -1f;

      Velocity[1] = -Velocity [1];
      Velocity[0] = Velocity[0] - 20f * factor;

      Velocity[0] += 0.6f * Math.Sign(Velocity[0]);
      Velocity[1] += 0.6f * Math.Sign(Velocity[1]);
    }
    
    public override nIDrawable Display { 
      get { 
        if (_ball == null) {
          var x = _size[0] / 2;
          var y = _size[1] / 2;
          _ball = new nQuad();
          _ball.Data.Texture = (Texture) Resources.Load("Ball");
          _ball.Data.Points.Set(new float[8] { x, y, x, -y, -x, -y, -x, y });
          _ball.Data.UV.Set(new float[8] { 1, 1, 1, 0, 0, 0, 0, 1 });
          _ball.Data.Depth = 4;
        }
        return _ball;
      } 
    }
  }
	
}
