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
  public class EasyAiProfile : IProfile
  {
    public float Speed { 
      get {
        return 0.6f;
      }
    }

    public void Update(Ball b, Paddle p) {
      var target = b.Position [0];

      float distance_to_target = Math.Abs(target - p.Position [0]);
      if (distance_to_target < 1f)
        return;

      /* Don't jerk */
      if ((distance_to_target > 2f) && (distance_to_target < 2.5f)) {
        distance_to_target = 0.3f;
      }

      if (target > p.Position[0])
        p.MoveRight(distance_to_target);

      if (target < p.Position[0])
        p.MoveLeft(distance_to_target);
    }
  }
}
