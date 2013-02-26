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

using System;
using System.Collections.Generic;
using System.Linq;
using n.Core;

namespace n.Events
{
  /** Delegate type */
  public delegate void nEventCallback(nIEvent data);

  /** Allows events to be attached to delegates, and triggered */
  public class nEventHandler
  {
    /** Set of pending event objects */
    private List<nIEvent> _events = new List<nIEvent>();

    /** Set of delegate bindings */
    private List<nEventBinding> _bindings = new List<nEventBinding>();

    /** Add a callback */
    public void Listen(int id, nEventCallback callback, bool deleteAfterInvoke = false) {
      var binding = new nEventBinding() {
        Id = id,
        Callback = callback,
        DeleteAfterInvoke = deleteAfterInvoke
      };
      _bindings.Add(binding);
    }

    /** 
     * Queue an event callback. 
     * <p>
     * Note, events are not dispatched until Dispatch() is called.
     * Also, if there is no binding for an id, it is not saved.
     */
    public void Trigger(nIEvent data) {
      data.Handlers = (from h in _bindings where h.Id == data.Id select h).ToList(); 
      if (data.Handlers.Any())
        _events.Add(data);
    }

    /** Dispatch all events which are pending */
    public void Dispatch(float timestep) {
      foreach (var item in _events) {
        foreach (var h in item.Handlers) {
          item.Step = timestep;
          h.Callback(item);
          if ((h.DeleteAfterInvoke) && (_bindings.Contains(h))) {
            _bindings.Remove(h);
          }
        }
      }
      _events.Clear();
    }
  }
}

