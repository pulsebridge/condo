---
layout: docs
title: bundle
group: shades
---

@{/*

bundle
    Executes the bundler command line utility.

bundle_args=''
    Required. The arguments to pass to the bundler command line tool.

bundle_options='' (Environment Variable: BUNDLE_OPTIONS)
    Additional options to use when executing the bundler command.

bundle_path='$(base_path)'
    The path in which to execute bundler.

bundle_wait='true'
    A value indicating whether or not to wait for exit.

bundle_quiet='$(Build.Log.Quiet)'
    A value indicating whether or not to avoid printing output.

base_path='$(CurrentDirectory)'
    The base path in which to execute bundler.

working_path='$(base_path)'
    The working path in which to execute bundler.

*/}

use namespace = 'System'
use namespace = 'System.IO'

use import = 'Condo.Build'

default base_path           = '${ Directory.GetCurrentDirectory() }'
default working_path        = '${ base_path }'

default bundle_args         = ''
default bundle_options      = '${ Build.Get("BUNDLE_OPTIONS") }'

default bundle_path         = '${ working_path }'
default bundle_wait         = '${ true }'
default bundle_quiet        = '${ Build.Log.Quiet }'

bundle-download once='bundle-download'

@{
    Build.Log.Header("bundle");

    if (string.IsNullOrEmpty(bundle_args))
    {
        throw new ArgumentException("bundle: cannot execute without arguments.", "bundle_args");
    }

    bundle_args = bundle_args.Trim();
    bundle_options = bundle_options.Trim();

    var bundle_cmd          = Build.GetPath("bundle");

    if (!bundle_cmd.Global)
    {
        Build.Log.Warn("bundle: bundle not found on the system -- command will not be executed.");
    }

    Build.Log.Argument("arguments", bundle_args);
    Build.Log.Argument("options", bundle_options);
    Build.Log.Argument("path", bundle_path);
    Build.Log.Argument("wait", bundle_wait);
    Build.Log.Argument("quiet", bundle_quiet);
    Build.Log.Header();
}

exec exec_cmd='${ bundle_cmd.Path }' exec_args='${ bundle_args } ${ bundle_options }' exec_path='${ bundle_path }' exec_quiet='${ bundle_quiet }' exec_redirect='${ false }' if='bundle_cmd.Global'