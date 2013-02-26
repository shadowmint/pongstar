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

namespace Ps.Model.Object
{
  public class Field : DrawableModel 
  {
    public Field(GameState parent, nCamera camera) : base(parent) {
      _cam = camera;
      
      var bounds = parent == null ? camera.ScreenBounds : parent.GameBounds();
      Bounds = new float[4] { bounds.xMin, bounds.yMin, bounds.xMax, bounds.yMax };
    }
    
    /** Camera reference */
    private nCamera _cam;
    
    /** Boundaries for the field in xMin, yMin, xMax, yMax order */
    public float[] Bounds;
    
    /** The background image for the field */
    public nBackground _bg = null;
    
    public override nIDrawable Display { 
      get { 
        if (_bg == null) {
          _bg = new nBackground(_cam);
          _bg.Texture = (Texture) Resources.Load("bg");
          _bg.Depth = 128;
          _bg.UV(new float[8] { 0, 1, 1, 1, 1, 0, 0, 0 });

          var ratio = (float) _bg.Texture.width / (float) _bg.Texture.height;
          _bg.Size[1] = _cam.ScreenBounds.height;
          _bg.Size[0] = _bg.Size[1] * ratio;
        }
        return _bg;
      } 
    }

    /** Update the way the background looks */
    public void Update(float seconds) {
      var offset = seconds * 1.0f;
      if (_bg != null) 
        _bg.Offset[0] += offset;
    }
  }
}
