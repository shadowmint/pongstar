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
  /** Renders a solid block that takes up the entire window */
  public class nBackground : nIDrawable
  {
    public nBackground(nCamera camera) {
      _camera = camera;
      _color.Set (new float[4] { 1, 1, 1, 1 });
      TilesHigh = 1;
      TilesWide = 1;
    }
    
    /** If we have to do an update */
    private bool _invalid = true;

    /** Actual data */
    private nDataArray _color = new nDataArray(4);
    private nDataArray _size = new nDataArray(2);
    private nDataArray _offset = new nDataArray(2);
    public Texture _texture = null;
    private nCamera _camera;
    private float[] _uv = null;
    private int _depth = 256;

    /** Set the UV coordinates manually */
    public void UV(float[] points) {
      _uv = points;

    }

    /** Color tint for background */
    public nDataArray Color { 
      get {
        return _color;
      }
    }
    
    /** Modify the actual depth using this */
    public int Depth { 
      get {
        return _depth;
      }
      set{
        _depth = value;
        _invalid = true;
      }
    }
    
    /** Texture */
    public Texture Texture {
      get {
        return _texture;
      }
      set {
        _texture = value;
        _invalid = true;
      }
    }
    
    /** Size in x and y of each background tile */
    public nDataArray Size { 
      get {
        return _size;
      }
    }
    
    /** Shortcut for automatically setting the size */
    public int TilesHigh {
      set {
        Size[1] = _camera.ScreenBounds.height / value;
      }
    }
    
    public int TilesWide {
      set {
        Size[0] = _camera.ScreenBounds.width / value;
      }
    }
    
    /** If you want to do something fancy like an animated background */
    public nDataArray Offset {
      get {
        return _offset;
      }
    }
    
    /** What we actually feed the renderer */
    private nSprite[] _data = null;

    /** Is this drawable inactive? */
    public bool Invalid { get { return false; } }

    /** Check if we're invalid? */
    private bool Rebuild() {
      return _invalid || _size.Invalid || _color.Invalid;
    }
    
    /** Reset invalid flag */
    private void Reset() {
      _invalid = false;
      _size.Reset();
      _color.Reset();
    }
    
    public nSprite[] Render (nGraphicsPipe pipe)
    {
      /* We can't reuse our sprites if we're changing sizes; reset them */
      if (_size.Invalid) 
        _data = null;
        
      if (Rebuild()) {
        var pos = _camera.ScreenBounds;
        var wide = (int)Math.Ceiling (pos.width / Size [0]);
        var high = (int)Math.Ceiling (pos.height / Size [1]);
        
        /* Always add one extra tile in every direction, to account for offset */
        wide += 2;
        high += 2;
        
        /* new set of sprites */
        if (_data == null) {
          _data = new nSprite[wide * high];
          for (var i = 0; i < _data.Length; ++i) {
            _data [i] = pipe.Sprite ();
          }
        }
        
        for (var x = 0; x < wide; ++x) {
          for (var y = 0; y < high; ++y) {
          
            /* Tile coordinates */
            var xMin = pos.x + Size [0] * x;
            var xMax = xMin + Size [0];
            var yMin = pos.y + Size [1] * y;
            var yMax = yMin + Size [1];
            
            /* Apply offset for hidden bits */
            xMin -= Size[0];
            xMax -= Size[0];
            yMin -= Size[1];
            yMax -= Size[1];
            
            /* Set sprite to cover entire size of window */
            var s = _data [y * wide + x];
            s.Depth = _depth;
            s.Texture = Texture;
            s.Color.Set (Color);
            //s.Points.Set (new float[8] {xMax, yMax, xMax, yMin, xMin, yMin, xMin, yMax});
            s.Points.Set (new float[8] {xMax, yMax, xMin, yMax, xMin, yMin, xMax, yMin});
          }
        }
        Reset ();
      }
      
      /* Moved is a special case; don't rebuild if all we did was shuffle a little */
      if (_offset.Invalid) {
        
        /* Offset cannot be larger than the size of the tile */
        var ox = _offset[0] % Size[0];
        var oy = _offset[1] % Size[1];
      
        for (int i = 0; i < _data.Length; ++i) {
          _data[i].Position[0] = ox;
          _data[i].Position[1] = oy;
          if (_uv != null) {
            _data[i].UV.Set (_uv);
          }
        }
        _uv = null;
        _offset.Reset();
      }
      return _data;
    }
  }
}

