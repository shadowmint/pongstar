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
using System.Linq;

namespace n.Gfx.Old
{
  /** 
   * An instance of a quad on the scene.
   * <p>
   * Notice that a Prop defines position, rotation and scale;
   * but the UV coordinates are per quad, not per prop. To allow
   * custom UV per instance (eg. animation) create the prop from
   * a resource, not a Quad.
   * <p>
   * To manifest the prop, add it to a layer. To remove it, remove
   * it from the layer. This will add it to the scene or remove it.
   */
  public class nProp
  {
    protected nSpirit _spirit;

    private GameObject _instance = null;

    private float _rotation = 0;

    private IList<nPropEvent> _events = new List<nPropEvent>();
    
    private Vector3[] _vertices;

    /** User data attached to this prop */
    public object Data { get; set; }

    public nProp (string texture, UnityEngine.Vector2 size)
    {
      var t = (Texture) Resources.Load(texture);
      _spirit = new nQuad(size);
      _spirit.Texture = t;
    }

    public nProp (Texture texture, UnityEngine.Vector2 size)
    {
      _spirit = new nQuad(size);
      _spirit.Texture = texture;
    }

    public nProp(nSpirit parent) {
      _spirit = parent;
    }

    /** Position */
    public UnityEngine.Vector2 Position {
      get { 
        var p = _instance.transform.position;
        return new UnityEngine.Vector2 (p [0], p [1]);
      }
      set { 
        if (_instance != null) {
          var p = _instance.transform.position;
          _instance.transform.position = new Vector3(value[0], value[1], p[2]);
        }
      }
    }

    /** Scale */
    public UnityEngine.Vector2 Scale {
      get { 
        var s = _instance.transform.localScale;
        return new UnityEngine.Vector2(s[0], s[1]);
      }
      set {
        if (_instance != null) {
          _instance.transform.localScale = new Vector3(value[0], value[1], 1);
        }
      }
    }

    /** Z-index */
    public float Depth {
      get { return _instance.transform.position[2]; }
      set { 
        if (_instance != null) {
          _instance.transform.position = new Vector3(
            _instance.transform.position[0],
            _instance.transform.position[1],
            value
          );
        }
      }
    }

    /** Push an update to vertex data to the game object */
    public void PushVertexUpdate()
    {
      if (_instance != null) {
        var mf = _instance.GetComponent<MeshFilter>();
        mf.mesh.vertices = _vertices;

        // Unity bug; must null before update.
        var mc = _instance.GetComponent<MeshCollider>();
        mc.sharedMesh = null;
        mc.sharedMesh = mf.mesh;
      }
    }
    
    /** Direct access to the vertex data; update gameobject use PushVertexUpdate() */
    public Vector3[] Vertices {
      get {
        if (_instance != null) {
          var mf = _instance.GetComponent<MeshFilter>();
          _vertices = mf.mesh.vertices;
          return _vertices;
        } else {
          return new Vector3[0];
        }
      }
      set {
        _vertices = value;
      }
    }
    
    /** Return typecast spirit */
    public T Spirit<T>() {
      return (T) _spirit;
    }

    /** Return an underlying game object component */
    public T Component<T>() where T : UnityEngine.Component
    {
      if (_instance != null) {
        var c = (T) _instance.GetComponent(typeof(T));
        return c;
      }
      return null;
    }

    /** Rotation */
    public float Rotation {
      get {
        return _rotation;
      }
      set {
        if (_instance != null) {
          _instance.transform.Rotate(new Vector3(0, 0, 1), value);
          _rotation = value;
        }
      }
    }

    /** Color of the base quad if no texture */
    public Color Color {
      set {
        if (_instance != null) {
          _instance.renderer.material.color = value;
        }
      }
    }

    /** If this prop is manifest on the game scene */
    public bool Visible { 
      get {
        return _instance != null;
      }
      set {
        if (value) {
          if (_instance == null) {
            _instance = _spirit.Manifest();
            _instance.AddComponent<nPropInput>();
            _instance.GetComponent<nPropInput>().Events = _events;
          }
        }
        else if (_instance != null) {
          GameObject.Destroy(_instance);
        }
      }
    }

    /** Listen for a click on this object; delegate must be unique */
    public void Listen(nCamera cam, nInputEvent type, nPropAction action) {
      var e = new nPropEvent() {
        Camera = cam,
        Event = type,
        Prop = this,
        Action = action
      };
      _events.Add(e);
    }
    
    /** Clear event handlers */
    public void ClearEventListeners(nCamera cam, nInputEvent type) {
      var items = (from p in _events where p.Event == type && p.Camera == cam select p).ToList();
      foreach (var i in items) {
        _events.Remove(i);
      }
    }
    
    /** Name of the attached game object */
    public string Name {
      get {
        if (_instance == null)
          return "";
        else
          return _instance.gameObject.name;
      }
    }
  }

  /** Prop event type */
  public class nPropEvent {
    public nCamera Camera { get; set; }
    public nInputEvent Event { get; set; }
    public nProp Prop { get; set; }
    public nPropAction Action { get; set; }
  }

  /** Prop click event type */
  public delegate void nPropAction(nPropEvent e);

  /** Handle events for the prop */
  public class nPropInput : MonoBehaviour {

    public IList<nPropEvent> Events { get; set; }

    public void Update()
    {
      if (Events != null) {
        foreach (var e in Events) {
          if (nInputHandler.Query(e.Camera, e.Event, this.gameObject))
            e.Action(e);
        }
      }
    }
  }
}

