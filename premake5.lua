workspace "Sq.NET"
	configurations { "Debug", "Release" }
	platforms { "Win32", "x64" }
	location "Build"
	targetdir "Bin/%{cfg.buildcfg}"
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

project "Sq.NET"
	defines "SQUNICODE"
	kind "SharedLib"
	language "C#"
	files "Sq.NET/**.cs"

project "Sq.REPL"
	defines "SQUNICODE"
	kind "ConsoleApp"
	language "C#"
	files "Sq.REPL/**.cs"
	links{"System", "Sq.NET"}
