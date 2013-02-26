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

namespace Ps.Model.Ai
{
  /** Easy AI profile */
  public class HardAiProfile : IProfile
  {
    public float Speed { 
      get {
        return 1.2f;
      }
    }

    public void Update(Ball b, Paddle p) {
      float target = 0f;
      if (b.Velocity [1] > 0) 
        target = b.Position [0];

      float distance_to_target = Math.Abs(target - p.Position [0]);
      if (distance_to_target < p.Speed) {
        distance_to_target = distance_to_target / 10f;
        if (distance_to_target < 1f)
          distance_to_target = 0f;
      }

      if (target > p.Position[0])
        p.MoveRight(distance_to_target);
      if (target < p.Position[0])
        p.MoveLeft(distance_to_target);
    }
  }
}
