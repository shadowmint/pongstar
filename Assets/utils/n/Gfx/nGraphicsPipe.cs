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

/** Debug control */
// #define DEBUG 

using System;
using System.Collections.Generic;
using n.Gfx.Impl;
using UnityEngine;
using n.Core;

namespace n.Gfx
{
  public class nGraphicsPipe
  {
    /** Mesh converter */
    private nSpriteMeshFactory _meshConverter = new nSpriteMeshFactory();
    
    /** Set of drawables to render */
    public List<nIDrawable> Drawables = new List<nIDrawable>();
    
    /** Sprite id factory */
    private int _spriteIdBase = 0;
    
    /** Sorting helper */
    private nGraphicsSpriteSorter _sorter = new nGraphicsSpriteSorter();

    /** Cache containers */
    private object[] _spriteSets = null;
    private nSprite[] _sprites = null;
    
    /** Generate the next sprite id */
    private int NextSpriteId() {
      ++_spriteIdBase;
      return _spriteIdBase;
    }
    
    /** Return a new sprite instance */
    public nSprite Sprite() {
      var rtn = new nSprite(NextSpriteId());
      rtn.Points.Set(new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 });
      rtn.Position.Set(new float[2] { 0, 0 });
      rtn.Color.Set(new float[4] { 1, 1, 1, 1 });
      rtn.UV.Set(new float[8] { 1, 1, 1, 0, 0, 0, 0, 1 });
      rtn.Scale.Set(new float[2] { 1, 1 });
      return rtn;
    }
    
    /** Draw this frame */
    public void Render() {

      /* Rebuild render cache */
      if ((_spriteSets == null) || (_spriteSets.Length != Drawables.Count))
        _spriteSets = new object[Drawables.Count];
     
      /** Reset things to avoid Bad Stuff */
      for (var i = 0; i < _spriteSets.Length; ++i) { _spriteSets[i] = null; }

      /* Count */
      var size = 0;
      var offset = 0;
      foreach (var d in Drawables) {
        if (d != null) {
          var items = d.Render(this);
          _spriteSets [offset] = items;
          ++offset;
          var count = 0;
          if (items != null) {
            foreach (var x in items) {
              if (x != null)
                ++count;
            }
            size += count;
          }
        }
      }

      /* Resize if not large enough */
      _sprites = new nSprite[size];

      #if DEBUG 
        nLog.Debug("Found {0} sprites to draw this frame", _sprites.Length);
      #endif

      /* Collect */
      if (size > 0) {
          offset = 0;
        for (var i = 0; i < _spriteSets.Length; ++i) {
          var spriteSet = (nSprite[]) _spriteSets[i];
          if ((spriteSet != null) && (spriteSet.Length > 0)) {
            for (var j = 0; j < spriteSet.Length; ++j) {
              if (spriteSet[j] != null) {
                _sprites[offset] = spriteSet[j];
                ++offset;
              }
            }
          }
        }
        
        /* Sort */
        Array.Sort(_sprites, _sorter);
     
        /* Render in correct order */
        var depth = _sprites[0] != null ? _sprites[0].Depth : 0;
        var count = 0;
        for(var i = 0; i < size; ++i) {
          if (_sprites[i] != null) {
            if (_sprites[i].Depth != depth) {
              #if DEBUG 
                nLog.Debug("Rendered {0} sprites at depth {1}", count, depth);
              #endif
              depth = _sprites[i].Depth;
              count = 0;
            }
            ++count;
            var m = _meshConverter.Convert(_sprites[i]);
            for (var j = 0; j < m.Material.passCount; ++j) {
              m.Material.SetPass(j);
              Graphics.DrawMeshNow(m.Mesh, Matrix4x4.TRS(m.Position, m.Rotation, m.Scale));
            }
            // Graphics.DrawMesh(m.Mesh, Matrix4x4.TRS(m.Position, m.Rotation, m.Scale), m.Material, 0);
            #if DEBUG 
              nLog.Debug("Draw sprite: {0}, {1}, {2}, {3}", m.Mesh.vertices[0], m.Mesh.vertices[1], m.Mesh.vertices[2], m.Mesh.vertices[3] );
              nLog.Debug("Color: {0}", m.Material.color);
            #endif
          }
        }
        #if DEBUG 
          nLog.Debug("Rendered {0} sprites at depth {1}", count, depth);
        #endif
      }
    }
  }
  
  /** For sorting sprites */
  public class nGraphicsSpriteSorter : IComparer<nSprite> {
    public int Compare (nSprite x, nSprite y)
    {
      if ((x == null) || (y == null))
        return 0;
      if (x.Depth < y.Depth)
        return 1;
      else if (x.Depth > y.Depth)
        return -1;
      else 
        return 0;
    }
  }
}
  
