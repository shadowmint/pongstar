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

namespace Ps.Model.Events
{
  public class PaddleHit : IAction
  {
    public PaddleHit(GameState state, Paddle target, float[] velocity, float[] position) : base(state) {
      Target = target;
      Velocity[0] = velocity[0];
      Velocity[1] = velocity[1];
      Position[0] = position[0];
      Position[1] = position[1];

      if (target == state.AiPaddle) 
        Type = SparkleType.SPARKLE_AI;
      else if (target == state.PlayerPaddle)
        Type = SparkleType.SPARKLE_PLAYER;
      else
        Type = SparkleType.SPARKLE_WALL;
    }

    public override int Id {
      get {
        return ID.PaddleHit;
      }
    }

    /** The paddle we hit */
    public Paddle Target;

    /** Type of hit */
    public SparkleType Type;

    /** Coordinates of the hit */
    public float[] Position = new float[2];

    /** Ball direction */
    public float[] Velocity = new float[2];
  }
}
