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
using n.Utils.Geom;

namespace n.Utils
{
  /** Layout bounds */
  public enum nEdge {
    LEFT,
    TOP,
    RIGHT,
    BOTTOM
  }

  /** Which axis to generate layout from */
  public enum nAxis {
    X,
    Y
  }

  /** For working with screen resolution layouts */
  public class nLayout 
  {
    /** Return distance in pixel units from mm */
    public static float Distance(float mm) {
      var dpi = Screen.dpi == 0 ? 96.0f : Screen.dpi;
      var ppmm = dpi / 25.4;
      var distance = ppmm * mm;
      return (float) distance;
    }

    /** Fontsize == distance but as an int */
    public static int FontSize(float mm) {
      return (int) Distance(mm);
    }

    /** Pixel coordinates of the center vertically */
    public static float Center(nAxis axis) {
      if (nAxis.X == axis) 
        return (float)Screen.width / 2f;
      else if (nAxis.Y == axis) 
        return (float)Screen.height / 2f;
      return 0f;
    }

    /** 
     * Return a rect of an object in screen pixels, centered at the given top/left offset with the given size.
     * <p> 
     * eg. To get a centered 100x30mm block you'd use:
     * nLayout.Block(nLayout.Center(nAxis.X), nLayout.Center(nAxis.Y), 100, 30);
     */
    public static Rect Block(float left, float top, float mmWide, float mmHigh) {
      var width = Distance(mmWide);
      var height = Distance(mmHigh);
      return new Rect(
        left - width / 2,
        top - height / 2,
        width,
        height
      );
    }

    /** Returns a block centered at left, top; no values are converted from mm */
    public static Rect EBlock(float left, float top, float wide, float high) {
      return new Rect(
        left - wide / 2,
        top - high / 2,
        wide,
        high
        );
    }

    /** Returns the full width of the screen in pixel values - 2 x padding */
    public static float PaddedFullWidth(float mmPadding) {
      var rtn = (float) Screen.width - Distance(mmPadding) * 2;
      return rtn;
    }

    /** Return coordinate of distance from boundary in float units from mm */
    public static float Distance(float mm, nEdge boundary) {
      if ((boundary == nEdge.TOP) || (boundary == nEdge.LEFT)) 
        return Distance(mm);
      else if (boundary == nEdge.RIGHT)
        return (float) Screen.width - Distance(mm);
      else if (boundary == nEdge.BOTTOM)
        return (float) Screen.height - Distance(mm);
      return 0f;
    }

    /** Special mapping function for screen -> world */
    public static float DistanceInWorld(float mm, nAxis axis, nCamera camera) {
      var height = camera.Native.orthographicSize * 2;
      var width = camera.Native.GetScreenWidth() / camera.Native.GetScreenHeight() * height;
      var p = 0f;
      if (axis == nAxis.X) {
        p = Distance(mm) / camera.Native.GetScreenWidth() * width;
      } 
      else if (axis == nAxis.Y) {
        p = Distance(mm) / camera.Native.GetScreenHeight() * height;
      }
      return p;
    }

    /** Special mapping function for screen -> world */
    public static float DistanceInWorld(float mm, nEdge boundary, nCamera camera) {
      var height = camera.Native.orthographicSize * 2;
      var width = camera.Native.GetScreenWidth() / camera.Native.GetScreenHeight() * height;
      var p = 0f;
      if (boundary == nEdge.BOTTOM)
        p = - height / 2 + DistanceInWorld(mm, nAxis.Y, camera);
      else if (boundary == nEdge.LEFT)
        p = - width / 2 + DistanceInWorld(mm, nAxis.X, camera);
      else if (boundary == nEdge.RIGHT)
        p = width / 2 - DistanceInWorld(mm, nAxis.X, camera);
      else if (boundary == nEdge.TOP)
        p = height / 2 - DistanceInWorld(mm, nAxis.Y, camera);
      return p;
    }
  }
}
