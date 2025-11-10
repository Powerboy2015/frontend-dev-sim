// #if TOOLS
using Godot;
using System.IO;

[Tool]
public partial class TeleportRegistryBuilder : EditorScript
{
	public override void _Run()
	{
		GD.Print("=== Starting Teleport Registry Build ===");
		var registry = new TeleportRegistry();
		var dir = DirAccess.Open("res://");

		if (dir == null)
		{
			GD.PrintErr("Failed to open res:// directory!");
			return;
		}

		ScanDirectory(dir, "res://", registry);

		// Save the registry
		var error = ResourceSaver.Save(registry, "res://teleport_registry.tres");

		if (error == Error.Ok)
		{
			GD.Print($"Registry built successfully! Found {registry.TeleportPointIds.Count} teleport points.");
		}
		else
		{
			GD.PrintErr($"Failed to save registry: {error}");
		}
	}

	private void ScanDirectory(DirAccess dir, string path, TeleportRegistry registry)
	{
		GD.Print($"Scanning directory: {path}");
		dir.ListDirBegin();
		string fileName = dir.GetNext();

		while (fileName != "")
		{
			string fullPath = path + fileName;

			if (dir.CurrentIsDir())
			{
				if (fileName != "." && fileName != ".." && !fileName.StartsWith("."))
				{
					var subDir = DirAccess.Open(fullPath);
					if (subDir != null)
					{
						ScanDirectory(subDir, fullPath + "/", registry);
					}
				}
			}
			else if (fileName.EndsWith(".tscn"))
			{
				ScanScene(fullPath, registry);
			}

			fileName = dir.GetNext();
		}
		dir.ListDirEnd();
	}

	private void ScanScene(string scenePath, TeleportRegistry registry)
	{
		var scene = GD.Load<PackedScene>(scenePath);
		if (scene != null)
		{
			var instance = scene.Instantiate();
			ScanNodeForTeleportPoints(instance, scenePath, "", registry);
			instance.QueueFree();
		}
	}

	private void ScanNodeForTeleportPoints(Node node, string scenePath, string parentPath, TeleportRegistry registry)
	{
		string nodePath = parentPath + node.Name;

		// Check both actual TeleportPoint2D type AND Node2D with the script
		bool isTeleportPoint = node is TeleportPoint2D;
		bool hasScript = node.GetScript().As<CSharpScript>()?.ResourcePath?.Contains("TeleportPoint2D.cs") ?? false;

		if (isTeleportPoint || hasScript)
		{
			var teleportId = node.Get("TeleportId").AsString();
			var displayName = node.Get("Displayname").AsString();

			if (!string.IsNullOrEmpty(teleportId))
			{
				registry.AddTeleportPoint(teleportId, displayName, scenePath, nodePath);
				GD.Print($"✓ Registered: {teleportId} in {scenePath}");
			}
		}

		foreach (Node child in node.GetChildren())
		{
			ScanNodeForTeleportPoints(child, scenePath, nodePath + "/", registry);
		}
	}
}
// #endif
