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
using n.Gfx.Impl;
using n.Core;

namespace n.Gfx
{
  /** 
   * A 2D layer which is drawn to in unity 
   * <p>
   * You'll notice this only provides a limited subset of the functionality
   * of the unity camera; that's deliberate.
   */
  public class nCamera
  {
    /** Camera instance */
    private UnityEngine.Camera _cam;

    /** The graphics pipe for this camera */
    private nGraphicsPipe _pipe;

    /** 
     * Creates a new orthographic camera
     * <p>
     * Notice that you must 'enable' this camera by setting
     * 'Active' to true, and 'Audio' to true if you want it to
     * be the primary audio listener.
     * <p>
     * The camera is placed at (0, 0, -1) looking down the Z-axis.
     * <p>
     * There MUST be a camera on the scene already to be able create 
     * this type of camera, and it inherits all the properties of the
     * main camera; although some of them are reset.
     */
    public nCamera(double height) {
      var original = GameObject.FindWithTag("MainCamera");
      _cam = (Camera) UnityEngine.Camera.Instantiate(
        original.camera,
        new Vector3(0, 0, -1),
        Quaternion.FromToRotation(
          new Vector3(0, 0, 0),
          new Vector3(0, 0, 1)
        )
      );
      _cam.orthographicSize = (float) height / 2;
      _cam.orthographic = true;
      _cam.backgroundColor = Color.black;
      _cam.depth = 0;
      _cam.enabled = false;
      _cam.GetComponent<AudioListener>().enabled = false;

      _pipe = new nGraphicsPipe();
      _cam.gameObject.AddComponent<nRenderer>();
      _cam.gameObject.GetComponent<nRenderer>().Pipe = _pipe;

      original.camera.enabled = false;
    }

    /** Reuturn x coordinate */
    public float X {
      get {
        return _cam.transform.position[0];
      }
    }

    /** Return y coordinate */
    public float Y {
      get {
        return _cam.transform.position[1];
      }
    }

    /** Direct access to the camera for rendering, etc. */
    public UnityEngine.Camera Native {
      get {
        return _cam;
      }
    }
    
    /** Move the camera to a different 2D location */
    public void Move(double x, double y) {
      _cam.transform.position = new Vector3((float) x, (float) y, -1.0f);
    }

    /** Background color for to the camera */
    public Color Background { 
      get { return _cam.backgroundColor; }
      set { _cam.backgroundColor = value; }
    }

    /** Convert mouse coordinates to camera coordinates */
    /*public Ray ScreenPointToRay(Vector3 point) {
      var ray = _cam.ScreenPointToRay(point);
      return ray;
    }*/

    /** Covert UI raw pointer coords to world coordinates */
    public float[] ScreenPointToWorld(float[] pos) {
      var result = Native.ScreenToWorldPoint(new Vector3(pos[0], pos[1], 0f));
      return new float[2] { result[0], -result[1] };
    }

    /** Covert UI raw pointer coords to world coordinates */
    public float[] ScreenPointToWorld(UnityEngine.Vector2 pos) {
      var result = Native.ScreenToWorldPoint(new Vector3(pos[0], pos[1], 0f));
      return new float[2] { result[0], -result[1] };
    }

    /** Camera render order */
    public int Depth {
      get { return (int) Math.Floor(_cam.depth); }
      set { _cam.depth = value; }
    }

    /** Access to the graphics pipe */
    public nGraphicsPipe Pipe {
      get {
        return _pipe;
      }
    }

    /** Enable to audio listener for this camera */
    public bool Audio {
      get {
        return _cam.GetComponent<AudioListener>().enabled;
      }
      set {
        _cam.GetComponent<AudioListener>().enabled = value;
      }
    }

    /** Enable this camera */
    public bool Active {
      get { return _cam.enabled; }
      set { 
        _cam.enabled = value; 
        _cam.gameObject.GetComponent<nRenderer>().Enabled = value;
      }
    }
    
    /** The bounds of this game in world space */
    public Rect ScreenBounds {
      get {
        var size = _cam.pixelRect;
        var origin = _cam.ScreenToWorldPoint(new Vector3(size.xMin, size.yMin, _cam.nearClipPlane));
        var end = _cam.ScreenToWorldPoint(new Vector3(size.xMax, size.yMax, _cam.nearClipPlane));
        var rtn = new Rect(origin[0], origin[1], end[0] - origin[0], end[1] - origin[1]);
        return rtn;
      }
    }

    /** Pixel size bounds, for the UI */
    public Rect PixelBounds {
      get {
        return _cam.pixelRect;
      }
    }
  }
}

