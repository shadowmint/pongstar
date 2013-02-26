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
using n.Platform;
using n.Core;
using Ps.Controllers;
using Ps.Model;
using System;

namespace Ps 
{
	public class Pongstar : nApp 
	{
    /** Should we trigger a start action when we start? */
    private static bool _suppressStartAction = false;
    
    /** The global instance */
    private static Pongstar _instance = null;

    private static Pongstar Instance { 
      get {
        if (_instance == null) {
          _suppressStartAction = true;
          if (Camera.mainCamera != null) {
            Camera.mainCamera.gameObject.AddComponent<Pongstar>();
            _instance = Camera.mainCamera.gameObject.GetComponent<Pongstar>();
          }
        }
        return _instance;
      }
    }

		/** Get a controller */
		public static T Get<T>() {
			return Pongstar.Instance.Controller<T>();
		}

		protected override void setup (nResolver resolver) {
      resolver.Bind<nStateFactory, GameStateFactory>();
		}

    public void Start() {
      if (!_suppressStartAction) {
        _instance = this;
        Get<GameController>().Index();
      }
    }
	}
}
