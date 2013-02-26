using UnityEngine;
using System.Collections;
using n.Test;
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using n.Core;

namespace n.Platform.Db.Impl
{
  [Serializable]
  public class DbTable<T> : nDbRecord {
    public long LastKey { get; set; }
    public List<T> Records { get; set; }
    protected override void Validate () {}
  }
}