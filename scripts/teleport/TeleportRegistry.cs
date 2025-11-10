using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class TeleportRegistry : Resource
{
    [Export]
    public Godot.Collections.Dictionary<string, string> TeleportPointIds { get; set; } = new();

    [Export]
    public Godot.Collections.Dictionary<string, string> TeleportPointNames { get; set; } = new();

    [Export]
    public Godot.Collections.Dictionary<string, string> TeleportPointScenes { get; set; } = new();

    [Export]
    public Godot.Collections.Dictionary<string, string> TeleportPointPaths { get; set; } = new();

    public void AddTeleportPoint(string id, string displayName, string scenePath, string nodePath)
    {
        TeleportPointIds[id] = id;
        TeleportPointNames[id] = displayName;
        TeleportPointScenes[id] = scenePath;
        TeleportPointPaths[id] = nodePath;
    }

    public string[] GetAllTeleportIds()
    {
        var ids = new List<string>();
        foreach (var key in TeleportPointIds.Keys)
        {
            ids.Add(key);
        }
        return ids.ToArray();
    }

    public TeleportPointData GetTeleportPointData(string id)
    {
        if (TeleportPointIds.ContainsKey(id))
        {
            return new TeleportPointData
            {
                Id = id,
                DisplayName = TeleportPointNames[id],
                ScenePath = TeleportPointScenes[id],
                NodePath = TeleportPointPaths[id]
            };
        }
        return null;
    }
}

// Keep this simple - don't save it as a Resource
public class TeleportPointData
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string ScenePath { get; set; }
    public string NodePath { get; set; }
}