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
  /** All the info about an event */
  public interface nIEvent 
  {
    int Id { get; }

    /* Time delta associated with this event */
    float Step { get; set; }

    IEnumerable<nEventBinding> Handlers { get; set; }

    nModel State { get; }
  }
}
