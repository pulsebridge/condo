namespace PulseBridge.Condo.Build.Tasks
{
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    using Moq;

    using NuGet.Configuration;

    using PulseBridge.Condo.Build;
    using PulseBridge.Condo.IO;

    using Xunit;

    public class PushNuGetPackageTest
    {
        [Fact]
        [Priority(1)]
        public void Execute_WithNullPackages_Fails()
        {
            // arrange
            var engine = Mock.Of<IBuildEngine>();
            var provider = Mock.Of<IPackageSourceProvider>();

            var settings = Settings.LoadDefaultSettings(null, null, null);
            var packages = default(ITaskItem[]);

            var actual = new PushNuGetPackage(settings, provider)
            {
                BuildEngine = engine,
                Packages = packages
            };

            // act
            var result = actual.Execute();

            // assert
            Assert.False(result);
            Assert.True(actual.Log.HasLoggedErrors);
        }

        [Fact]
        [Priority(1)]
        public void Execute_WithEmptyPackages_Succeeds()
        {
            // arrange
            var engine = Mock.Of<IBuildEngine>();
            var provider = Mock.Of<IPackageSourceProvider>();

            var settings = Settings.LoadDefaultSettings(null, null, null);
            var packages = new ITaskItem[0];

            var actual = new PushNuGetPackage(settings, provider)
            {
                BuildEngine = engine,
                Packages = packages
            };

            // act
            var result = actual.Execute();

            // assert
            Assert.True(result);
            Assert.False(actual.Log.HasLoggedErrors);
        }

        [Fact]
        [Priority(1)]
        public void Execute_WithValidPackage_Succeeds()
        {
            using (var temp = new TemporaryPath())
            {
                // arrange
                var source = temp.Combine("source");
                var push = temp.Combine("push");

                var engine = Mock.Of<IBuildEngine>();
                var provider = NuGetMocks.CreateProvider(source, push);

                var settings = Settings.LoadDefaultSettings(null, null, null);

                var id = "test";
                var version = "1.0.0";

                var package = NuGetMocks.CreatePackage(id, version, temp.FullPath);

                var item = new TaskItem(package);
                var items = new [] { item };

                var actual = new PushNuGetPackage(settings, provider)
                {
                    BuildEngine = engine,
                    Packages = items
                };

                // act
                var result = actual.Execute();

                // assert
                Assert.True(result);
                Assert.False(actual.Log.HasLoggedErrors);

                Assert.True(File.Exists(package), "package did not exist");
                Assert.True(File.Exists(Path.Combine(push, Path.GetFileName(package))), "push package did not exist");
            }
        }
    }
}