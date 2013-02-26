//
//  Copyright 2012  douglasl
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using UnityEngine;
using n.Core;
using n.Gfx.Impl;

namespace n.Gfx
{
  /** Single drawable object */
  public class nSprite {
  
    /** Internal values */
    private int _id = -1;
    private nSpriteData _points;
    private nSpriteData _uv;
    private nSpriteData _origin;
    private nSpriteData _position;
    private nSpriteData _color;
    private nSpriteData _scale;
    private nSpriteTags _flags = nSpriteTags.NONE;
    private Texture _texture = null;
    private float _rotation = 0f;
    private float _depth = 0f;
    
    public nSprite (int id)
    {
      _id = id;
      _points = new nSpriteData(this, nSpriteTags.POINTS, 8);
      _uv = new nSpriteData(this, nSpriteTags.UV, 8);
      _position = new nSpriteData(this, nSpriteTags.POSITION, 2);
      _scale = new nSpriteData(this, nSpriteTags.SCALE, 2);
      _color = new nSpriteData(this, nSpriteTags.COLOR, 4);
      _flags = nSpriteTags.NONE;
    }
    
    /** Copy all appropriate values from this source sprite */
    public void Load(nSprite src) {
      if (!src.Invalid)
        return;
      if ((src.Flags & nSpriteTags.COLOR) > 0) 
        Color.Set(src.Color);
      if ((src.Flags & nSpriteTags.DEPTH) > 0) 
        Depth = src.Depth;
      if ((src.Flags & nSpriteTags.POINTS) > 0) 
        Points.Set(src.Points);
      if ((src.Flags & nSpriteTags.POSITION) > 0) 
        Position.Set(src.Position);
      if ((src.Flags & nSpriteTags.SCALE) > 0) 
        Scale.Set(src.Scale);
      if ((src.Flags & nSpriteTags.ROTATION) > 0) 
        Rotation = src.Rotation;
      if ((src.Flags & nSpriteTags.TEXTURE) > 0) 
        Texture = src.Texture;
      if ((src.Flags & nSpriteTags.UV) > 0) 
        UV.Set(src.UV);
    }
    
    /** Reset invalid flag */
    public void Reset() {
      _flags = 0;
    }
    
    /** Set a flag */
    public void Flag(nSpriteTags flag) {
      _flags |= flag;
    }
    
    /** Unique ID for this sprite */
    public int Id { 
      get {
        return _id;
      }
    }
    
    /** 
     * Points in top-left, top-right, bottom-right, bottom-left over 
     * <p>
     * These points are all relative to the origin so a typical set might be:
     * 1,1 1,0 0,0 0,1
     * <p>
     * To then rotate around the center of this sprite, you would set the
     * origin value to 0.5,0.5
     */
    public nSpriteData Points {
      get {
        return _points;
      }
    }
    
    /** UV coordinate for the points */
    public nSpriteData UV{
      get {
        return _uv;
      }
    }
    
    /** The texture attached to this sprite */
    public Texture Texture{
      get {
        return _texture;
      }
      set {
        _texture = value;
        _flags |= nSpriteTags.TEXTURE;
      }
    }
    
    /** Position of the origin in world space */
    public nSpriteData Position{
      get {
        return _position;
      }
    }

    /** Scale of the sprite in world space */
    public nSpriteData Scale {
      get {
        return _scale;
      }
    }
    
    /** Rotation around the origin point (0,0) */
    public float Rotation{
      get {
        return _rotation;
      }
      set {
        _rotation = value;
        _flags |= nSpriteTags.ROTATION;
      }
    }
    
    /** Color tint for this sprite */
    public nSpriteData Color{
      get {
        return _color;
      }
    }
    
    /** Depth at which to render this sprite */
    public float Depth{
      get {
        return _depth;
      }
      set {
        _depth = value;
        _flags |= nSpriteTags.DEPTH;
      }
    }
    
    /** Flags which are active */
    public nSpriteTags Flags {
      get {
        return _flags;
      }
    }
    
    /** If this sprite has changed */
    public bool Invalid {
      get {
        return _flags != nSpriteTags.NONE;
      }
    }
  }
}

