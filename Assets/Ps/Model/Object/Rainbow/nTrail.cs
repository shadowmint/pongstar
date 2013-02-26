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
  public class nTrail : nIDrawable
  {
    public float MinWidth { get; set; }

    public float MaxWidth { get; set; }

    private int _segmentCount;

    private nSprite[] _sprites;

    public bool Invalid { get { return false; } }

    public nTrail(int segments) {
      _segmentCount = segments;
    }

    /** Create the segment buffer */
    private void CreateSegments(nGraphicsPipe pipe) {
      _sprites = new nSprite[_segmentCount];
      for (int i = 0; i < _segmentCount; ++i) {
        var s = pipe.Sprite();
        s.Depth = 28f;
        s.Texture = (Texture) Resources.Load("rainbow");
        s.Color.Set(new float[4] { 1f, 1f, 1f, 1f });
        s.UV.Set(new float[8] { 1, 1, 1, 0, 0, 0, 0, 1 });
        _sprites[i] = s;
      }
    }

    public nSprite[] Render(nGraphicsPipe pipe) {
      if (_sprites == null) 
        CreateSegments(pipe);
      return _sprites;
    }

    /** Rebuild this trail from the set of points */
    public void Rebuild(nGLine[] points) {
      if (_sprites != null) {
        var count = _sprites.Length < points.Length ? _sprites.Length : points.Length;
        if (count > 0) {
          for (var i = 0; i < count; ++i) {
            var offset1 = 1.0f - ((float)i) / points.Length;
            var offset2 = 1.0f - ((float)(i + 1)) / points.Length;
            RebuildSegment(_sprites[i], points[i], offset1, offset2);
          }
        }
        for (var i = count; i < _sprites.Length; ++i) {
          _sprites [i].Color [3] = 0f;
        }
      }
    }

    /** Create a line segment */
    private void RebuildSegment(nSprite s, nGLine l, float offset1, float offset2) {
      var w1 = MinWidth + (MaxWidth - MinWidth) * offset1;
      var w2 = MinWidth + (MaxWidth - MinWidth) * offset2;
      var pnts = l.SPoints(w1, w2);
      s.Points.Set(pnts);
      s.Color[3] = (1.0f - offset1) * 0.2f;
    }
  }
}
