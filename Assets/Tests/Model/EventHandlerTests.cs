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
using n.Test;
using System;
using Ps.Model;
using System.Collections.Generic;

namespace Ps.Tests.Model
{
  /** Dummy event ids */
  public class EventHandlerTestEventIds 
  {
    public static int EVENT = 0x01;
    public static int EVENT2 = 0x02;
  }

  /** Dummy event type */
  public class EventHandlerTestsEvent : IEventData 
  {
    public long Data { get; set; }

    public GameState State { get { return null; } }

    public int Id {
      get {
        return EventHandlerTestEventIds.EVENT;
      }
    }

    public IEnumerable<EventBinding> Handlers { get; set; }
  }

  /** Dummy event type 2 */
  public class EventHandlerTestsEvent2 : IEventData 
  {
    public string Data { get; set; }

    public GameState State { get { return null; } }

    public int Id {
      get {
        return EventHandlerTestEventIds.EVENT2;
      }
    }
    
    public IEnumerable<EventBinding> Handlers { get; set; }
  }

  public class EventHandlerTests : nTestBase
  {
    public static string Value = "";
    public static long LValue = 0;

    public Ps.Model.EventHandler setup() {
      return new Ps.Model.EventHandler();
    }
    
    [nTest]
    public void test_can_create_instance() {
      var instance = setup();
      instance.ShouldNotBe(null);
    }
    
    [nTest]
    public void test_can_trigger_events() {
      var instance = setup();

      instance.Listen(EventHandlerTestEventIds.EVENT, test_delegate_event);
      instance.Listen(EventHandlerTestEventIds.EVENT2, test_delegate_event2, true);

      instance.Trigger(new EventHandlerTestsEvent() { Data = 100 });
      instance.Trigger(new EventHandlerTestsEvent() { Data = 1000 });
      instance.Trigger(new EventHandlerTestsEvent() { Data = 10 });
      instance.Trigger(new EventHandlerTestsEvent() { Data = 1 });
      instance.Trigger(new EventHandlerTestsEvent2() { Data = "hello" });
      instance.Trigger(new EventHandlerTestsEvent2() { Data = "hello" });

      Value.ShouldBe("");
      LValue.ShouldBe(0);

      instance.Dispatch();

      Value.ShouldBe("hellohello");
      LValue.ShouldBe(1111);

      instance.Trigger(new EventHandlerTestsEvent() { Data = 100 });
      instance.Trigger(new EventHandlerTestsEvent() { Data = 1000 });
      instance.Trigger(new EventHandlerTestsEvent() { Data = 10 });
      instance.Trigger(new EventHandlerTestsEvent() { Data = 1 });
      instance.Trigger(new EventHandlerTestsEvent2() { Data = "HELLO" });
      instance.Trigger(new EventHandlerTestsEvent2() { Data = "HELLO" });
      instance.Dispatch();

      Value.ShouldBe("hellohello");
      LValue.ShouldBe(2222);
    }

    /** Test delegates */
    public void test_delegate_event(IEventData raw) {
      var data = (EventHandlerTestsEvent) raw;
      LValue += data.Data;
    }

    /** Test delegates */
    public void test_delegate_event2(IEventData raw) {
      var data = (EventHandlerTestsEvent2) raw;
      Value += data.Data;
    }
  }
}