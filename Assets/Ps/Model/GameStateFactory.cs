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

namespace Ps.Model
{
	public class GameStateFactory : nStateFactory 
	{
    private static GameState _instance = null;

    public void Reset() {
      var r = new nResolver();
      _instance = r.Resolve<GameState>();
    }

    public nModel State {
      get {
        if (_instance == null) 
          Reset();
        return _instance;
      }
    }
	}
}
