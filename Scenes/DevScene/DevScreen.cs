using Godot;
using System;

public partial class DevScreen : Control
{
	private TextEdit preview;
	private TextEdit Input_Field;

	private string[] code_Parts;
	private string[] Player_code_Parts;

	int Current_Step = 0;
	[Export]
	NodePath CloseButtonPath;

	public override void _Ready()
	{
		// get both text fields
		preview = GetNode<TextEdit>("Panel/CodeSpace/preview");
		Input_Field = GetNode<TextEdit>("Panel/CodeSpace/Input_Field");

		//close button

		var CloseButton = GetNode<Button>(CloseButtonPath);
		CloseButton.Pressed += OnCloseWindowPressed;

		// get the challanges
		var Challenge = ClientSettings.ChosenClient.CodingChallenges[0];

		// set phantom code
		var Full_Code = Challenge.Code;

		Full_Code = Full_Code.Replace("""\t""", "\t");
		Full_Code = Full_Code.Replace("""\n""", "\n");

		code_Parts = Full_Code.Split("\n");

		// set player code
		var Full_Player_Code = Challenge.PlayerCode;
		Player_code_Parts = Full_Player_Code.Split("\n");


		// // set the first part of the phantom text
		preview.Text = code_Parts[0];
	}

	public override void _Process(double delta)
	{
		// add a new line after checking if preview and the player written code is the same

		if (Input_Field.Text == preview.Text)
		{
			if (code_Parts.Length > Current_Step)
			{
				Current_Step += 1;
				preview.Text += "\n" + code_Parts[Current_Step];

				Wallet.Instance.AddCoins(10);
			}
		}
	}

	private void OnLivePreviewPressed()
	{
		GD.Print("switch to live preview");
	}

	private void OnCloseWindowPressed()
	{
		ClientSettings.ChosenClient.CodingChallenges[0].PlayerCode = Input_Field.Text;

		var ClientScreen = GD.Load<PackedScene>("res://Scenes/ClientScene/ClientScreen.tscn");

		var Container = GetParent();
		var Instance = ClientScreen.Instantiate();

		Container.AddChild(Instance);
		QueueFree();
	}
}
