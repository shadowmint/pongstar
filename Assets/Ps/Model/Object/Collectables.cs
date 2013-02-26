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
using System.Linq;
using n.Gfx;
using System;
using n.Utils;
using Ps.Controllers;
using n.Utils.Geom;
using n.Gfx.Anim;
using Ps.Model.Events;

namespace Ps.Model.Object
{
  public class Collectables : DrawableModel 
  {
    /** Agregate for all the sprites */
    private nSpriteGroup _sprites = new nSpriteGroup();

    /** Collection for the twinkles */
    private nMotionGroup _twinkles = new nMotionGroup();

    /** Collection for the bonus score notice */
    private nMotionGroup _scores = new nMotionGroup();

    /** Actual twinkles */
    public List<Twinkle> Twinkles = new List<Twinkle>();

    /** Count of visible twinkles */
    private int _twinkleCount = 0;

    /** Time since last twinkle appears */
    private float _lastTwinkle = 0;

    /** Maximum number of twinkles */
    private int _maxTwinkles = 10;

    /** Twinkle appears every this many seconds */
    private float _twinkleRate = 1f;

    /** The twinkle factory to make more with */
    private TwinkleFactory _factory;

    public Collectables(GameState parent) : base(parent) {
      _sprites.Add(_twinkles);
      _sprites.Add(_scores);
      _factory = new TwinkleFactory(_parent, _twinkles);
    }

    public override nIDrawable Display { 
      get { 
        return _sprites;
      } 
    }

    /** Update trail */
    public void Update(float seconds) {
      _lastTwinkle += seconds;
      if (_lastTwinkle > _twinkleRate) {
        _lastTwinkle = 0f;
        CreateTwinkle();
      }
      _sprites.Update(seconds);
      CheckBallCollisions(_parent.Ball);
      CheckRemoveRecords();
    }

    /** Check for invalid twinkles and remove them */
    private void CheckRemoveRecords() {
      var dead = (from t in Twinkles where t.Invalid select t).ToList();
      if (dead.Any()) {
        _twinkleCount -= dead.Count();
        _lastTwinkle = 0f;
        foreach (var t in dead) {
          Twinkles.Remove(t);
        }
      }
    }

    /** Create a new twinkle */
    private void CreateTwinkle() {
      if (_twinkleCount < _maxTwinkles) {
        _factory.Manufacture(this);
        ++_twinkleCount;
      }
    }

    /** Create a score node */
    public void CreateScore(Twinkle parent) {
      var sc = new ScoreDsp() {
        Position = new float[2] { parent.Position[0], parent.Position[1] },
        Size = new float[2] { 6f, 3f },
        Float = 5.0f
      };
      var m = new nMotion(sc, new ScoreAnim() {
        Lifespan = 2f
      });
      _scores.Add(m);
      _parent.Trigger(new CollectableHit(_parent, parent.Points));
    }

    /** Check if the ball collides with any twinkles and convert into a score */
    public void CheckBallCollisions(Ball b) {
      if (b != null) {
        var qb = new nGQuad(5f).Offset(b.Position);
        foreach (var t in Twinkles) {
          var qc = new nGQuad(t.Size).Offset(t.Position);
          if (qc.Intersects(qb)) {
            t.Die();
            CreateScore(t);
          }
        }
      }
    }
  }

  /** Twinkle type */
  public class Twinkle : nIDrawable {

    /** Kill a twinkle */
    public void Die() {
      Invalid = true;
    }

    /** If we're dead yet */
    public bool Invalid { get; private set; }

    /** Points for this */
    public int Points { get; set; }

    /** The actual drawable for this twinkle */
    private nSprite[] _sprites = null;

    /** Size of this twinkle */
    public float Size { get; set; }

    /** Color base */
    public float[] Color { get; set; }

    /** Actual spin rate */
    public float SpinRate { get; set; }

    /** Position of this twinkle */
    public float[] Position { get; set; }

    public nSprite[] Render(nGraphicsPipe pipe) {
      if (Invalid) return null;
      if (_sprites == null) {
        _sprites = new nSprite[1];
        _sprites[0] = pipe.Sprite();
        _sprites[0].Points.Set(new nGQuad(Size).Points);
        _sprites[0].Texture = (Texture) Resources.Load("Line");
        _sprites[0].Position.Set(Position);
        _sprites[0].Color.Set(Color);
        _sprites[0].Depth = nRand.Float(4f, 1f);
      }
      return _sprites;
    }
  }

