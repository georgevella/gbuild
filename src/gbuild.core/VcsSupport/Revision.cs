using System;
using LibGit2Sharp;

namespace GBuild.Core.VcsSupport
{
    public class Revision
    {
        public string Message { get; }
        public string Id { get; }
        public string Committer { get; }
        public DateTimeOffset When { get; }

        public Revision(string message, string id, string committer, DateTimeOffset when)
        {
            Message = message;
            Id = id;
            Committer = committer;
            When = when;
        }
    }
}