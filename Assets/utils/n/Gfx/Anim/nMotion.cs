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

namespace n.Gfx.Anim
{
  /** An animated sprite of some type */
  public class nMotion
  {
    public nMotion(nIDrawable parent, nIAnim motion) {
      Parent = parent;
      Motion = motion;
      Sprites = null;
    }

    public nIDrawable Parent { get; private set; }

    public nIAnim Motion { get; private set; }

    public nSprite[] Sprites { get; set; }
  }
}

