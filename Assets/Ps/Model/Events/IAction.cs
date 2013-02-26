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
using n.Gfx;
using System;

namespace Ps.Model.Events
{
  public class IAction : IEventData
  {
    private GameState _state;

    public IAction(GameState state) {
      _state = state;
    }

    public virtual int Id { 
      get {
        return ID.NoOp;
      }
    }

    public GameState State { 
      get {
        return _state;
      }
    }

    public IEnumerable<EventBinding> Handlers { get; set; }
  }
}
