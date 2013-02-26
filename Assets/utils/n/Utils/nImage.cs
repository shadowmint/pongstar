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
using n.Utils.Geom;
using System.IO;
using n.Core.External;

namespace n.Utils
{
  /** 
   * Sometimes you need to load an image as an image, not a texture. 
   * <p>
   * Note that you must import the asset as a 'TextAsset' of type .bytes
   * to be able to import it like this, but it must *actually* be a .tga
   * file.
   * <p>
   * ONLY .tga files are supported, via the Tga Reader project by
   * David Polomis.
   */
  public class nImage 
  {
    private TargaImage _image;

    public void Load(string path) {
      TextAsset asset = Resources.Load(path) as TextAsset;
      var s = new MemoryStream(asset.bytes);
      var br = new BinaryReader(s);
      _image = new TargaImage();
      _image.LoadTGAImage(br);
    }

    private Color32[] Section (int x, int y, int width, int height)
    {
      Color32[] rtn = null;
      if ((x >= 0) && (x < Width) && (y >= 0) && (y < Height)) {
        if ((width > 0) && ((width + x) <= Width) && (height > 0) && ((height + y <= Height))) {
          rtn = new Color32[width * height];
          var bytes = _image.Header.BytesPerPixel;
          var format = _image.GetPixelFormat();
          if (format != PixelFormat.Format32bppArgb) {
            nLog.Debug("Invalid TGA image; only Format32bppArgb is supported");
          }
          else {
            var roffset = 0;
            for (var yi = (y + height - 1); yi >= y; --yi) {
              for (var xi = x; xi < (x + width); ++xi) {
                var offset = (yi * Width + xi) * bytes;
                rtn[roffset].a = _image.Raw[offset + 3];
                rtn[roffset].r = _image.Raw[offset + 2];
                rtn[roffset].g = _image.Raw[offset + 1];
                rtn[roffset].b = _image.Raw[offset + 0];
                ++roffset;
              }
            }
          }
        }
      }
      return rtn;
    }

    /** Return the average color of a given region */
    public Color32 RegionSample(int x, int y, int width, int height)
    {
      Color32 rtn = new Color32();
      int count = 0;
      int a = 0;
      int r = 0;
      int g = 0;
      int b = 0;
      if ((x >= 0) && (x < Width) && (y >= 0) && (y < Height)) {
        if ((width > 0) && ((width + x) <= Width) && (height > 0) && ((height + y <= Height))) {
          var bytes = _image.Header.BytesPerPixel;
          var format = _image.GetPixelFormat();
          if (format != PixelFormat.Format32bppArgb) {
            nLog.Debug("Invalid TGA image; only Format32bppArgb is supported");
          }
          else {
            var roffset = 0;
            for (var yi = (y + height - 1); yi >= y; --yi) {
              for (var xi = x; xi < (x + width); ++xi) {
                var offset = (yi * Width + xi) * bytes;
                a = _image.Raw[offset + 3];
                r = _image.Raw[offset + 2];
                g = _image.Raw[offset + 1];
                b = _image.Raw[offset + 0];
                roffset += 4;
                ++count;
              }
            }
            if (count > 0) {
              rtn.r = (byte) (r / count);
              rtn.g = (byte) (g / count);
              rtn.b = (byte) (b / count);
              rtn.a = (byte) (a / count);
            }
          }
        }
      }
      return rtn;
    }

    /** Return a zone in RGBA format */
    public byte[] Region(int x, int y, int width, int height)
    {
      byte[] rtn = null;
      if ((x >= 0) && (x < Width) && (y >= 0) && (y < Height)) {
        if ((width > 0) && ((width + x) <= Width) && (height > 0) && ((height + y <= Height))) {
          rtn = new byte[width * height * 4];
          var bytes = _image.Header.BytesPerPixel;
          var format = _image.GetPixelFormat();
          if (format != PixelFormat.Format32bppArgb) {
            nLog.Debug("Invalid TGA image; only Format32bppArgb is supported");
          }
          else {
            var roffset = 0;
            for (var yi = (y + height - 1); yi >= y; --yi) {
              for (var xi = x; xi < (x + width); ++xi) {
                var offset = (yi * Width + xi) * bytes;
                rtn[roffset + 3] = _image.Raw[offset + 3];
                rtn[roffset + 0] = _image.Raw[offset + 2];
                rtn[roffset + 1] = _image.Raw[offset + 1];
                rtn[roffset + 2] = _image.Raw[offset + 0];
                roffset += 4;
              }
            }
          }
        }
      }
      return rtn;
    }

    public Texture2D Texture ()
    {
      return Texture(0, 0, Width, Height);
    }

    public Texture2D Texture (int x, int y, int width, int height)
    {
      var bytes = Section (x, y, width, height);
      if (bytes == null)
        return null;
      var texture = new Texture2D(width, height, TextureFormat.ARGB32, true, true);
      texture.SetPixels32(bytes, 0);
      texture.Apply();
      return texture;
    }

    public int Width {
      get {
        return _image.Header.Width;
      }
    }

    public int Height {
      get {
        return _image.Header.Height;
      }
    }
  }
}
