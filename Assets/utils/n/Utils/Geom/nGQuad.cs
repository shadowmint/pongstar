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

namespace n.Utils.Geom
{
  /** Helper for basic quad geometry */
  public class nGQuad {

    public float[] Points { get; set; }

    public nGQuad() {
      Init(-1, -1, 1, 1);
    }

    public nGQuad(float size) {
      Init(-size/2f, -size/2f, size/2f, size/2f);
    }

    public nGQuad(float width, float height) {
      Init(-width/2f, -height/2f, width/2f, height/2f);
    }

    public nGQuad (nGQuad parent)
    {
      Init(parent.xMin, parent.yMin, parent.xMax, parent.yMax);
    }

    public nGQuad (Rect parent)
    {
      Init(parent.xMin, parent.yMin, parent.xMax, parent.yMax);
    }

    public nGQuad(float xMin, float yMin, float xMax, float yMax) {
      Init(xMin, yMin, xMax, yMax);
    }

    /** 
     * Apply an offset to the position and return the quad. 
     * <p>
     * So you can go: var points = new nQuad(10f).Offset(blah.Position).Points;
     */
    public nGQuad Offset(float[] Position) {
      return Offset(Position[0], Position[1]);
    }

    /** 
     * Apply an offset to the position and return the quad. 
     * <p>
     * So you can go: var points = new nQuad(10f).Offset(5f, 5f).Points;
     */
    public nGQuad Offset(float xOffset, float yOffset) {
      for (var i = 0; i < Points.Length; ++i) {
        if ((i % 2) == 0)
          Points[i] += xOffset;
        else
          Points[i] += yOffset;
      }
      return this;
    }

    /** This quad as a rect */
    public Rect Rect{ 
      get {
        var rtn = new Rect(xMin + (xMax - xMin) / 2f, yMin + (yMax - yMin) / 2f, xMax - xMin, yMax - yMin);
        return rtn;
      }
    }

    public float xMin {
      get {
        return Points[4];
      }
    }

    public float xMax {
      get {
        return Points[0];
      }
    }

    public float yMin {
      get {
        return Points[5];
      }
    }

    public float yMax {
      get {
        return Points[1];
      }
    }

    public float Height {
      get {
        return yMax - yMin;
      }
    }

    public float Width {
      get {
        return xMax - xMin;
      }
    }

    /** Check if two sprites intersect */
    public static bool Intersects(nSprite a, nSprite b) {
      var q1 = new nGQuad();
      q1.Points = a.Points.Raw;
      q1.Offset(a.Position[0], a.Position[1]);
      
      var q2 = new nGQuad();
      q2.Points = b.Points.Raw;
      q2.Offset(b.Position[0], b.Position[1]);
      
      return q1.Intersects(q2);
    }

    /** If another quad intersects, non-rotated */
    public bool Intersects(nGQuad q) {
      var rectA = Rect;
      var rectB = q.Rect;
    
      // Logging for debug
      // nLog.Debug("{0},{1} -> {2},{3} vs. {4},{5} -> {6},{7}", xMin, yMin, xMax, yMax, q.xMin, q.yMin, q.xMax, q.yMax);

      // For rotated test, see:
      // http://stackoverflow.com/questions/115426/algorithm-to-detect-intersection-of-two-rectangles
      var rtn = ((Math.Abs(rectA.x - rectB.x) < (Math.Abs(rectA.width + rectB.width) / 2)) &&
                 (Math.Abs(rectA.y - rectB.y) < (Math.Abs(rectA.height + rectB.height) / 2)));

      return rtn;
    }

    private void Init(float xMin, float yMin, float xMax, float yMax) {
      Points = new float[8] { xMax, yMax, xMax, yMin, xMin, yMin, xMin, yMax };
    }
  }
}
