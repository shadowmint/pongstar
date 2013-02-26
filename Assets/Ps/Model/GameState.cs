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
using Ps.Model.Ai;
using Ps.Controllers;
using n.Utils.Geom;

namespace Ps.Model
{
	public class GameState : nModel 
	{
    /** Current score */
    public Score Score { get; set; }

    /** The player paddle */
    public Paddle PlayerPaddle { get; set; }
    
    /** The AI paddle */
    public Paddle AiPaddle { get; set; }
    
    /** The ball position */
    public Ball Ball { get; set; }
    
    /** The field space */
    public Field Field { get; set; }
    
    /** Any sparkles we're currently displaying */
    public Sparkle Sparkle { get; set; }

    /** Collectables */
    public Collectables Collectables { get; set; }

    /** The current rainbow trail from the ball */
    public RainbowTrail RainbowTrail { get; set; }

    /** Victory flare */
    public Ps.Model.Object.Flare Flare { get; set; }
    
    /** The camera in use */
    private nCamera _camera;

    /** Event dispatcher */
    private EventHandler _events;

    /** Input handler */
    private GameInput _input;

    /** If the display is active */
    public bool Activated = false;

    /** Cached game bounds */
    private Rect _bounds;

    /** If bounds is set */
    private bool _boundsSet = false;

    /** An audio manager */
    public nAudio Audio { get; set; }

    /** Reset the current state */
    public void Reset() {
      Score = new Score();
      NewGame();
    }

    /** Reset the ball position and start again */
    public void NewGame() {

      PlayerPaddle = new Paddle(this);
      AiPaddle = new Paddle(this);
      Ball = new Ball(this);
      Field =  new Field(this, Camera());
      Sparkle = new Sparkle(this);
      RainbowTrail = new RainbowTrail(this);
      Flare = new Ps.Model.Object.Flare(this);
      Collectables = new Collectables(this);

      /* actions */
      SetupActions();
      _input = new GameInput(this);
      Game.SoundReset(); // reload 
      
      /* Set initial paddle positions */
      PlayerPaddle.Position[0] = 0f;
      PlayerPaddle.Position[1] = Field.Bounds[1] + nLayout.DistanceInWorld(14f, nAxis.Y, _camera);;
      AiPaddle.Position[0] = 0f;
      AiPaddle.Position[1] = Field.Bounds[3] - AiPaddle.Size[1] - nLayout.DistanceInWorld(7f, nAxis.Y, _camera);

      var direction = new nGLine() {
        P1 = new float[2] { 0, 0 },
        P2 = new float[2] { nRand.Float(0f, 1f), nRand.Float(1f, 0.3f) }
      };
      var unit = direction.Unit;
      Ball.Position[0] = 0;
      Ball.Position[1] = 0;
      Ball.Velocity[0] = unit[0] * 35f;
      Ball.Velocity[1] = unit[1] * 35f;

      /* reset paddles */
      AiPaddle.Ai = new EasyAiProfile();
      AiPaddle.Speed = AiPaddle.Ai.Speed;
      PlayerPaddle.Speed = Config.PlayerSpeed;
      AiPaddle.Position[0] = 0;
      PlayerPaddle.Position[0] = 0;

      /* ai */
      Activated = false;
      Camera().Pipe.Drawables.Clear();
      Camera().Pipe.Render();
    }

    /** Loads all the bound actions for the state */
    private void SetupActions() {
      _events = new EventHandler();
      _events.Listen(ID.PaddleHit, Sparkles.AddSparkles);
      _events.Listen(ID.WallHit, Sparkles.AddSparklesToWall);
      _events.Listen(ID.PaddleHit, Game.PlayerTouch);
      _events.Listen(ID.WallHit, Game.EndGame);
      _events.Listen(ID.PlayerInput, Game.PlayerMove);
      _events.Listen(ID.CollectableHit, Game.UpdateScore);
    }

    /** Trigger an event */
    public void Trigger(IEventData data) {
      _events.Trigger(data);
    }

    /** Get the game bounds */
    public Rect GameBounds() {
      if (!_boundsSet) {
        var cam = Camera();
        _bounds.xMin = nLayout.DistanceInWorld(2, nEdge.LEFT, cam);
        _bounds.xMax = nLayout.DistanceInWorld(2, nEdge.RIGHT, cam);
        _bounds.yMin = nLayout.DistanceInWorld(0, nEdge.BOTTOM, cam);
        _bounds.yMax = nLayout.DistanceInWorld(5, nEdge.TOP, cam);
        _boundsSet = true;
      }
      return _bounds;
    }

    /** Generate a camera to use */
    public nCamera Camera() {
      if (_camera == null) 
        _camera = new nCamera(Config.Height);
      return _camera;
    }

    /** Generate a ui camera to use */
    public nCamera UiCamera() {
      var c = new nCamera(Config.Height);
      return c;
    }

    /** Init things that need to be scene specific */
    public void Init(GameObject parent) {
      Audio = new nAudio(parent);
    }

    /** Check input events */
    public void Poll() {
      _input.Check(_events);
    }

    /** Update the gamestate */
    public void Update(float seconds) {
      Ball.Update(seconds, _events);
      PlayerPaddle.Update(seconds);
      AiPaddle.Update(seconds);
      Sparkle.Update(seconds);
      Field.Update(seconds);
      RainbowTrail.Update(seconds);
      Flare.Update(seconds);
      Collectables.Update(seconds);
      _input.Process(_events);
      _events.Dispatch();
    }
	}
}
