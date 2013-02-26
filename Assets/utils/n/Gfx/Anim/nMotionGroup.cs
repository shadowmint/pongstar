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
  /** Collect temporary sprites, eg. particles, with one of these */
  public class nMotionGroup 
  {
    /** Current set of mobiles */
    private List<nMotion> _mobiles = new List<nMotion>();

    /** Dead list */
    private List<nMotion> _dead = new List<nMotion>();

    /** Add a sprite to the cluster */
    public void Add(nMotion sprite) {
      _mobiles.Add(sprite);
    }

    /** Push the sprites from this cluster into the set */
    public void Render(nGraphicsPipe pipe, List<nSprite> sprites) {
      foreach (var m in _mobiles) {
        m.Sprites = m.Parent.Render(pipe);
        if (m.Sprites != null)
          sprites.AddRange(m.Sprites);
      }
    }

    /** Update all the sprites */
    public void Update(float seconds) {
      _dead.Clear();
      foreach (var m in _mobiles) {
        if (m.Sprites != null) {
          m.Motion.Update(m.Parent, m.Sprites, seconds);
          if (!m.Motion.Alive) {
            _dead.Add(m);
          }
        }
      }
      foreach (var d in _dead) {
        _mobiles.Remove(d);
      }
    }
  }
}

