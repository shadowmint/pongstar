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
using System.Collections.Generic;
using n.Core;

// TODO: Batch materials using the texture id as a key.
namespace n.Gfx.Impl
{
  /** Handles marshelling a raw sprite into a mesh to render */
  public class nSpriteMeshFactory
  {
    /** Cache */
    private IDictionary<int, nSpriteMesh> _cache = new Dictionary<int, nSpriteMesh>();
    
    /** Rebuild parts of a mesh without creating a new one */
    public void UpdateMesh(nSpriteMesh sm, nSprite sprite) {
      /* mesh, uvs, etc */
      if ((sprite.Flags & nSpriteTags.POINTS) > 0) {
        var verticies = new Vector3[4] {
          new Vector3(sprite.Points [0], sprite.Points [1], sprite.Depth),
          new Vector3(sprite.Points [2], sprite.Points [3], sprite.Depth),
          new Vector3(sprite.Points [4], sprite.Points [5], sprite.Depth),
          new Vector3(sprite.Points [6], sprite.Points [7], sprite.Depth)
        };
        sm.Mesh.vertices = verticies;
        
        /* Must set triangles after verticies */
        var triangles = new int[6] { 0, 1, 2, 0, 2, 3 };
        sm.Mesh.triangles = triangles;
        sm.Mesh.normals = new Vector3[4] {
          new Vector3(0f, 0f, 1f),
          new Vector3(0f, 0f, 1f),
          new Vector3(0f, 0f, 1f),
          new Vector3(0f, 0f, 1f)
        };
      }
      if ((sprite.Flags & nSpriteTags.UV) > 0) {
        var uvs = new UnityEngine.Vector2[4] {
          new UnityEngine.Vector2(sprite.UV [0], sprite.UV [1]),
          new UnityEngine.Vector2(sprite.UV [2], sprite.UV [3]),
          new UnityEngine.Vector2(sprite.UV [4], sprite.UV [5]),
          new UnityEngine.Vector2(sprite.UV [6], sprite.UV [7])
        };
        sm.Mesh.uv = uvs;
      }
      
      /* material */
      if ((sprite.Flags & nSpriteTags.COLOR) > 0)
        sm.Material.color = new Color(sprite.Color [0], sprite.Color [1], sprite.Color [2], sprite.Color [3]);
      if ((sprite.Flags & nSpriteTags.TEXTURE) > 0) 
        sm.Material.mainTexture = sprite.Texture;

      /* position */
      if ((sprite.Flags & nSpriteTags.POSITION) > 0) {
        sm.Position [0] = sprite.Position [0];
        sm.Position [1] = sprite.Position [1];
      }
      if ((sprite.Flags & nSpriteTags.DEPTH) > 0) {
        //sm.Position [2] = sprite.Depth;
        sm.Mesh.vertices[0][2] = sprite.Depth;
        sm.Mesh.vertices[1][2] = sprite.Depth;
        sm.Mesh.vertices[2][2] = sprite.Depth;
        sm.Mesh.vertices[3][2] = sprite.Depth;
      }

      /* position */
      if ((sprite.Flags & nSpriteTags.SCALE) > 0) {
        sm.Scale [0] = sprite.Scale [0];
        sm.Scale [1] = sprite.Scale [1];
        sm.Scale [2] = 1.0f;
      }

      /* rotation */
      if ((sprite.Flags & nSpriteTags.ROTATION) > 0) 
        sm.Rotation = Quaternion.AngleAxis(sprite.Rotation, new Vector3(0, 0, 1f));
      
      /* cache result */
      sprite.Reset();
    }
    
    /** Build a new mesh for a sprite */
    public nSpriteMesh CreateMesh(nSprite sprite, bool exists)
    {
      nSpriteMesh rtn = null;
      if (!exists) {
        var shader = Shader.Find("Custom/nPropShader");
        rtn = new nSpriteMesh() {
          Material = new Material(shader),
          Mesh = new Mesh(),
          Position = new Vector3(),
          Scale = new Vector3()
        };
        
        /* setup default rotation to avoid errors */
        rtn.Rotation = Quaternion.AngleAxis(sprite.Rotation, new Vector3(0, 0, 1f));
        
        _cache[sprite.Id] = rtn;
      } 
      else 
        rtn = _cache[sprite.Id];
      UpdateMesh(rtn, sprite);
      return rtn;
    }
    
    /** Convert a sprite into a renderable sprite */
    public nSpriteMesh Convert (nSprite sprite)
    {
      nSpriteMesh rtn = null;
      var exists = _cache.ContainsKey (sprite.Id);
      if (sprite.Invalid || !exists) 
        rtn = CreateMesh (sprite, exists);
      else 
        rtn = _cache [sprite.Id];
      return rtn;
    }
  }
}

