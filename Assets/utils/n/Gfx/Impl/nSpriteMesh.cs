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
using System.Collections.Generic;

namespace n.Gfx.Impl
{
  /** Drawable data for a sprite */
  public class nSpriteMesh
  {
    /** Raw mesh */
    public Mesh Mesh;
    
    /** Material */
    public Material Material;
    
    /** Rotation vector */
    public Quaternion Rotation;
    
    /** Position vector */
    public Vector3 Position;

    /** Scale vector */
    public Vector3 Scale;
  }
}

