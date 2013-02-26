using System;
using UnityEngine;
using System.Collections.Generic;
using n.Core;

namespace n.Gfx.Old
{
  /** Event types */
  public enum nInputEvent {
    ENTER,
    EXIT,
    UP,
    DOWN
  }

  /** Handles input events */
  public class nInputHandler
  {
    /** Static instance */
    private static nInputHandler _instance = null;

    /** Access to the static instance */
    private static nInputHandler Get ()
    {
      if (_instance == null) {
        var r = new nResolver();
        _instance = r.Resolve<nInputHandler>();
      }
      return _instance;
    }

    /** Set of hits from last frame */
    private IDictionary<nCamera, RaycastHit[]> _hits = new Dictionary<nCamera, RaycastHit[]>();

    /** Set of hits this frame */
    private IDictionary<nCamera, RaycastHit[]> _prevHits = new Dictionary<nCamera, RaycastHit[]>();

    /** The last frame we saw */
    private int lastFrame = 0;

    /** Update for camera if we haven't already */
    public void UpdateHits(nCamera c)
    {
      var thisFrame = Time.frameCount;
      if (lastFrame != thisFrame) {
        _prevHits = _hits;
        _hits = new Dictionary<nCamera, RaycastHit[]>();
        lastFrame = thisFrame;
      }
      if (c != null) {
        if (!_hits.ContainsKey(c)) {
          //var mouse = Input.mousePosition;
          //var ray = c.ScreenPointToRay(mouse);
          //RaycastHit[] hits;
          //hits = Physics.RaycastAll(ray);
          //_hits [c] = hits;           
        }
      }
    }

    /** Check if there is a match in the given set for the game object */
    public bool Intersects (RaycastHit[] hits, GameObject o) {
      var rtn = false;
      foreach (var h in hits) {
        if ((h.collider != null) && (h.collider.gameObject == o)) {
          rtn = true;
          break;
        }
      }
      return rtn;
    }

    /** Check if we match a specific event for a specific gameobject */
    public bool QueryHits (nCamera c, nInputEvent e, GameObject o)
    {
      var rtn = false;
      var hits = _hits[c];
      if (e == nInputEvent.DOWN) {
        if (Input.GetMouseButtonDown(0) && Intersects(hits, o))
          rtn = true;
      } 
      else if (e == nInputEvent.UP) {
        if (Input.GetMouseButtonUp(0) && Intersects(hits, o))
          rtn = true;
      } 
      else if (e == nInputEvent.ENTER) {
        if (_prevHits.ContainsKey(c)) {
          var lhits = _prevHits[c];
          if (Intersects(hits, o) && (!Intersects(lhits, o))) {
              rtn = true;
          }
        }
      } 
      else if (e == nInputEvent.EXIT) {
        if (_prevHits.ContainsKey(c)) {
          var lhits = _prevHits[c];
          if (!Intersects(hits, o) && (Intersects(lhits, o)))
            rtn = true;
        }
      }
      return rtn;
    }

    /** Check if we match a specific event for a specific gameobject */
    public static bool Query (nCamera c, nInputEvent e, GameObject o) {
      var i = nInputHandler.Get();
      i.UpdateHits (c);
      return i.QueryHits(c, e, o);
    }
  }
}

