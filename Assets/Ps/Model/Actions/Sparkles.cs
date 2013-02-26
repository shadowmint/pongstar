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
  public class Sparkles
  {
    public static void AddSparkles(IEventData raw) {
      var data = (PaddleHit) raw;
      var s = data.State;
      s.Sparkle.CreateSparkles(data.Position, s.Ball.Velocity, data.Type);
    }

    public static void AddSparklesToWall(IEventData raw) {
      var data = (WallHit)raw;
      if ((data.Target == WallHitTarget.WALL_LEFT) || (data.Target == WallHitTarget.WALL_RIGHT)) {
        var s = data.State;
        s.Sparkle.CreateSparkles(data.Position, s.Ball.Velocity, SparkleType.SPARKLE_WALL);
      }
    }
  }
}
