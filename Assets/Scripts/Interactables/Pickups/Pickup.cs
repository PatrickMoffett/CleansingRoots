using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : MonoBehaviour
{
    public enum Category {Health,Ammo}

    public Category category;
    public int modifier;
    public AudioClip clipToPlayOnPickup;
}
