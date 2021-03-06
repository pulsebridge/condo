// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitRepositoryExtensions.cs" company="automotiveMastermind and contributors">
//   © automotiveMastermind and contributors. Licensed under MIT. See LICENSE and CREDITS for details.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AM.Condo.IO
{
    using System.Text;

    /// <summary>
    /// Represents a set of extension methods for the <see cref="IGitRepository"/> interface.
    /// </summary>
    public static class GitRepositoryExtensions
    {
        #region Private Fields
        private static readonly GitLogOptions Options = new GitLogOptions
        {
            HeaderPattern = @"^(\w*)(?:\(([\w\$\.\-\* ]*)\))?\: (.*)$",
            RevertPattern = @"^Revert\s""([\s\S]*)""\s*This reverts commit (\w*)\.",
            FieldPattern = @"^-(.*?)-$",
            IncludeInvalidCommits = false,
            GroupBy = "Type",
            SortBy = "Scope",
            HeaderCorrespondence = { "Type", "Scope", "Subject" },
            RevertCorrespondence = { "Header", "Hash" },
            ReferencePrefixes = { "#" },
            MentionPrefixes = { "@" },
            ActionKeywords = { "Close", "Closes", "Closed", "Fix", "Fixes", "Fixed", "Resolve", "Resolves", "Resolved" },
            NoteKeywords = { "BREAKING CHANGE", "BREAKING CHANGES" }
        };

        private static readonly IGitLogParser Parser = new GitLogParser();
        #endregion

        #region Methods
        /// <summary>
        /// Tracks changes of all modifications made to all files within the repository.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which all changes should be tracked.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Add(this IGitRepositoryInitialized repository)
        {
            return repository.Add(".", force: false);
        }

        /// <summary>
        /// Creates a default README file with no content at the root of the repository.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to save a readme file.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Save(this IGitRepositoryInitialized repository)
        {
            return repository.Save("README", string.Empty);
        }

        /// <summary>
        /// Creates a file with the specified <paramref name="relativePath"/> with no content.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to save an empty file at the specified <paramref name="relativePath"/>.
        /// </param>
        /// <param name="relativePath">
        /// The path of the file relative to the root of the repository that should be created with no content.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Save(this IGitRepositoryInitialized repository, string relativePath)
        {
            return repository.Save(relativePath, string.Empty);
        }

        /// <summary>
        /// Creates a new commit with the specified <paramref name="subject"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to commit.
        /// </param>
        /// <param name="subject">
        /// The subject, or first line of the commit message.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Commit(this IGitRepositoryInitialized repository, string subject)
        {
            return repository.Commit(type: null, scope: null, subject: subject, body: null, notes: null);
        }

        /// <summary>
        /// Creates a new commit with the specified <paramref name="type"/> and <paramref name="subject"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to commit.
        /// </param>
        /// <param name="type">
        /// The type of the commit.
        /// </param>
        /// <param name="subject">
        /// The subject, or first line of the commit message.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Commit
            (this IGitRepositoryInitialized repository, string type, string subject)
        {
            return repository.Commit(type: type, scope: null, subject: subject, body: null, notes: null);
        }

        /// <summary>
        /// Creates a new commit with the specified <paramref name="type"/>, <paramref name="scope"/> and
        /// <paramref name="subject"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to commit.
        /// </param>
        /// <param name="type">
        /// The type of the commit.
        /// </param>
        /// <param name="scope">
        /// The scope of the commit.
        /// </param>
        /// <param name="subject">
        /// The subject, or first line of the commit message.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Commit
            (this IGitRepositoryInitialized repository, string type, string scope, string subject)
        {
            return repository.Commit(type, scope, subject, body: null, notes: null);
        }

        /// <summary>
        /// Creates a new commit with the specified <paramref name="type"/>, <paramref name="scope"/> and
        /// <paramref name="subject"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to commit.
        /// </param>
        /// <param name="type">
        /// The type of the commit.
        /// </param>
        /// <param name="scope">
        /// The scope of the commit.
        /// </param>
        /// <param name="subject">
        /// The subject, or first line of the commit message.
        /// </param>
        /// <param name="body">
        /// The body of the commit.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Commit
            (this IGitRepositoryInitialized repository, string type, string scope, string subject, string body)
        {
            return repository.Commit(type, scope, subject, body, notes: null);
        }

        /// <summary>
        /// Creates a new commit with the specified <paramref name="type"/>, <paramref name="scope"/> and
        /// <paramref name="subject"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to commit.
        /// </param>
        /// <param name="type">
        /// The type of the commit.
        /// </param>
        /// <param name="scope">
        /// The scope of the commit.
        /// </param>
        /// <param name="subject">
        /// The subject, or first line of the commit message.
        /// </param>
        /// <param name="body">
        /// The body of the commit message.
        /// </param>
        /// <param name="notes">
        /// The notes for the commit message.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Commit
            (
                this IGitRepositoryInitialized repository,
                string type,
                string scope,
                string subject,
                string body,
                string notes
            )
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(type))
            {
                message.Append(type);
            }

            if (!string.IsNullOrEmpty(scope))
            {
                message.Append('(');
                message.Append(scope);
                message.Append(')');
            }

            if (!string.IsNullOrEmpty(subject))
            {
                if (message.Length > 0)
                {
                    message.Append(": ");
                }

                message.Append(subject);
            }

            if (!string.IsNullOrEmpty(body))
            {
                if (message.Length > 0)
                {
                    message.AppendLine();
                    message.AppendLine();
                }

                message.Append(body);
            }

            if (!string.IsNullOrEmpty(notes))
            {
                if (message.Length > 0)
                {
                    message.AppendLine();
                    message.AppendLine();
                }

                message.Append(notes);
            }

            return repository.Commit(message.ToString());
        }

        /// <summary>
        /// Restores all available submodules within the current repository.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to restore submodules.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized RestoreSubmodules(this IGitRepositoryInitialized repository)
        {
            return repository.RestoreSubmodules(recursive: false);
        }

        /// <summary>
        /// Creates a new branch with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which to create a new branch.
        /// </param>
        /// <param name="name">
        /// The name of the branch to create.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Branch(this IGitRepositoryInitialized repository, string name)
        {
            return repository.Branch(name, source: null);
        }

        /// <summary>
        /// Gets a log of commits using the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance from which to retrieve a log of commits.
        /// </param>
        /// <param name="options">
        /// The options used to retrieve and parse the log of commits.
        /// </param>
        /// <returns>
        /// The log of commits using the specified <paramref name="options"/>.
        /// </returns>
        public static GitLog Log(this IGitRepositoryInitialized repository, GitLogOptions options)
        {
            return repository.Log(from: null, to: "HEAD", options: options, parser: null);
        }

        /// <summary>
        /// Gets a log of commits using the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository instance from which to retrieve a log of commits.
        /// </param>
        /// <param name="from">
        /// The git item specification from which to start the log.
        /// </param>
        /// <param name="options">
        /// The options used to retrieve and parse the log of commits.
        /// </param>
        /// <returns>
        /// The log of commits using the specified <paramref name="options"/>.
        /// </returns>
        public static GitLog Log(this IGitRepositoryInitialized repository, string from, GitLogOptions options)
        {
            return repository.Log(from, to: "HEAD", options: options, parser: null);
        }

        /// <summary>
        /// Gets the log of commmits from a repository using the default log options, which are based on the AngularJS
        /// conventional changelog presets.
        /// </summary>
        /// <param name="repository">
        /// The repository instance from which to retrieve a log of commits.
        /// </param>
        /// <returns>
        /// The log of commits from the repository using the default log options, which are based on the AngularJS
        /// conventional changelog presets.
        /// </returns>
        public static GitLog Log(this IGitRepositoryInitialized repository)
        {
            return repository.Log(from: null, to: null, options: Options, parser: Parser);
        }

        /// <summary>
        /// Pushes any staged changes to the "origin" remote and includes tags.
        /// </summary>
        /// <param name="repository">
        /// The repository instance from which commits should be pushed.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Push(this IGitRepositoryInitialized repository)
        {
            return repository.Push(remote: "origin", tags: true);
        }

        /// <summary>
        /// Pushes any staged changes to the "origin" remote and optionally includes tags.
        /// </summary>
        /// <param name="repository">
        /// The repository instance from which commits should be pushed.
        /// </param>
        /// <param name="tags">
        /// A value indicating whether or not to include tags.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Push(this IGitRepositoryInitialized repository, bool tags)
        {
            return repository.Push(remote: "origin", tags: tags);
        }

        /// <summary>
        /// Pulls the changes for the current branch.
        /// </summary>
        /// <param name="repository">
        /// The repository instance in which commits should be pulled.
        /// </param>
        /// <returns>
        /// The current repository instance.
        /// </returns>
        public static IGitRepositoryInitialized Pull(this IGitRepositoryInitialized repository)
        {
            return repository.Pull(all: false);
        }
        #endregion
    }
}
