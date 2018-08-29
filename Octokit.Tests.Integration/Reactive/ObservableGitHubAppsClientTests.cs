﻿using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Octokit.Reactive;
using Xunit;

namespace Octokit.Tests.Integration.Clients
{
    public class ObservableGitHubAppsClientTests
    {
        public class TheGetMethod
        {
            IObservableGitHubClient _github;

            public TheGetMethod()
            {
                // Regular authentication
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedClient());
            }

            [GitHubAppsTest]
            public async Task GetsApp()
            {
                var result = await _github.GitHubApps.Get(Helper.GitHubAppSlug);

                Assert.Equal(Helper.GitHubAppId, result.Id);
                Assert.False(string.IsNullOrEmpty(result.Name));
                Assert.NotNull(result.Owner);
            }
        }

        public class TheGetCurrentMethod
        {
            IObservableGitHubClient _github;

            public TheGetCurrentMethod()
            {
                // Authenticate as a GitHubApp
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppsClient());
            }

            [GitHubAppsTest]
            public async Task GetsCurrentApp()
            {
                var result = await _github.GitHubApps.GetCurrent();

                Assert.Equal(Helper.GitHubAppId, result.Id);
                Assert.False(string.IsNullOrEmpty(result.Name));
                Assert.NotNull(result.Owner);
            }
        }

        public class TheGetAllInstallationsForCurrentMethod
        {
            IObservableGitHubClient _github;

            public TheGetAllInstallationsForCurrentMethod()
            {
                // Authenticate as a GitHubApp
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppsClient());
            }

            [GitHubAppsTest]
            public async Task GetsAllInstallations()
            {
                var result = await _github.GitHubApps.GetAllInstallationsForCurrent().ToList();

                foreach (var installation in result)
                {
                    Assert.Equal(Helper.GitHubAppId, installation.AppId);
                    Assert.NotNull(installation.Account);
                    Assert.NotNull(installation.Permissions);
                    Assert.Equal(InstallationPermissionLevel.Read, installation.Permissions.Metadata);
                    Assert.False(string.IsNullOrEmpty(installation.HtmlUrl));
                    Assert.NotEqual(0, installation.TargetId);
                }
            }
        }

        public class TheGetInstallationForCurrentMethod
        {
            IObservableGitHubClient _github;

            public TheGetInstallationForCurrentMethod()
            {
                // Authenticate as a GitHubApp
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppsClient());
            }

            [GitHubAppsTest]
            public async Task GetsInstallation()
            {
                // Get the installation Id
                var installationId = Helper.GetGitHubAppInstallationForOwner(Helper.UserName).Id;

                // Get the installation by Id
                var result = await _github.GitHubApps.GetInstallationForCurrent(installationId);

                Assert.True(result.AppId == Helper.GitHubAppId);
                Assert.Equal(Helper.GitHubAppId, result.AppId);
                Assert.NotNull(result.Account);
                Assert.NotNull(result.Permissions);
                Assert.Equal(InstallationPermissionLevel.Read, result.Permissions.Metadata);
                Assert.False(string.IsNullOrEmpty(result.HtmlUrl));
                Assert.NotEqual(0, result.TargetId);
            }
        }

        public class TheGetAllInstallationsForCurrentUserMethod
        {
            IObservableGitHubClient _github;

            public TheGetAllInstallationsForCurrentUserMethod()
            {
                // Need to Authenticate as User to Server but not possible without receiving redirect from github.com
                //_github = new ObservableGitHubClient(Helper.GetAuthenticatedUserToServer());
                _github = null;
            }

            [GitHubAppsTest(Skip = "Not possible to authenticate with User to Server auth")]
            public async Task GetsAllInstallationsForCurrentUser()
            {
                var result = await _github.GitHubApps.GetAllInstallationsForCurrentUser();

                Assert.NotNull(result);
            }
        }

        public class TheCreateInstallationTokenMethod
        {
            IObservableGitHubClient _github;

            public TheCreateInstallationTokenMethod()
            {
                // Authenticate as a GitHubApp
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppsClient());
            }

            [GitHubAppsTest]
            public async Task CreatesInstallationToken()
            {
                // Get the installation Id
                var installationId = Helper.GetGitHubAppInstallationForOwner(Helper.UserName).Id;

                // Create installation token
                var result = await _github.GitHubApps.CreateInstallationToken(installationId);

                Assert.NotNull(result.Token);
                Assert.True(DateTimeOffset.Now < result.ExpiresAt);
            }
        }

        public class TheGetOrganizationInstallationForCurrentMethod
        {
            IObservableGitHubClient _github;

            public TheGetOrganizationInstallationForCurrentMethod()
            {
                // Authenticate as a GitHubApp
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppsClient());
            }

            [GitHubAppsTest]
            public async Task GetsOrganizationInstallations()
            {
                var result = await _github.GitHubApps.GetOrganizationInstallationForCurrent(Helper.Organization);

                Assert.NotNull(result);
            }
        }

        public class TheGetRepositoryInstallationForCurrentMethod
        {
            IObservableGitHubClient _github;
            IObservableGitHubClient _githubAppInstallation;

            public TheGetRepositoryInstallationForCurrentMethod()
            {
                // Autheticate as a GitHubApp
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppsClient());

                // Authenticate as a GitHubApp Installation
                _githubAppInstallation = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppInstallationForOwner(Helper.UserName));
            }

            [GitHubAppsTest]
            public async Task GetsRepositoryInstallations()
            {
                // Find a repo under the installation
                var repos = await _githubAppInstallation.GitHubApps.Installation.GetAllRepositoriesForCurrent();
                var repo = repos.Repositories.First();

                // Now, using the GitHub App auth, find this repository installation
                var result = await _github.GitHubApps.GetRepositoryInstallationForCurrent(repo.Owner.Login, repo.Name);

                Assert.NotNull(result);
            }

            [GitHubAppsTest]
            public async Task GetsRepositoryInstallationsWithRepositoryId()
            {
                // Find a repo under the installation
                var repos = await _githubAppInstallation.GitHubApps.Installation.GetAllRepositoriesForCurrent();
                var repo = repos.Repositories.First();

                // Now, using the GitHub App auth, find this repository installation
                var result = await _github.GitHubApps.GetRepositoryInstallationForCurrent(repo.Id);

                Assert.NotNull(result);
            }
        }

        public class TheGetUserInstallationForCurrentMethod
        {
            IObservableGitHubClient _github;

            public TheGetUserInstallationForCurrentMethod()
            {
                // Authenticate as a GitHubApp
                _github = new ObservableGitHubClient(Helper.GetAuthenticatedGitHubAppsClient());
            }

            [GitHubAppsTest]
            public async Task GetsUserInstallations()
            {
                var result = await _github.GitHubApps.GetUserInstallationForCurrent(Helper.UserName);

                Assert.NotNull(result);
            }
        }
    }
}
