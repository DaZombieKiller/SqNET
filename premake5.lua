workspace "sqNet"
	configurations { "Debug", "Release" }
	platforms { "Win32", "x64" }
	targetdir "bin/%{cfg.buildcfg}"
	defines { "_CRT_SECURE_NO_WARNINGS", "SQDOTNET" }

project "squirrel"
	kind "SharedLib"
	language "C++"
	includedirs "squirrel/include"
	files
	{
		"squirrel/squirrel/**.cpp",
		"squirrel/sqstdlib/**.cpp",
		"squirrel3.def",
	}

project "sqNet"
	defines "SQUNICODE"
	kind "SharedLib"
	language "C#"
	files
	{
		"sqNet/**.cs",
	}

project "sqREPL"
	defines "SQUNICODE"
	kind "ConsoleApp"
	language "C#"
	links
	{
		"System",
		"sqNet",
	}
	
	files
	{
		"sqREPL/**.cs",
	}
