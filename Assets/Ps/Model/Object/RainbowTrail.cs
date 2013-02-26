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
  public class RainbowTrail : DrawableModel
  {
    public RainbowTrail(GameState parent) : base(parent) {
      _sprites.Add(_trail);
      _sprites.Add(_sparkles);
      _sparkleManager = new RainbowSparkles() {
        LastSparkle = 0f,
        SparkleRate = 0.03f,
        Sparkles = _sparkles,
      };
      _trailManager = new RainbowTrailManager(_trail) {
        SegmentSize = 0.3f,
        MaxLength = 26f,
        MinWidth = 2f,
        MaxWidth = 6f
      };
    }

    public nSpriteGroup _sprites = new nSpriteGroup();

    public nMotionGroup _sparkles = new nMotionGroup();

    public nMotionGroup _trail = new nMotionGroup();

    /** Handles the sparkles */
    private RainbowSparkles _sparkleManager;

    /** Handles the trail */
    private RainbowTrailManager _trailManager;

    public override nIDrawable Display { 
      get { 
        return _sprites;
      } 
    }

    /** Update trail */
    public void Update(float seconds) {
      _trailManager.Update(seconds, _parent.Ball);
      _sparkleManager.Update(seconds, _parent);
      _sprites.Update(seconds);
    }
  }
}
