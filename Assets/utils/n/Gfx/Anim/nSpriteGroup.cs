//
//  Copyright 2012  douglasl
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Collections.Generic;
using n.Core;

namespace n.Gfx.Anim
{
  /** Agregate groups of drawables of and sprite clusters */
  public class nSpriteGroup : nIDrawable
  {
    /** Current set of drawables */
    private List<nIDrawable> _drawables = new List<nIDrawable>();

    /** Current set of clusters */
    private List<nMotionGroup> _clusters = new List<nMotionGroup>();

    /** Sprite array */
    private List<nSprite> _sprites = new List<nSprite>();

    /** Dead drawables */
    private List<nIDrawable> _dead = new List<nIDrawable>();

    /** Check for participant */
    public bool Contains (nIDrawable drawable)
    {
      return _drawables.Contains(drawable);
    }

    /** Check for participant */
    public bool Contains (nMotionGroup target)
    {
      return _clusters.Contains(target);
    }

    public void Clear() {
      _drawables = new List<nIDrawable>();
      _clusters = new List<nMotionGroup>();
      _sprites = new List<nSprite>();
    }

    public bool Invalid { get { return false; } }

    public void Add(nIDrawable drawable) {
      _drawables.Add(drawable);
    }

    public void Add(nMotionGroup cluster) {
      _clusters.Add(cluster);
    }

    /** Regenerate all the sprites in the cluster */
    public nSprite[] Render(nGraphicsPipe pipe) {
      _sprites.Clear();
      for (var j = 0; j < _drawables.Count; ++j) {
        var items = _drawables[j].Render(pipe);
        _sprites.AddRange(items);
      }
      foreach (var c in _clusters) {
        c.Render(pipe, _sprites);
      }
      var rtn = _sprites.ToArray();
      return rtn;
    }

    /** Update all the sprites */
    public void Update (float seconds)
    {
      foreach (var c in _clusters) {
        c.Update (seconds);
      }
      _dead.Clear ();
      foreach (var d in _drawables) {
        if (d.Invalid) 
          _dead.Add (d);
      }
      foreach (var d in _dead) {
        _drawables.Remove(d);
      }
    }
  }
}

