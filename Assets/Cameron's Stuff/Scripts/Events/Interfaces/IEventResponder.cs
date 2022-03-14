using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEventResponder
{
    public Dictionary<string, Action> eventDictionary { get; set; }

    public List<IEventResponse> responses { get; set; }
}
