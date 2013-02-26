using UnityEngine;
using System.Collections;
using n.Test;
using System.Linq;
using System;
using System.Collections.Generic;
using n.Platform.Db;
using n.Core;
using n.Gfx;
using n;

namespace Tests
{
  public class nQuadTests : nTestBase 
  {
    [nTest]
    public void test_can_create_instance()
    {
      var instance = new n.Utils.Geom.nGQuad();
      instance.ShouldNotBe(null);
    }

    [nTest]
    public void test_can_detect_intersection()
    {
      var i1 = new n.Utils.Geom.nGQuad(5f);
      var i2 = new n.Utils.Geom.nGQuad(5f);
      i1.Intersects(i2).ShouldBe(true);
    }

    [nTest]
    public void test_can_detect_intersection_with_offset()
    {
      var i1 = new n.Utils.Geom.nGQuad(5f);
      var i2 = new n.Utils.Geom.nGQuad(5f).Offset(4f, 4f);
      i1.Intersects(i2).ShouldBe(true);

      i1 = new n.Utils.Geom.nGQuad(5f);
      i2 = new n.Utils.Geom.nGQuad(5f).Offset(10f, 10f);
      i1.Intersects(i2).ShouldBe(false);

      i1 = new n.Utils.Geom.nGQuad(5f);
      i2 = new n.Utils.Geom.nGQuad(5f).Offset(10f, 1f);
      i1.Intersects(i2).ShouldBe(false);

      i1 = new n.Utils.Geom.nGQuad(5f);
      i2 = new n.Utils.Geom.nGQuad(5f).Offset(1f, 10f);
      i1.Intersects(i2).ShouldBe(false);

      i1 = new n.Utils.Geom.nGQuad(3f);
      i2 = new n.Utils.Geom.nGQuad(5f).Offset(2.8f, 1f);
      i1.Intersects(i2).ShouldBe(true);
    }
  }
}
