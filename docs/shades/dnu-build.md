---
layout: docs
title: dnu-build
group: shades
---

@{/*

dnu-build
    Executes a dnu package manager command to build all available projects.

dnu_build_path='$(working_path)'
    Required. The path in which to execute the dnu command line tool.

dnu_build_project=''
    A semi-colon (;) delimited list of projects to build using the dnu command line tool.

dnu_build_framework=''
    A semi-colon (;) delimited list of target frameworks to build against.

dnu_build_configuration='$(configuration)'
    A semi-colon (;) delimited list of configurations to build.

dnu_build_options='' (Environment Variable: DNU_BUILD_OPTIONS)
    Additional options to include when executing the dnu command line tool for build operations.

dnu_build_output_path='$(target_path)/build'
    The path in which to store the resulting packages.

base_path='$(CurrentDirectory)'
    The base path in which to execute the dnu command line tool.

working_path='$(base_path)'
    The working path in which to execute the dnu command line tool.

target_path='$(working_path)/artifacts'
    The target path for build artifacts.

configuration=''
    The default configurations to use if no configurations are specified.

*/}

use namespace = 'System'
use namespace = 'System.IO'

use import = 'Condo.Build'

default configuration           = ''

default base_path               = '${ Directory.GetCurrentDirectory() }'
default working_path            = '${ base_path }'
default target_path             = '${ Path.Combine(base_path, "artifacts") }'

default dnu_build_path          = '${ working_path }'
default dnu_build_output_path   = '${ Path.Combine(target_path, "build") }'
default dnu_build_project       = ''
default dnu_build_framework     = ''
default dnu_build_configuration = '${ configuration }'
default dnu_build_options       = '${ Build.Get("DNU_BUILD_OPTIONS") }'

@{
    Build.Log.Header("dnu-build");

    if (!string.IsNullOrEmpty(dnu_build_project))
    {
        dnu_build_project = dnu_build_project.Trim();
    }

    if (!string.IsNullOrEmpty(dnu_build_options))
    {
        dnu_build_options = dnu_build_options.Trim();
    }

    var dnu_build_name = File.Exists(dnu_build_path)
        ? Path.GetDirectoryName(dnu_build_path)
        : Path.GetFileName(dnu_build_path);

    dnu_build_output_path = Path.Combine(dnu_build_output_path, dnu_build_name);

    Build.Log.Argument("path", dnu_build_path);
    Build.Log.Argument("project", dnu_build_project);
    Build.Log.Argument("framework", dnu_build_framework);
    Build.Log.Argument("configuration", dnu_build_configuration);
    Build.Log.Argument("options", dnu_build_options);
    Build.Log.Argument("output path", dnu_build_output_path);
    Build.Log.Header();

    dnu_build_project = dnu_build_project.Replace(';', ' ').Trim();

    if (!string.IsNullOrEmpty(dnu_build_output_path))
    {
        dnu_build_options += string.Format(@" --out ""{0}""", dnu_build_output_path);
    }

    if (!string.IsNullOrEmpty(dnu_build_framework))
    {
        dnu_build_options += string.Format(@" --framework ""{0}""", dnu_build_framework);
    }

    if (!string.IsNullOrEmpty(dnu_build_configuration))
    {
        dnu_build_options += string.Format(@" --configuration ""{0}""", dnu_build_configuration);
    }

    dnu_build_options = dnu_build_options.Trim();
}

dnu dnu_args='build ${ dnu_build_project }' dnu_options='${ dnu_build_options }' dnu_runtime='default -r ${ Build.Unix ? "mono" : "clr" }' dnu_path='${ dnu_build_path }'