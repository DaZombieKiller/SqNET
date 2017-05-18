## Squirrel.NET
A C# binding for the Squirrel programming language  
*Squirrel.NET* is intended to be a comprehensive binding of Squirrel 3 for C#. Every function is currently implemented, except for some sqstd_\* functions, which will be added eventually.  
Don't forget to run `git submodule update --init --recursive` after cloning!

## Compiling
Build the `sqnetlib` project via CMake first, which should provide you with shared libraries for `squirrel`, `sqstdlib` and `sqnetlib`.  
Afterwards, run `generate_csharp.bat` if on Windows, otherwise run Protobuild with the `--generate` parameter inside the `sqnet` directory.  
This should generate project files for Squirrel.NET.

## License
Squirrel.NET is licensed under the [MIT License](LICENSE.md)
