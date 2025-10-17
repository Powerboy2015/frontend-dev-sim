using Godot;

public partial class KeyBindButton : Button
{
	private bool _isListening = false;
	private Key _currentKey = Key.None;
	public Key CurrentKey
	{
		get => _currentKey;
		set
		{
			_currentKey = value;
			Text = value.ToString();
		}
	}
	
	public override void _Ready()
	{
		Pressed += OnPressed;
	}
	
	private void OnPressed()
	{
		_isListening = true;
		Text = "Press any key...";
	}
	
	public override void _Input(InputEvent @event)
	{
		if (!_isListening) return;
		
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.IsEcho())
		{
			CurrentKey = keyEvent.Keycode;
			_isListening = false;
			GetViewport().SetInputAsHandled();
		}
	}
}
