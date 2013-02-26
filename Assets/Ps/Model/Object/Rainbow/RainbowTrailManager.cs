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

  /** Manages the trail itself */
  public class RainbowTrailManager 
  {
    public float MinWidth { get; set; }
    
    public float MaxWidth { get; set; }

    public float SegmentSize { get; set; }

    public float MaxLength { get; set; }

    private nMotionVector _motion;

    private nTrail _trail; 

    private nMotionGroup _sprites;

    private bool _ready = false;

    public RainbowTrailManager(nMotionGroup sprites) {
      _sprites = sprites;
    }

    private void Init() {
      _motion = new nMotionVector() {
        MaxLength = MaxLength,
        SegmentSize = SegmentSize
      };
      _trail = new nTrail((int) (MaxLength / SegmentSize)) {
        MinWidth = MinWidth,
        MaxWidth = MaxWidth
      };
      var m = new nMotion(_trail, new ForeverAnim());
      _sprites.Add(m);
      _ready = true;
    }

    public void Update(float seconds, Ball ball) {
      if (!_ready)
        Init();
      _motion.Update(ball.Position[0], ball.Position[1]);
      _trail.Rebuild(_motion.Points());
    }
  }
}
