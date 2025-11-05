using Godot;
using System;
using System.Collections.Generic;

public partial class TeleportManager : Node
{
    private List<TeleportPoint2D> teleportPoints = new List<TeleportPoint2D>();
    public void RegisterTeleportPoint(TeleportPoint2D point)
    {
        if (!teleportPoints.Contains(point))
        {
            teleportPoints.Add(point);
        }
    }
}