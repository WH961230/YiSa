using GraphProcessor;

[System.Serializable, NodeMenuItem("Custom/SetCursorNode")]
public class SetCursorNode : BaseNode
{
	[Input(name = "In")]
    public float                input;

	[Output(name = "Out")]
	public float				output;

	public override string		name => "SetCursorNode";

	protected override void Process()
	{
	    
	}
}
