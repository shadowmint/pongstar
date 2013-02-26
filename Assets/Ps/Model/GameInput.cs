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
using Ps.Model.Object;
using Ps.Model.Events;
using n.Utils;
using Ps.Model.Actions;

namespace Ps.Model
{
	public class GameInput 
	{
    private GameState _state;

    private PlayerInputType _direction = PlayerInputType.NONE;
    private bool _trackingTouch = false;
    private bool _trackingTarget = false;
    private float _target;
    private float _trackDistance;

    public GameInput(GameState state) {
      _state = state;
      _trackDistance = nLayout.DistanceInWorld(0.5f, nAxis.X, _state.Camera());
    }

    private void ProcessMouseInput(bool up, float x) {
      if (up) {
        _direction = PlayerInputType.NONE;
        _trackingTarget = false;
      }
      else {
        var distance = Math.Abs(_state.PlayerPaddle.Position[0] - x);
        var left = _state.PlayerPaddle.Position[0] < x;
        if (distance > _trackDistance) {
          if (!left) {
            _direction = PlayerInputType.LEFT;
            _target = x;
            _trackingTarget = true;
          }
          else {
            _direction = PlayerInputType.RIGHT;
            _target = x;
            _trackingTarget = true;
          }
        }
      }
    }

    public void Check(EventHandler events) {

      var e = Event.current;
      if (e == null)
        return;

      if (e.isKey) {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
          _direction = PlayerInputType.LEFT;
        else if ((_direction == PlayerInputType.LEFT) && (Input.GetKeyUp(KeyCode.LeftArrow))) 
          _direction = PlayerInputType.NONE;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
          _direction = PlayerInputType.RIGHT;
        else if ((_direction == PlayerInputType.RIGHT) && (Input.GetKeyUp(KeyCode.RightArrow)))
          _direction = PlayerInputType.NONE;
      }

      else if (e.isMouse) {
        var up = Input.GetMouseButtonUp(0);
        var pos = _state.Camera().ScreenPointToWorld(e.mousePosition)[0];
        ProcessMouseInput(up, pos);
      }

      else {
        if (UnityEngine.Input.touchCount > 0) {
          var up = false;
          var pos = _state.Camera().ScreenPointToWorld(Input.touches[0].position)[0];
          _trackingTouch = true;
          ProcessMouseInput(up, pos);
        }
        else if (_trackingTouch) {
          ProcessMouseInput(true, 0);
        }
      }
    }

    public void Process(EventHandler events) {
      if (_direction != PlayerInputType.NONE) {
        var delta = _state.PlayerPaddle.Position[0] - _target;
        events.Trigger(new PlayerInput(_state, _state.PlayerPaddle, _direction, delta));
      }
      if (_trackingTarget) {
        if (Math.Abs(_state.PlayerPaddle.Position [0] - _target) < _state.PlayerPaddle.Size [0] / 2f)
          _direction = PlayerInputType.NONE;
      }
    }
  }
}
