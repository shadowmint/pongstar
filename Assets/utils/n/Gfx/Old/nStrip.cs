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
using System.Collections.Generic;

namespace n.Gfx.Old
{
  /** 
   * A flat 2D triangle strip where each pair of triangles forms a line segment.
   */
  public class nStrip : nSpirit
  {
    private UnityEngine.Vector2[] _uv;
    private Vector3[] _vertices;
    private int[] _triangles;
    protected Mesh _mesh;
    protected string _name;
    public Texture Texture { get; set; }

    /** GameObject instances created will be named Name__[RandomNumber] */
    public nStrip(string name, UnityEngine.Vector2[] points, float width)
    {
      _name = name;
      Init(points, width);
    }

    /** GameObject instances created will be named Strip__[RandomNumber] */
    public nStrip(UnityEngine.Vector2[] points, float width)
    {
      _name = "Strip";
      Init(points, width);
    }

    /** GameObject instances created will be named Name__[RandomNumber] */
    public nStrip(string name, UnityEngine.Vector2[] points, float[] widths)
    {
      _name = name;
      Init(points, widths);
    }
    
    /** GameObject instances created will be named Strip__[RandomNumber] */
    public nStrip(UnityEngine.Vector2[] points, float[] widths)
    {
      _name = "Strip";
      Init(points, widths);
    }

    private void Init(UnityEngine.Vector2[] points, float width)
    {
      var widths = new float[points.Length];
      for (int i = 0; i < points.Length; ++i) {
        widths [i] = width;
      }
      Init(points, widths);
    }

    /** 
     * The coordinate point set must be length 4 
     * <p>
     * Notice that C# uses pass by value; use; x.v = PopulateLineSegment(x.v); 
     */
    public static void PopulateLineSegment(Vector3[] points, UnityEngine.Vector2 start, UnityEngine.Vector2 end, float startWidth, float endWidth) {
      var q = end - start;
      var x = new Vector3(q [1], -q [0], 0);
      var w1 = startWidth;
      var w2 = endWidth;
      
      /* Some kind of weird issue prevents unity from rendering tiny width meshes.
       * This is minimum size of a half-strip-width that seems to render ok. */
      var minStripWidth = 0.01;
      
      /* Start point */
      x.Normalize();
      var y = new UnityEngine.Vector2(x[0], x[1]);
      y.Scale(new Vector3(w1 / 2.0f, w1 / 2.0f, 0f));
      if (y.magnitude < minStripWidth) {
        y = new UnityEngine.Vector2(x[0], x[1]);
        y[0] = (float) ((double) y[0] * minStripWidth);
        y[1] = (float) ((double) y[1] * minStripWidth);
      }
      points[0] = new Vector3(start [0] + y[0], start [1] + y[1], 0);
      points[1] = new Vector3(start [0] - y[0], start [1] - y[1], 0);
      
      /* End point */
      y = new UnityEngine.Vector2(x[0], x[1]);
      y.Scale(new Vector3(w2 / 2.0f, w2 / 2.0f, 0f));
      if (y.magnitude < minStripWidth) {
        y = new UnityEngine.Vector2(x[0], x[1]);
        y[0] = (float) ((double) y[0] * minStripWidth);
        y[1] = (float) ((double) y[1] * minStripWidth);
      }
      points[2] = new Vector3(end [0] + y[0], end[1] + y[1], 0);
      points[3] = new Vector3(end [0] - y[0], end[1] - y[1], 0);
    }
    
    private void Init(UnityEngine.Vector2[] points, float[] widths)
    {
      if (points.Length > 1) {
      
        /* generate a set of points for this line segment */
        var segments = points.Length - 1;
        var coords = new Dictionary<int, Vector3[]>();
        for (int i = 0; i < segments; ++i) {
          coords [i] = new Vector3[4];
          var start = points [i];
          var end = points [i + 1];
          var w1 = widths[i];
          var w2 = widths[i + 1];
          PopulateLineSegment(coords[i], start, end, w1, w2);
        }
        
        /* generate a vertex set for this mesh */
        _vertices = new Vector3[4 * segments];
        for (var i = 0; i < segments; ++i) {
          _vertices [i * 4 + 0] = coords [i] [0];
          _vertices [i * 4 + 1] = coords [i] [1];
          _vertices [i * 4 + 2] = coords [i] [2];
          _vertices [i * 4 + 3] = coords [i] [3];
        }

        /* generate UVs even though we wont really be using them */
        _uv = new UnityEngine.Vector2[4 * segments];
        for (var i = 0; i < segments; ++i) {
          _uv [i * 4 + 0] = new UnityEngine.Vector2(1, 1);
          _uv [i * 4 + 1] = new UnityEngine.Vector2(1, 0);
          _uv [i * 4 + 2] = new UnityEngine.Vector2(0, 1);
          _uv [i * 4 + 3] = new UnityEngine.Vector2(0, 0);
        }
        
        /* generate triangle set */
        _triangles = new int[6 * segments];
        for (var i = 0; i < segments; ++i) {
          _triangles [i * 6 + 0] = i * 4 + 0;
          _triangles [i * 6 + 1] = i * 4 + 1;
          _triangles [i * 6 + 2] = i * 4 + 2;
          _triangles [i * 6 + 3] = i * 4 + 2;
          _triangles [i * 6 + 4] = i * 4 + 1;
          _triangles [i * 6 + 5] = i * 4 + 3;
        }

        _mesh = new Mesh();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uv;
        _mesh.RecalculateNormals();
      }
    }

    /** Generate a name for this quad */
    protected string Name()
    {
      var random = UnityEngine.Random.Range(0, int.MaxValue);
      var name = String.Format("{0}__{1}", _name, random);
      return name;
    }

    /** Create a GameObject for this quad and add it to the scene */
    public virtual GameObject Manifest()
    {
      /* Create object and add to scene */
      var rtn = (GameObject)new GameObject(
        Name(),
        typeof(MeshRenderer), // Required to render
        typeof(MeshFilter),   // Required to have a mesh
        typeof(MeshCollider)  // Required to catch input events
      );
      rtn.GetComponent<MeshFilter>().mesh = _mesh;
      rtn.GetComponent<MeshCollider>().sharedMesh = _mesh;

      var shader = Shader.Find ("Custom/nPropShader");
      rtn.renderer.material.shader = shader;
      rtn.renderer.material.color = Color.white;
      if (Texture != null) 
        rtn.renderer.material.mainTexture = Texture;

      /* Set position */
      rtn.transform.position = new Vector3(0, 0, 0);

      return rtn;
    }
  }
}

