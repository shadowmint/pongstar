using UnityEngine;
using System.Collections;
using n.Test;
using System;
using n.Core;

namespace Tests
{
  public class MyRecordType : nDbRecord {
  
    public int Value1 { get; set; }
    public long Value2 { get; set; }
    public bool Value3 { get; set; }
    public double Value4 { get; set; }
    public string Value5 { get; set; }
    public String Value6 { get; set; }
    public DateTime Value7 { get; set; }
    public MyRecordType Ref { get; set; }
    
    protected override void Validate ()
    {
      if (Value2 < 2) Errors.Add("Value2", "Cannot have value < 2");
      if (Value6 != "Value6") Errors.Add("Value6", "Must have a value of 'Value6'");
    }
  }
}