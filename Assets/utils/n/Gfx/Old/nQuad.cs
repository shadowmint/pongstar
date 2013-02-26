//
//  Copyright 2012  doug
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

namespace n.Gfx.Old
{
  /** 
   * A flat 2D textured quad 
   * <p>
   * When you create the quad you must choose the size in world units.
   * The quad mesh is always centered around (0, 0); use transform to 
   * move it.
   */
  public class nQuad : nSpirit
  {
    public UnityEngine.Vector2[] UV { get; set; }

    public Texture Texture { get; set; }

    private Vector3[] _vertices;

    private int[] _triangles;

    protected Mesh _mesh;

    protected string _name;

    /** GameObject instances created will be named Name__[RandomNumber] */
    public nQuad (string name, UnityEngine.Vector2 size) {
      _name = name;
      Init(size);
    }

    /** GameObject instances created will be named Quad__[RandomNumber] */
    public nQuad(UnityEngine.Vector2 size) {
      _name = "Quad";
      Init(size);
    }
    
    /** Direct vertex access */
    public Vector3[] Layout {
      get {
        return _vertices;
      }
    }

    private void Init(UnityEngine.Vector2 size) {
      var x = size[0] / 2;
      var y = size[1] / 2;
      Texture = null;
      _mesh = new Mesh();
      _vertices = new Vector3[] {
        new Vector3( x, y,  0),
        new Vector3( x, -y, 0),
        new Vector3(-x, y, 0),
        new Vector3(-x, -y, 0),
      };
      UV = new UnityEngine.Vector2[] {
        new UnityEngine.Vector2(1, 1),
        new UnityEngine.Vector2(1, 0),
        new UnityEngine.Vector2(0, 1),
        new UnityEngine.Vector2(0, 0),
      };
      _triangles = new int[] {
        0, 1, 2,
        2, 1, 3,
      };
    }

    /** Generate a name for this quad */
    protected string Name () {
      var random = UnityEngine.Random.Range(0, int.MaxValue);
      var name = String.Format("{0}__{1}", _name, random);
      return name;
    }

    /** Create a GameObject for this quad and add it to the scene */
    public virtual GameObject Manifest ()
    {
      /** Update mesh to latest instance */
      _mesh.vertices = _vertices;
      _mesh.uv = UV;
      _mesh.triangles = _triangles;
      _mesh.RecalculateNormals();
      
      /* Create object and add to scene */
      var rtn = (GameObject)new GameObject (
        Name (),
        typeof(MeshRenderer), // Required to render
        typeof(MeshFilter),   // Required to have a mesh
        typeof(MeshCollider)  // Required to catch input events
      );
      rtn.GetComponent<MeshFilter> ().mesh = _mesh;
      rtn.GetComponent<MeshCollider> ().sharedMesh = _mesh;

      /* Set texture */
      if (Texture != null) {
        rtn.renderer.material.mainTexture = Texture;
        var shader = Shader.Find ("Custom/nPropShader");
        rtn.renderer.material.shader = shader;
        rtn.renderer.material.color = Color.white;
      }

      /* Set position */
      rtn.transform.position = new Vector3(0, 0, 0);

      return rtn;
    }
  }
}

