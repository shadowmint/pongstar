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
  public class nDataArray {
  
    public nDataArray(int size) {
      _data = new float[size];
    }
    
    private bool _invalid = true;
    
    private float[] _data;
    
    public float this [int i] {
      get {
        return _data[i];
      }
      set {
        _data[i] = value;
        _invalid = true;
      }
    }
    
    public bool Invalid {
      get {
        return _invalid;
      }
    }
    
    /** Raw data array */
    public float[] Raw {
      get {
        return _data;
      }
    }
    
    /** Return the array length */
    public int Length {
      get {
        return _data.Length;
      }
    }
    
    /** Set the inner data array */
    public void Set(nDataArray data)
    {
      _data = data.Raw;
      _invalid = true;
    }
    
    /** Set data using objects */
    public void Set(params float[] data) {
      if (_data.Length == data.Length) {
        _data = data;
        _invalid = true;
      } 
      else {
        bool mod = false;
        for (var i = 0; (i < _data.Length) && (i < data.Length); ++i) {
          _data [i] = data [i];
          mod = true;
        }
        if (mod)
          _invalid = true;
      }
    }
    
    /** Reset invalid state */
    public void Reset() {
      _invalid = false;
    }
  }
}
