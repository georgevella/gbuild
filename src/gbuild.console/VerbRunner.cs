namespace GBuild.Console
{
    public interface IVerbRunner
    {
        void Run(object options);
    }

    internal class VerbRunner<TVerbOptions> : IVerbRunner
    {
        private readonly IVerb<TVerbOptions> _verb;

        public VerbRunner(IVerb<TVerbOptions> verb)
        {
            _verb = verb;
        }

        public void Run(object options)
        {
            _verb.Run((TVerbOptions) options);
        }
    }
}