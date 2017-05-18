using System;
using System.Collections.Generic;

public class EventController
{
    public delegate void unitDie(int scores);
    public delegate void dmgUp(int scores);
    public delegate void playerHealth(int health);
    public delegate void setlevelProgress(float percent);
}