using Godot;
using System;
using System.IO;

[GlobalClass]
public partial class ClientBase : Resource
{
    [Export] public string Name = "";
    [Export] public string Description = "";
    [Export] public string Code = "";
    public string PlayerCode = "";
    [Export] public Texture2D ClientTexture = null;
}
