/* 
 * Copyright 2012 Douglas Linder
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICEnsE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIOns OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using UnityEngine;
using n.Core;
using n.Gfx.Impl;

namespace n.Gfx
{
  /** Renders a border-only box */
  public class nBox : nIDrawable
  { 
    /** Actual data */
    private nDataArray _points = new nDataArray(8);
    private nDataArray _color = new nDataArray(4);
    private nDataArray _origin = new nDataArray(2);
    private nDataArray _position = new nDataArray(2);
    private float _rotation = 0;
    private float _thickness = 1;
    private float _depth = 0;

    public bool Invalid { get { return false; } }

    /** If data has been invalidated */
    private bool _invalid = true;

    /** Depth! */
    public float Depth {
      get {
        return _depth;
      }
      set {
        _depth = value;
        if (_data != null) {
          foreach (var i in _data) {
            i.Depth = Depth;
          }
        }
      }
    }
    
    /** Rendering data */
    private nSprite[] _data = null;
    
    /** Build points for a box */
    public void SetCorners(float x1, float y1, float x2, float y2) {
      var values = new float[4] { x1, y1, x2, y2 };
      x1 = values[0] > values[2] ? values[0] : values[2];
      x2 = values[0] < values[2] ? values[0] : values[2];
      y1 = values[1] > values[3] ? values[1] : values[3];
      y2 = values[1] < values[3] ? values[1] : values[3];
      Points.Set(new float[8] { x1, y1, x1, y2, x2, y2, x2, y1 });
      _data = null;
    }
    
    /** Build points for a box */
    public void SetSize(float x, float y, float width, float height) {
      var dx = width / 2.0f;
      var dy = height / 2.0f;
      Points.Set(new float[8] { x + dx, y + dy, x + dx, y - dy, x - dx, y - dy, x - dx, y + dy });
      _data = null;
    }
    
    /** Set this box from a sprite marker */
    public void SetParent(nSprite parent) {
      Position.Set(parent.Position.Raw);
      Points.Set(parent.Points.Raw);
      Rotation = parent.Rotation;
      _data = null;
    }
    
    /** The points for this box */
    public nDataArray Points {
      get {
        return _points;
      }
    }
    
    /** The color of the box */
    public nDataArray Color { 
      get {
        return _color;
      }
    }
    
    /** Position offset */
    public nDataArray Position { 
      get {
        return _position;
      }
    }
    
    /** Thickness of the line segments */
    public float Thickness {
      get { 
        return _thickness; 
      }
      set {
        _thickness = value;
        _invalid = true;
      }
    }
    
    /** Set the rotation angle */
    public float Rotation {
      get { 
        return _rotation; 
      }
      set {
        _rotation = value;
        _invalid = true;
      }
    }

    /** Return true if invalid */
    private bool Redraw ()
    {
      return _points.Invalid || _color.Invalid || _origin.Invalid || _position.Invalid || _invalid;
    }
    
    private float[] LineSegment(float x1, float y1, float x2, float y2) {
      var values = new float[4] { x1, y1, x2, y2 };
      x1 = values[0] > values[2] ? values[0] : values[2];
      x2 = values[0] < values[2] ? values[0] : values[2];
      y1 = values[1] > values[3] ? values[1] : values[3];
      y2 = values[1] < values[3] ? values[1] : values[3];
    
      var rtn = new float[8];
      var perp_y = x1 - x2; /* perp = (-y, x) */
      var perp_x = y2 - y1;
      var m = (float) Math.Sqrt(perp_x * perp_x + perp_y * perp_y);
      if (m > 0) {
        perp_x = perp_x / m * _thickness / 2.0f;
        perp_y = perp_y / m * _thickness / 2.0f;
        
        /* TR, BR, BL, TL with thickness for awesome */
        rtn[0] = x1 + perp_x;
        rtn[1] = y1 + perp_y;
        rtn[2] = x1 - perp_x;
        rtn[3] = y1 - perp_y;
        rtn[4] = x2 - perp_x;
        rtn[5] = y2 - perp_y;
        rtn[6] = x2 + perp_x;
        rtn[7] = y2 + perp_y;       
      }
      return rtn;
    }
    
    public nSprite[] Render (nGraphicsPipe pipe)
    {
      if (_data == null) {
        _data = new nSprite[4];
        for (var i = 0; i < _data.Length; ++i) {
          _data[i] = pipe.Sprite ();
          _data[i].Depth = _depth;
        }
        _data[0].Points.Set(LineSegment(_points[0], _points[1] + _thickness / 2.0f, _points[2], _points[3] - _thickness / 2.0f));
        _data[1].Points.Set(LineSegment(_points[2], _points[3], _points[4], _points[5]));
        _data[2].Points.Set(LineSegment(_points[4], _points[5] - _thickness / 2.0f, _points[6], _points[7] + _thickness / 2.0f));
        _data[3].Points.Set(LineSegment(_points[6], _points[7], _points[0], _points[1]));
      }
      
      /* Rebuild content? */
      if (Redraw()) {
        foreach (var item in _data) {
          item.Color.Set(_color);
          item.Rotation = _rotation;
          item.Position.Set(_position);
          item.Texture = (Texture) Resources.Load("Line.jpg");
        }
      }
      
      return _data;
    }
  }
}

