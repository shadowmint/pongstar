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
  /** A flat 3D text mesh */
  public class nText : nQuad
  {
    public Font Font { get; set; }

    public float FontSize { get; set; }

    public string Text { get; set; }

    public Color Color { get; set; }

    /** GameObject instances created will be named Name__[RandomNumber] */
    public nText (string name, UnityEngine.Vector2 size) : base(name, size) {
      Color = Color.white;
    }

    /** GameObject instances created will be named Text__[RandomNumber] */
    public nText(UnityEngine.Vector2 size) : base(size) {
      _name = "Text";
      Color = Color.white;
    }

    /** Create a GameObject for this quad and add it to the scene */
    public override GameObject Manifest ()
    {
      /* Create object and add to scene */
      var rtn = (GameObject)new GameObject (
        Name (),
        typeof(MeshRenderer), // Required to render
        typeof(MeshFilter), // Required to render
        typeof(TextMesh),     // For text
        typeof(MeshCollider)  // Required to catch input events
      );

      var tm = rtn.GetComponent<TextMesh> ();
      var mr = rtn.GetComponent<MeshRenderer> ();
      var mc = rtn.GetComponent<MeshCollider> ();
      var mf = rtn.GetComponent<MeshFilter> ();

      /* collision box */
      mc.sharedMesh = _mesh;
      mf.mesh = _mesh;
      mf.renderer.material.color = Color.white;

      /* font texture */
      if (Font != null) {
        tm.font = Font;
        mr.material = Font.material;
      }
      mr.material.color = Color;

      /* this seems to mysteriously control the font quality. O_o */
      tm.fontSize = (int) (100 * FontSize);

      /* font size */
      tm.characterSize = FontSize * 10.0f / tm.fontSize;
      tm.text = Text;
      tm.lineSpacing = tm.lineSpacing * 0.85f;

      /* Set position */
      rtn.transform.position = new Vector3(0, 0, 0);

      return rtn;
    }
  }
}

