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

namespace n.Gfx
{
  /** A flat 2D textured quad */
  public class nQuad : nIDrawable
  {
    /** The sprite for this quad */
    public nSprite Data { get; set; }
    
    /** What we actually feed the renderer */
    private nSprite[] _data = null;

    public bool Invalid { get { return false; } }

    public nQuad() {
      Data = new nSprite(0);
      Data.Points.Set(new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 });
      Data.Position.Set(new float[2] { 0, 0 });
      Data.Color.Set(new float[4] { 1, 1, 1, 1 });
      Data.UV.Set(new float[8] { 1, 1, 1, 0, 0, 0, 0, 1 });
    }

    /** If you wrap this with another class you MUST call render each frame */
    public nSprite[] Render (nGraphicsPipe pipe)
    {
      if (_data == null)
        _data = new nSprite[1] { pipe.Sprite() };
      
      if (Data.Invalid) {
        _data[0].Load(Data);
        Data.Reset();
      }
      
      return _data;
    }
  }
}

