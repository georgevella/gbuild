using System;

namespace GBuild.Core.VcsSupport
{
    public class Revision
    {
        public Revision(string message, string id, string committer, DateTimeOffset when)
        {
            Message = message;
            Id = id;
            Committer = committer;
            When = when;
        }

        public string Message { get; }
        public string Id { get; }
        public string Committer { get; }
        public DateTimeOffset When { get; }
    }
}