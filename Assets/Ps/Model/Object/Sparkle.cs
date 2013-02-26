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
using System.Linq;
using n.Core;
using System.Collections.Generic;
using n.Gfx;
using System;
using n.Gfx.Anim;

namespace Ps.Model.Object
{
  public enum SparkleType {
    SPARKLE_PLAYER,
    SPARKLE_AI,
    SPARKLE_WALL
  }

  public class Sparkle : DrawableModel
  {
    public Sparkle(GameState parent) : base(parent) {
      _sparkles = new nMotionGroup();
      _sprites = new nSpriteGroup();
      _sprites.Add(_sparkles);
    }

    public override nIDrawable Display { 
      get { 
        return _sprites; 
      } 
    }

    public bool Invalid { get { return false; } }

    /** Display manager */
    private nSpriteGroup _sprites;

    /** Set of sparkles */
    private nMotionGroup _sparkles;

    /** Create a new sparkle group */
    public void CreateSparkles(float[] Position, float[] Velocity, SparkleType type) {
      var factory = new SparkFactory() {
        Source = new float[2] { Position[0], Position[1] },
        Velocity = new float[3] { Velocity[0], Math.Sign(Velocity[1]) * 10f, 0f },
        Lifespan = 1.0f,
        LifespanVar = 0.5f,
        Count = 10,
        CountVar = 2,
        Tint = new float[4] { 1.0f, 1.0f, 1.0f, 1.0f }
      };

      /* Custom per type */
      factory.VelocityVar [2] = nRand.Float(0f, 50f);
      if (type == SparkleType.SPARKLE_WALL) {
        factory.Velocity [1] = 0;
        factory.Velocity [0] = Math.Sign(Velocity [0]) * 10f;
        factory.VelocityVar [0] = 20f;
        factory.VelocityVar [1] = 30f;
        factory.Tint = new float[4] { 0.5f, 0.5f, 1.0f, 1.0f };
      } else {
        factory.Velocity [0] = Velocity [0]; 
        factory.Velocity [1] = Math.Sign(Velocity [1]) * 10f;
        factory.VelocityVar [0] = 20f;
        factory.VelocityVar [1] = 30f;
        factory.Tint = new float[4] { 1.0f, 1.0f, 0.5f, 1.0f };
      } 

      var items = factory.Manufacture();
      foreach (var i in items) {
        var m = new nMotion(i, new SparkAnim());
        _sparkles.Add(m);
      }
    }

    /** Update the sparkles */
    public void Update(float seconds) {
      _sprites.Update(seconds);
    }
  }
}
