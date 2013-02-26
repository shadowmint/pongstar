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
using UnityEngine;
using n.Core;

namespace n.Gfx.Impl
{
  /** Float collection, because C# doesn't support indexed properties */
  public class nSpriteData {
  
    public nSpriteData(nSprite parent, nSpriteTags flag, int size) {
      _data = new float[size];
      _parent = parent;
      _flag = flag;
    }
    
    private nSpriteTags _flag;
    
    private nSprite _parent;
    
    private float[] _data;
    
    public float this [int i] {
      get {
        return _data[i];
      }
      set {
        _data[i] = value;
        _parent.Flag(_flag);
      }
    }
    
    /** Raw data array */
    public float[] Raw {
      get {
        return _data;
      }
    }
    
    /** Set the inner data array */
    public void Set (nSpriteData data)
    {
      _data = data.Raw;
      _parent.Flag(_flag);
    }
    
    /** Set the inner data array */
    public void Set (nDataArray data)
    {
      _data = data.Raw;
      _parent.Flag(_flag);
    }
    
    /** Set the inner data array */
    public void Set (float[] data)
    {
      _data = data;
      _parent.Flag(_flag);
    }
  }
}
