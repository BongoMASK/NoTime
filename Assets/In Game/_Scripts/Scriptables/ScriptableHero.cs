using System;
using UnityEngine;

/// <summary>
/// Create a scriptable hero 
/// </summary>
[CreateAssetMenu(fileName = "New Scriptable Example")]
public class ScriptableHero : ScriptableUnitBase {
    public ExampleHeroType HeroType;
 
}
//example type
[Serializable]
public enum ExampleHeroType {
    StarLord = 0,
    Thor = 1
}

