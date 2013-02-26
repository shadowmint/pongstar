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

namespace Ps.Model.Object
{
  public class Spark : nIDrawable
  {
    public float[] Position { get; set; }
    public float[] Velocity { get; set; }
    public float Rotation { get; set; }
    public float[] Tint { get; set; }
    public float Lifespan { get; set; }
    public float Lived { get; set; }
    public float[] Scale { get; set; }
    public float[] ScaleChange { get; set; }
    private nSprite[] _sprites = null;

    public Spark() {
      Position = new float[2];
      Velocity = new float[3];
      Tint = new float[4];
      Scale = new float[2];
      ScaleChange = new float[2];
    }

    public bool Alive {
      get {
        return Lived < Lifespan;
      }
    }

    public bool Invalid {
      get {
        return !Alive;
      }
    }

    public nSprite[] Render(nGraphicsPipe pipe) {
      if (_sprites == null) {
        _sprites = new nSprite[1] {
          pipe.Sprite()  
        };
      }
      return _sprites;
    }
  }
}
