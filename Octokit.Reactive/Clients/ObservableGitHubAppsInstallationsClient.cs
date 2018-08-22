﻿using System;
using Octokit.Clients;
using Octokit.Reactive.Internal;

namespace Octokit.Reactive
{
    /// <summary>
    /// A client for GitHub Applications Installations API.
    /// </summary>
    /// <remarks>
    /// See the <a href="http://developer.github.com/v3/apps/installations/">GitHub Apps Installations API documentation</a> for more information.
    /// </remarks>
    public class ObservableGitHubAppsInstallationsClient : IObservableGitHubAppsInstallationsClient
    {
        private IGitHubAppsInstallationsClient _client;
        private readonly IConnection _connection;

        public ObservableGitHubAppsInstallationsClient(IGitHubClient client)
        {
            _client = client.GitHubApps.Installation;
            _connection = client.Connection;
        }

        /// <summary>
        /// List repositories of the authenticated GitHub App Installation (requires GitHubApp Installation-Token auth).
        /// </summary>
        /// <remarks>https://developer.github.com/v3/apps/installations/#list-repositories</remarks>
        public IObservable<RepositoriesResponse> GetAllRepositoriesForCurrent()
        {
            return _connection.GetAndFlattenAllPages<RepositoriesResponse>(ApiUrls.InstallationRepositories(), null, AcceptHeaders.GitHubAppsPreview);
        }

        /// <summary>
        /// List repositories of the authenticated GitHub App Installation (requires GitHubApp Installation-Token auth).
        /// </summary>
        /// <param name="options">Options for changing the API response</param>
        /// <remarks>https://developer.github.com/v3/apps/installations/#list-repositories</remarks>
        public IObservable<RepositoriesResponse> GetAllRepositoriesForCurrent(ApiOptions options)
        {
            return _connection.GetAndFlattenAllPages<RepositoriesResponse>(ApiUrls.InstallationRepositories(), null, AcceptHeaders.GitHubAppsPreview, options);
        }

        /// <summary>
        /// List repositories accessible to the user for an installation (requires GitHubApp User-To-Server Auth).
        /// </summary>
        /// <param name="installationId">The Id of the installation</param>
        /// <remarks>https://developer.github.com/v3/apps/installations/#list-repositories-accessible-to-the-user-for-an-installation</remarks>
        public IObservable<RepositoriesResponse> GetAllRepositoriesForCurrentUser(long installationId)
        {
            return _connection.GetAndFlattenAllPages<RepositoriesResponse>(ApiUrls.UserInstallationRepositories(installationId), null, AcceptHeaders.GitHubAppsPreview);
        }

        /// <summary>
        /// List repositories accessible to the user for an installation (requires GitHubApp User-To-Server Auth).
        /// </summary>
        /// <param name="installationId">The Id of the installation</param>
        /// <param name="options">Options for changing the API response</param>
        /// <remarks>https://developer.github.com/v3/apps/installations/#list-repositories-accessible-to-the-user-for-an-installation</remarks>
        public IObservable<RepositoriesResponse> GetAllRepositoriesForCurrentUser(long installationId, ApiOptions options)
        {
            return _connection.GetAndFlattenAllPages<RepositoriesResponse>(ApiUrls.UserInstallationRepositories(installationId), null, AcceptHeaders.GitHubAppsPreview);
        }
    }
}