## Sq.NET
A C# binding for the Squirrel programming language

*Sq.NET* is intended to be a comprehensive binding of Squirrel 3.1 to C#. Every function is currently implemented, except for some sqstd_\* functions, which will be added eventually.

Due to the usage of varargs in Squirrel's printing functions, a custom fork of Squirrel must be used that makes use of a custom variant type instead, so that it can be sent to C# without issues. This fork is present as a submodule in this repository, so don't forget to run `git submodule update --init --recursive` after cloning.  
The changes to the Squirrel code are rather small, and are quite easy to make. See the fork's repository for more.

## Compiling
To generate Visual Studio 2015 project files, simply run the `generate_vs2015.bat` file in the repository root. Alternatively, see [Using Premake](https://github.com/premake/premake-core/wiki/Using-Premake). Once the project files have been generated, all you need to do is build the solution.

## License
Sq.NET is licensed under the [MIT License](LICENSE)
