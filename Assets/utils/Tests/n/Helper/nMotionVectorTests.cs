using UnityEngine;
using System.Collections;
using n.Test;
using System.Linq;
using System;
using System.Collections.Generic;
using n.Platform.Db;
using n.Core;
using n.Gfx;
using n.Utils;
using n.Utils.Geom;

namespace Tests
{
  public class nMotionVectorTests : nTestBase 
  {
    [nTest]
    public void test_can_create_instance()
    {
      var instance = new nMotionVector();
      instance.ShouldNotBe(null);
    }

    [nTest]
    public void test_can_keep_to_required_length() {
      var instance = new nMotionVector();
      
      instance.MaxLength = 10f;
      instance.Update(0, 0);
      instance.Update(2, 0);
      instance.Update(4, 0);
      instance.Update(6, 0);
      instance.Update(8, 0);
      instance.Update(10, 0);
      instance.Length.ShouldBe(10);

      instance.Update(12, 0);
      instance.Length.ShouldBe(10);
    }

    [nTest]
    public void test_can_generate_points()
    {
      var instance = new nMotionVector();

      instance.SegmentSize = 0.5f;
      instance.MaxLength = 10f;

      instance.Update(0, 0);
      instance.Update(2, 0);
      instance.Update(4, 0);
      instance.Update(6, 0);
      instance.Update(8, 0);
      instance.Update(10, 0);

      nGLine[] points;
      points = instance.Points();

      points.Length.ShouldBe(20);
    }

    [nTest]
    public void test_can_generate_points_from_small_increments() {
      var instance = new nMotionVector();
      
      instance.SegmentSize = 0.5f;
      instance.MaxLength = 10f;

      for (var i = 0.0f; i < 10.1f; i += 0.01f) {
        instance.Update(0, i);
      }

      nGLine[] points;
      points = instance.Points();
      
      // +1 for the last segment
      points.Length.ShouldBe(21);
    }
  }
}
