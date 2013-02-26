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
using UnityEngine;
using System.Collections;
using n.Core;
using System.Collections.Generic;
using System.Linq;
using n.Gfx;
using System;
using n.Utils;
using Ps.Controllers;
using n.Utils.Geom;
using n.Gfx.Anim;

namespace Ps.Model.Object
{

  /** A trail object */

  /** Fade after use */
  public class RainbowSparkAnim : SparkAnim
  {
    protected override void Setup(nSprite display) {
      display.Texture = (Texture)Resources.Load("glow");
      display.Points.Set(new float[8] { 3, 3, 3, -3, -3, -3, -3, 3 });
      display.Color.Set(new float[4] { 1f, 1f, 1f, 0f });
      display.Depth = 20f;
    }
  }
}
