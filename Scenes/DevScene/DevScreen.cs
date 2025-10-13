using Godot;
using System;

public partial class DevScreen : Control
{
	private TextEdit preview;
	private TextEdit Input_Field;

	private string[] code_Parts;
	int Current_Step = 0;

	public override void _Ready()
	{
		// get both text fields
		preview = GetNode<TextEdit>("Panel/CodeSpace/preview");
		Input_Field = GetNode<TextEdit>("Panel/CodeSpace/Input_Field");

		// get the html code
		using var file = FileAccess.Open("res://Codes/html_1.txt", FileAccess.ModeFlags.Read);
		var Full_Code = file.GetAsText();
		code_Parts = Full_Code.Split("\n");

		// set the first part of the phantom text
		preview.Text = code_Parts[0];
	}

	public override void _Process(double delta)
	{
		// add a new line after checking if preview and the player written code is the same
		if (Input_Field.Text == preview.Text)
		{
			Current_Step += 1;
			preview.Text += "\n" + code_Parts[Current_Step];
		}
	}

	private void OnLivePreviewPressed()
	{
		GD.Print("switch to live preview");
	}
	
	private void OnCloseWindowPressed()
	{
		QueueFree();
	}
}
