# condo

> A build system for [DNX][dnx-url] projects.

## Vitals

Info          | Badges
--------------|--------------
Version       | [![Version][release-v-image]][release-url] [![NuGet Version][nuget-v-image]][nuget-url]
License       | [![License][license-image]][license]
Downloads     | [![NuGet Downloads][nuget-d-image]][nuget-url]
Build Status  | [![Travis Build Status][travis-image]][travis-url] [![AppVeyor Build Status][appveyor-image]][appveyor-url]
Chat          | [![Join Chat][gitter-image]][gitter-url]

## Getting Started

### What is Condo?

Condo is a cross-platform command line interface (CLI) build system for projects using the .NET Core Framework. It is capable of automatically detecting and executing all of the steps
necessary to make a DNX project function correctly, including, but not limited to:

* Automatic semantic versioning
* Restoring package manager dependencies (NuGet, NPM, Bower)
* Executing default task runner commands (Grunt, Gulp)
* Compiling projects and test projects
* Executing unit tests
* Packing NuGet packages
* Pushing (Publishing) NuGet packages

These are just some of the most-used features of the build system.

### Using Condo

The easiest way to start using Condo is to use the [Yeoman Condo Generator][yo-url], which will configure a new "solution" structure for use with the Condo build system.

1. Make sure that you have Yeoman installed:

	```bash
	npm install -g yo
	```

2. Install the Condo generator:

	```bash
	npm install -g generator-condo
	```

3. Initiate the generator:

	```bash
	yo condo
	```

4. Run the build:

	OS X / Linux:

	```bash
	./condo.sh
	```

	Windows (CLI):

	```cmd
	condo
	```

	Windows (PoSH):
	```posh
	./condo.ps1
	```

## Documentation

For more information, please refer to the [official documentation][docs-url].

## Copright and License

&copy; PulseBridge, Inc. and contributors. Distributed under the MIT license. See [LICENSE][] and [CREDITS][] for details.

[license-image]: https://img.shields.io/badge/license-MIT-blue.svg
[license]: LICENSE
[credits]: CREDITS

[release-url]: //github.com/pulsebridge/condo/releases/latest
[release-v-image]:https://img.shields.io/github/release/pulsebridge/condo.svg?style=flat-square&label=github

[travis-url]: //travis-ci.org/pulsebridge/condo
[travis-image]: https://img.shields.io/travis/pulsebridge/condo.svg?label=travis

[appveyor-url]: //ci.appveyor.com/project/pulsebridge/condo
[appveyor-image]: https://img.shields.io/appveyor/ci/pulsebridge/condo.svg?label=appveyor

[nuget-url]: //www.nuget.org/packages/pulsebridge.condo
[nuget-v-image]: https://img.shields.io/nuget/v/pulsebridge.condo.svg?label=nuget
[nuget-d-image]: https://img.shields.io/nuget/dt/pulsebridge.condo.svg?label=nuget

[yo-url]: //www.npmjs.com/package/generator-condo

[dnx-url]: http://docs.asp.net/en/latest/dnx/index.html
[docs-url]: http://open.pulsebridge.com/condo

[gitter-url]: //gitter.im/pulsebridge/condo
[gitter-image]:https://img.shields.io/badge/⊪%20gitter-join%20chat%20→-1dce73.svg