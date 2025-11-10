using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ClientBase : Resource
{
	[Export] public string Name = "";
	[Export] public string Description = "";
	[Export] public Array<CodingBase> CodingChallenges;
	[Export] public Texture2D ClientTexture = null;


}
