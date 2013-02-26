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
	public class Config 
	{
    /** The height of the view to create */
    public static float Height = 100f;

    #region Score constants

    /** Points per bounce */
    public static int PointsPerBounce = 20;

    /** Points per paddle hit */
    public static int PointsPerPaddleBounce = 100;

    /** Win a round */
    public static int PointsPerWin = 5000;

    /** Lose a round */
    public static int PointsPerLoss = -5000;

    /** Points to win */
    public static int WinScore = 5;

    #endregion

    #region Player ability config 

    /** How fast the player can move */
    public static float PlayerSpeed = 1.5f;

    #endregion

    #region Collectable visual config

    /** How fast collectable nodes spin */
    public static float CollectableSpinRate = 30f;

    #endregion
	}
}
