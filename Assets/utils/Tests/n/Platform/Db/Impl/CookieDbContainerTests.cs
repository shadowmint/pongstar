using UnityEngine;
using System.Collections;
using n.Test;
using System.Linq;
using System;
using System.Collections.Generic;
using n.Platform.Db;
using n.Platform.Db.Impl;
using n;

namespace Tests
{
  public class CookieDbContainerTests : nTestBase
  {
    #if UNITY_WEBPLAYER
    private CookieDbContainer setup ()
    {
      return new CookieDbContainer();
    }
    
    [nTest]
    public void test_can_create_instance ()
    {
      var instance = setup ();
      instance.ShouldNotBe (null);
    }

    /*[nTest]
    public void test_can_insert_value_and_read_it ()
    {
      var instance = setup ();
      instance.Set ("example.key", "example.value");
      instance.Set ("example.2.key", "example.2.value");

      var value1 = instance.Get("example.key");
      value1.ShouldNotBe(null);
      value1.ShouldBe("example.value");

      var value2 = instance.Get("example.2.key");
      value2.ShouldNotBe(null);
      value2.ShouldBe("example.2.value");
    }

    [nTest]
    public void test_can_check_value_exists ()
    {
      var instance = setup ();
      instance.Set ("example.key", "example.value");
      instance.Set ("example.2.key", "example.2.value");
      instance.Set ("example.3.key", "example.3.value");
      instance.Clear ("example.3.key");
      instance.Clear ("example.4.key");

      instance.Exists("example.key").ShouldBe(true);
      instance.Exists("example.2.key").ShouldBe(true);
      instance.Exists("example.3.key").ShouldBe(false);
      instance.Exists("example.4.key").ShouldBe(false);
    }*/
    #endif
  }
}
