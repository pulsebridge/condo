---
layout: docs
title: mkdir
group: shades
---

@{/*

mkdir
    Creates a directory and all child directories if it doesn't already exist.

mkdir_name=''
    The name of the directory to create (the directory will be created within the current base_path)

mkdir_path=''
    The fully-qualified path of the directory to create.

base_path='$(CurrentDirectory)'
    The base path in which to create the directory (if the mkdir_name parameter is set).

*/}

use namespace = 'System'
use namespace = 'System.IO'

use import = 'Condo.Build'

default base_path = '${ Directory.GetCurrentDirectory() }'

default mkdir_name = ''
default mkdir_path = ''

@{
    Build.Log.Header("mkdir");

    if (string.IsNullOrEmpty(mkdir_path))
    {
        if (string.IsNullOrEmpty(mkdir_name))
        {
            throw new ArgumentException("Either a directory name or path must be specified.", "mkdir_path");
        }

        mkdir_path = Path.Combine(base_path, mkdir_name);
    }

    Build.Log.Argument("path", mkdir_name);
    Build.Log.Header();

    Build.MakeDirectory(mkdir_path);
}