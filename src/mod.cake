#load "config/mod.cake"
#load "tasks/mod.cake"
#load "data.cake"
#load "parameters.cake"
#load "setup.cake"

private SpectreBuild _engine;
public SpectreBuild Spectre
{
    get
    {
        if(_engine == null)
        {
            _engine = new SpectreBuild(Context ,target => RunTarget(target));
        }
        return _engine;
    }
}

public class SpectreBuild
{
    private ICakeContext _context;
    private Action<string> _action;
    
    public BuildParameters Parameters { get; }

    public SpectreBuild(ICakeContext context, Action<string> action)
    {
        _context = context;
        _action = action;

        Parameters = new BuildParameters();
    }

    public void Build()
    {
        _action(_context.Argument("target", "Default"));
    }
}
