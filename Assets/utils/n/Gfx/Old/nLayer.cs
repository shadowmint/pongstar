using System;
using System.Collections.Generic;
using UnityEngine;

namespace n.Gfx.Old
{
  /** A collection of props that can be modified together */
  public class nLayer
  {
    private List<nProp> _props = new List<nProp>();

    /** Most recently added prop */
    private nProp _last = null;

    /** Position of this layer */
    private UnityEngine.Vector2 _position = new UnityEngine.Vector2(0f, 0f);

    public void Add(nProp p)
    {
      if (!_props.Contains(p)) {
        _props.Add(p);
        _last = p;
      }
    }
    
    /** Count of items in this layer */
    public int Count {
      get {
        return _props.Count;
      }
    }

    /** List of props */
    public IEnumerable<nProp> Props {
      get {
        return _props;
      }
    }

    /** Most recently added prop */
    public nProp Last {
      get {
        return _last;
      }
    }

    /** Controls visibility of the entire layer */
    public bool Visible {
      set {
        foreach (var p in _props) {
          p.Visible = value;
        }
      }
    }

    /** Modify the position of a visible layer */
    public UnityEngine.Vector2 Position { 
      set {
        if (_position != value) {
          _position = value;
          foreach (var p in _props) {
            p.Position = value;
          }
        }
      }
    }
  }
}

