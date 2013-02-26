using System;
using UnityEngine;

namespace n.Gfx.Old
{
  public interface nSpirit
  {
    /** Manifest on the scene */
    GameObject Manifest();

    /** Set the texture if this spirit supports it */
    Texture Texture { set; }
  }
}

