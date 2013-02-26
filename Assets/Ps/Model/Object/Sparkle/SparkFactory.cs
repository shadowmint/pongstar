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
using System.Linq;
using n.Core;
using System.Collections.Generic;
using n.Gfx;
using System;

namespace Ps.Model.Object
{
  public class SparkFactory 
  {
    public float[] Source { get; set; }
    public float[] Velocity { get; set; }
    public float[] VelocityVar { get; set; }
    public float Lifespan { get; set; }
    public float LifespanVar { get; set; }
    public float ScaleChange { get; set; }
    public float ScaleChangeVar { get; set; }
    public float[] Tint { get; set; }
    public int Count { get; set; }
    public int CountVar { get; set; }

    public SparkFactory() {
      Velocity = new float[3];
      VelocityVar = new float[3];
      Tint = new float[4];
      Source = new float[2];
    }

    public List<Spark> Manufacture() {
      var rtn = new List<Spark>();
      var actual_count = nRand.Int(Count, CountVar);
      for (var i = 0; i < actual_count; ++i) {
        var sc = nRand.Float(0.4f, 0.2f);
        var s = new Spark() {
          Lived = 0,
          Lifespan = nRand.Float(Lifespan, LifespanVar),
          Velocity = new float[3] { 
            nRand.Float(Velocity[0], VelocityVar[0]),
            nRand.Float(Velocity[1], VelocityVar[1]),
            nRand.Float(Velocity[2], VelocityVar[2])
          },
          Rotation = 0f,
          Tint = new float[4] {
            Tint[0] * nRand.Float(0.9f, 0f, 0.7f),
            Tint[1] * nRand.Float(0.9f, 0f, 0.7f),
            Tint[2] * nRand.Float(0.9f, 0f, 0.7f),
            Tint[3] * 1.0f
          },
          Scale = new float[2] {
            1, 1
          },
          ScaleChange = new float[2] {
            sc, sc
          },
          Position = new float[2] {
            Source[0], Source[1]
          }
        };

        rtn.Add(s);
      }
      return rtn;
    }
  }
  
}
