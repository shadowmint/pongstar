using UnityEngine;
using System.Collections;
using n.Test;
using System.Linq;
using System;
using System.Collections.Generic;
using n.Platform.Db;
using n.Core;
using n.Gfx;

namespace Tests
{
  public class nBoxTests : nTestBase 
  {
    [nTest]
    public void test_can_create_instance()
    {
      var instance = new nBox();
      instance.ShouldNotBe(null);
    }
    
    [nTest]
    public void test_can_set_size_from_corners()
    {
      var instance = new nBox();
      instance.SetCorners(-10f, -10f, 10f, 10f);
    }
    
    [nTest]
    public void test_can_set_size_from_center()
    {
      var instance = new nBox();
      instance.SetSize(0f, 0f, 10f, 10f);
    }
    
    [nTest]
    public void test_can_set_size_from_parent()
    {
      var instance = new nBox();
      
      var quad = new nQuad();
      quad.Data.Points.Set(new float[8] { 1, 1, 1, 0, 0, 0, 0, 1 });
      
      instance.SetParent(quad.Data);
    }
  }
}