  /** Twinkle animation */
  public class TwinkleAnim : nIAnim {

    public float Lifespan { get; set; }

    public float Life { get; set; }

    private Twinkle _parent;

    public void Update(nIDrawable Parent, nSprite[] sprites, float seconds) {
      _parent = (Twinkle)Parent;
      Life += seconds;
      var visible = 0.5f;
      var factor = 0f;
      if (Life < visible) 
        factor = (Life / visible);
      else
        factor = 1.0f - (Life / (Lifespan - visible));

      sprites[0].Rotation += _parent.SpinRate * seconds;
      sprites[0].Color[3] = factor;
    }

    public bool Alive {
      get {
        if (_parent == null) return true;
        var alive = !_parent.Invalid && (Life <= Lifespan);
        if (!alive) {
          _parent.Die();
        }
        return alive;
      }
    }
  }

  /** Twinkle factory */
  public class TwinkleFactory {

    private nMotionGroup _twinks;

    private GameState _state;

    public TwinkleFactory(GameState state, nMotionGroup twinks) { 
      _twinks = twinks;
      _state = state;
    }

    private Twinkle MakeTwinkle(Collectables parent) {
      var bounds = _state.GameBounds();
      var t = new Twinkle() {
        Size = nRand.Float(5f, 1f),
        Points = 500,
        Position = new float[2] {
          nRand.Float(0, bounds.xMax * 3f / 4f),
          nRand.Float(0, bounds.yMax * 3f / 4f)
        },
        Color = new float[4] {
          nRand.Float(0.9f, 0f, 0.2f), 
          nRand.Float(0.9f, 0f, 0.2f), 
          nRand.Float(0.9f, 0f, 0.2f), 
          0f
        },
        SpinRate = nRand.Float(0, Config.CollectableSpinRate)
      };
      return t;
    }

    private TwinkleAnim MakeTwinkleAnim() {
      return new TwinkleAnim() {
        Lifespan = nRand.Float(15f, 5f),
        Life = 0f,
      };
    }

    public void Manufacture(Collectables parent) {
      var m = new nMotion(MakeTwinkle(parent), MakeTwinkleAnim());
      _twinks.Add(m);
      parent.Twinkles.Add((Twinkle) m.Parent);
    }
  }

  /** Score type */
  public class ScoreDsp : nIDrawable {
    
    /** Kill a twinkle */
    public void Die() {
      Invalid = true;
    }
    
    /** If we're dead yet */
    public bool Invalid { get; private set; }
    
    /** The actual drawable for this twinkle */
    private nSprite[] _sprites = null;
    
    /** Size of this twinkle */
    public float[] Size { get; set; }

    /** Float rate */
    public float Float { get; set; }
    
    /** Position of this twinkle */
    public float[] Position { get; set; }
    
    public nSprite[] Render(nGraphicsPipe pipe) {
      if (Invalid) return null;
      if (_sprites == null) {
        _sprites = new nSprite[1];
        _sprites[0] = pipe.Sprite();
        _sprites[0].Points.Set(new nGQuad(Size[0], Size[1]).Points);
        _sprites[0].Texture = (Texture) Resources.Load("points500");
        _sprites[0].Position.Set(Position);
        _sprites[0].Depth = nRand.Float(3f, 1f);
        _sprites[0].Color.Set(new float[4] { 
          nRand.Float(0f, 1f, 0f), 
          nRand.Float(0f, 1f, 0f), 
          nRand.Float(0f, 1f, 0f), 
          1f
        });
      }
      return _sprites;
    }
  }
  
  /** Twinkle animation */
  public class ScoreAnim : nIAnim {
    
    public float Lifespan { get; set; }
    
    public float Life { get; set; }
    
    private ScoreDsp _parent;
    
    public void Update(nIDrawable Parent, nSprite[] sprites, float seconds) {
      _parent = (ScoreDsp) Parent;
      Life += seconds;
      var factor = 1.0f - (Life / Lifespan);
      sprites[0].Color[3] = factor;
      sprites[0].Position[1] += _parent.Float * seconds;
    }
    
    public bool Alive {
      get {
        if (_parent == null) return true;
        var alive = !_parent.Invalid && (Life <= Lifespan);
        if (!alive) {
          _parent.Die();
        }
        return alive;
      }
    }
  }
}
