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

namespace Ps.Model
{
	public class GameStateViewModel : nModel
	{
    public GameStateViewModel(GameState state, nCamera camera) {
      Items = new List<nIDrawable> {
        state.Field.Display,
        state.PlayerPaddle.Display,
        state.AiPaddle.Display,
        state.Ball.Display,
        state.Sparkle.Display,
        state.RainbowTrail.Display,
        state.Flare.Display,
        state.Collectables.Display
      };
      Camera = camera;
    }
    
    /** Set of drawable for this game */
    public List<nIDrawable> Items { get; set; }
    
    /** The camera for this view */
    public nCamera Camera { get; set; }
	}
}
