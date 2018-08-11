using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager {

  private static AudioSource source;
  private static AudioClip BreakChest;
  private static AudioClip PickUp;
  private static AudioClip CutFlower;
  private static AudioClip AddRibbon;
  private static AudioClip WalkingStone;
  private static AudioClip WalkingGrass;
  private static AudioClip AddBulb;
  private static AudioClip OpenDoor;

  // Loads all the sounds from the files
  public static void SetUp () {
    source = GameObject.Find("Manager").GetComponent<AudioSource>();
    BreakChest = Resources.Load("Sound Effects/BreakingWood", typeof(AudioClip)) as AudioClip;
    PickUp = Resources.Load("Sound Effects/PickUp", typeof(AudioClip)) as AudioClip;
    CutFlower = Resources.Load("Sound Effects/Scissor", typeof(AudioClip)) as AudioClip;
    AddRibbon = Resources.Load("Sound Effects/Ribbon", typeof(AudioClip)) as AudioClip;
    WalkingStone = Resources.Load("Sound Effects/Walking", typeof(AudioClip)) as AudioClip;
    WalkingGrass = Resources.Load("Sound Effects/WalkingGrass", typeof(AudioClip)) as AudioClip;
    AddBulb = Resources.Load("Sound Effects/Bulb", typeof(AudioClip)) as AudioClip;
    OpenDoor = Resources.Load("Sound Effects/OpenDoor", typeof(AudioClip)) as AudioClip;
  }

  // Plays the sound according to the action performed
  public static void PlaySound(string action)
  {
    AudioClip sound = null;
    source.time = 1f;
    switch (action)
    {
      case "chest":
        source.loop = false;
        sound = BreakChest;
        break;
      case "pick":
        source.loop = false;
        sound = PickUp;
        break;
      case "cut":
        source.loop = false;
        sound = CutFlower;
        break;
      case "ribbon":
        source.loop = false;
        sound = AddRibbon;
        break;
      case "walking_default":
        source.time = 2f;
        source.loop = true;
        sound = WalkingStone;
        break;
      case "walking_grass":
        source.loop = true;
        sound = WalkingGrass;
        break;
      case "bulb":
        source.loop = false;
        sound = AddBulb;
        break;
      case "open":
        source.loop = false;
        sound = OpenDoor;
        break;
    }
    if (sound != null)
      source.clip = sound;
      source.Play();
  }

  // Stops current audio
  public static void StopAudio()
  {
    source.Stop();
  }

}
