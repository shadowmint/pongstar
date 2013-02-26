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
  /** Helper for basic line geometry */
  public class nGLine {

    public float[] P1 { get; set; }

    public float[] P2 { get; set; }

    public float[] Mid { 
      get {
        var u = Unit;
        var d = Length / 2.0f;
        return new float[2] {
          P1[0] + u[0] * d,
          P1[1] + u[1] * d,
        };
      }
    }

    /** Length */
    public float Length {
      get {
        var x = P1[0] - P2[0];
        var y = P1[1] - P2[1];
        return (float) Math.Sqrt(x * x + y * y);
      }
    }

    /** Unit vector from x1, y1 to x2, y2 set to x3, y3 */
    public float[] Unit {
      get {
        var x = P2 [0] - P1 [0];
        var y = P2 [1] - P1 [1];
        var factor = 1.0f / (float)Math.Sqrt(x * x + y * y);
        return new float[2] { x * factor, y * factor };
      }
    }

    /** Perpendicular unit vector */
    public float[] Perp {
      get {
        var unit = Unit;
        return new float[2] {
          -unit[1],
          unit[0]
        };
      }
    }
    
    /** 
     * Not really perpendicular vector for special bounce animations.
     * The zero y magnitude makes reflections looks cool.
     */
    public float[] SPerp {
      get {
        var x = (P2 [0] - P1 [0]) * 0f; // 0 for best look
        var y = P2 [1] - P1 [1];
        var factor = 1.0f / (float)Math.Sqrt(x * x + y * y);
        return new float[2] { -y * factor, x * factor };
      }
    }

    /** Generate points for this line segment as a quad with the given widths */
    public float[] Points(float width1, float width2) {
      width1 = width1 / 2.0f;
      width2 = width2 / 2.0f;
      var p = Perp;
      var rtn = new float[8] {
        P1[0] + p[0] * width1, P1[1] + p[1] * width1, 
        P1[0] - p[0] * width1, P1[1] - p[1] * width1, 
        P2[0] - p[0] * width2, P2[1] - p[1] * width2, 
        P2[0] + p[0] * width2, P2[1] + p[1] * width2, 
      };
      return rtn;
    }

    /** Generate points for this line segment using a special perp vector for smooth bouce animations */
    public float[] SPoints(float width1, float width2) {
      width1 = width1 / 2.0f;
      width2 = width2 / 2.0f;
      var p = SPerp;
      var rtn = new float[8] {
        P1[0] + p[0] * width1, P1[1] + p[1] * width1, 
        P1[0] - p[0] * width1, P1[1] - p[1] * width1, 
        P2[0] - p[0] * width2, P2[1] - p[1] * width2, 
        P2[0] + p[0] * width2, P2[1] + p[1] * width2, 
      };
      return rtn;
    }
  }
}
