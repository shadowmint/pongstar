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

  /** A trail object */

  /** Manages adding sparkles */
  public class RainbowSparkles
  {
    public nMotionGroup Sparkles { get; set; }

    /** How long ago did we last add a sparkle? */
    public float LastSparkle { get; set; }

    /** Add sparkles this often */
    public float SparkleRate { get; set; }

    /** Real sparkle production rate, based on speed */
    private int _realRate { get; set; }

    private void UpdateRate(GameState state) {
      var normative_speed = 45f;
      var speed = (float) Math.Sqrt(state.Ball.Velocity[0] * state.Ball.Velocity[0] + state.Ball.Velocity[1] * state.Ball.Velocity[1]);
      _realRate = (int) (speed / normative_speed);
      if (_realRate == 0) 
        _realRate = 1;
    }

    public void Update(float seconds, GameState state) {
      LastSparkle += seconds;
      UpdateRate(state);
      if (LastSparkle > SparkleRate) {
        var pos = new float[2] {
          state.Ball.Position [0],
          state.Ball.Position [1]
        };
        if (nRand.Bool(true, 2)) 
          CreateSparkles(pos, state.Ball.Velocity);
        LastSparkle = 0;
      }
    }
  
    /** Create a new sparkle group */
    public void CreateSparkles(float[] Position, float[] Velocity) {
      var factory = new SparkFactory() {
        Source = new float[2] { Position[0], Position[1] },
        Lifespan = 0.5f,
        LifespanVar = 0.1f,
        Count = _realRate,
        CountVar = _realRate / 2,
        Tint = new float[4] { 1.0f, 1.0f, 1.0f, 1.0f }
      };
      
      factory.VelocityVar [2] = nRand.Float(0f, 50f);
    
      factory.Velocity [0] = Velocity [0] * 0.5f;
      factory.Velocity [1] = Velocity [1] * 0.5f;
      factory.VelocityVar [0] = Math.Sign(factory.Velocity [0]) * 3f;
      factory.VelocityVar [1] = Math.Sign(factory.Velocity [1]) * 3f;
      factory.Tint = new float[4] { 1.0f, 1.0f, 0.0f, 1.0f };     
    
      var items = factory.Manufacture();
      foreach (var i in items) {
        var m = new nMotion(i, new RainbowSparkAnim());
        Sparkles.Add(m);
      }
    }
  }
}
