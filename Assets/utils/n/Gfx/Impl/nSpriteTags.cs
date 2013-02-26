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
using n.Gfx.Impl;

namespace n.Gfx.Impl
{
  /** Enum of changes */
  [Flags]
  public enum nSpriteTags {
    NONE = 0,
    POINTS = 0x01,
    UV = 0x02,
    TEXTURE = 0x04,
    SCALE = 0x08,
    POSITION = 0x10,
    ROTATION = 0x20,
    COLOR = 0x40,
    DEPTH = 0x80
  }
}
