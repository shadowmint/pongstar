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
  public class nMotionVector 
  {
    /** The length of the line segments to keep in this vector */
    public float SegmentSize { get; set; }

    /** The max length from the start of the vector to track */
    public float MaxLength { get; set; }

    /** The current length of the vector */
    public float Length { get; set; }

    /** A set of all the points we hold */
    private Queue<nGLine> _segments = new Queue<nGLine>();

    /** If the last point is part of a segment */
    private bool _lastSeg = false;

    /** The second most recent point we turned into a segment */
    private float[] _plast = null;

    /** The most recent point we added */
    private float[] _last = null;

    public nMotionVector() {
      SegmentSize = 1.0f;
      MaxLength = 10f;
      Length = 0f;
    }

    /** Add a new coordinate for the object this vector is tracking */
    public void Update(float x, float y) {
      _lastSeg = false;
      if (_last != null) {
        var dist = Distance(_plast[0], _plast[1], x, y);
        if (dist > SegmentSize) {
          //nLog.Debug("The distance between the last point and current point is: " + dist);
          var count = (int)Math.Floor(dist / SegmentSize);
          AddNewSegments(count, x, y);
        }
      }
      _last = new float[2] { x, y };

      /* Save a marker for the first segment */
      if (_plast == null) {
        _plast = _last;
      }

      /* Update size */
      Length = TotalLength();
      if (Length > MaxLength) {
        Crop();
        Length = TotalLength();
      }
      //nLog.Debug("Length was: " + Length);
    }

    /** Unit vector from x1, y1 to x2, y2 set to x3, y3 */
    private void Unit(float x1, float y1, float x2, float y2, out float x3, out float y3) {
      x3 = x2 - x1;
      y3 = y2 - y1;
      var factor = 1.0f / (float) Math.Sqrt(x3 * x3 + y3 * y3);
      x3 = x3 * factor;
      y3 = y3 * factor;
      //nLog.Debug("Unit {0},{1} -> {2},{3} = {4},{5}", x1, y1, x2, y2, x3, y3);
    }

    /** Check if the new point on the current line segment */
    /*private bool Inline(float x, float y) {
      if ((_start == null) || (_start.Next == null))
        return false;
      float ux, uy = 0f;
      Unit(_start.Next.X, _start.Next.Y, _start.X, _start.Y, out ux, out uy);
      var k1 = x / ux;
      var k2 = y / uy;
      var tolerance = 0.001f;
      if ((k1 >= (k2 - tolerance)) && (k1 <= k2 + tolerance))
        return true;
      return false;
    }*/

    /** Add some new segments */
    private void AddNewSegments(int count, float x, float y) {
      //nLog.Debug("Request to add {0} new segments", count);
      float ux, uy;
      Unit(_plast[0], _plast [1], x, y, out ux, out uy);
      var prev = new float[2] { _plast[0], _plast[1] };
      for (var i = 0; i < count; ++i) {
        var segment = new nGLine() {
          P1 = new float[2] { 
            prev[0],
            prev[1],
          },
          P2 = new float[2] { 
            _plast[0] + (i + 1) * ux * SegmentSize,
            _plast[1] + (i + 1) * uy * SegmentSize
          }
        };
        _segments.Enqueue(segment);
        prev[0] = segment.P2[0];
        prev[1] = segment.P2[1];

        /* Save a marker for next time */
        if (i == (count - 1)) {
          _plast[0] = segment.P2[0];
          _plast[1] = segment.P2[1];
          //nLog.Debug("The new plast is updated to {0},{1}", _plast[0], _plast[1]);
          if ((segment.P2[0] == x) && (segment.P2[1] == y)) {
            _lastSeg = true;
          }
        }
      }
      //nLog.Debug("Added segment, new count is " + _segments.Count);
    }

    /** Calculate the total length */
    private float TotalLength() {
      var total = 0f;
      foreach (var s in _segments) {
        total += Distance(s.P1[0], s.P1[1], s.P2[0], s.P2[1]);
      }
      return total;
    }

    /** Distance between two points */
    private float Distance(float x1, float y1, float x2, float y2) {
      var total = (x2 - x1) * (x2 - x1);
      total += (y2 - y1) * (y2 - y1);
      var rtn = (float) Math.Sqrt(total);
      return rtn;
    }

    /** Move / Crop items from the end until we're within the length limit */
    private void Crop() {
      var length = Length;
      while (length > MaxLength) {
        _segments.Dequeue();
        length -= SegmentSize;
      }
    }

    /** Return a set of line segments that describe this line. */
    public nGLine[] Points() {
      var size = _lastSeg ? _segments.Count : _segments.Count + 1;
      var rtn = new nGLine[size];
      var offset = 0;

      foreach (var s in _segments) {
        rtn [offset] = s;
        ++offset;
      }

      /* Extra segment for the last point? */
      if (!_lastSeg) {
        rtn[offset] = new nGLine() {
          P1 = new float[2] { _plast[0], _plast[1] },
          P2 = new float[2] { _last[0], _last[1] }
        };
      }

      return rtn;
    }
  }
}
