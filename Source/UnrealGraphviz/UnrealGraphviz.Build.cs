// Copyright Epic Games, Inc. All Rights Reserved.

using System.IO;
using System.Linq;
using UnrealBuildTool;

public class UnrealGraphviz : ModuleRules
{
	public UnrealGraphviz(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
		OptimizeCode = CodeOptimization.InShippingBuildsOnly;

		PublicDefinitions.Add("GVDLL");

		PublicDependencyModuleNames.AddRange(
			new string[]
			{
				"Core",
			}
		);


		PrivateDependencyModuleNames.AddRange(
			new string[]
			{
				"CoreUObject",
				"Engine",
				"Slate",
				"SlateCore",
			}
		);

		string[] StaticLibs =
		{
			"cdt.lib",
			"cgraph.lib",
			"gvc.lib",
			"gvplugin_core.lib",
			"gvplugin_dot_layout.lib",
			"xdot.lib",
		};

		PublicAdditionalLibraries.AddRange(
			StaticLibs.Select(Lib => Path.Combine(GetStaticLibPath(), Lib))
		);

		PrivateIncludePaths.AddRange(
			new[]
			{
				GetIncludePath()
			}
		);

		string[] DynamicLibs =
		{
			"cdt.dll",
			"cgraph.dll",
			"expat.dll",
			"gvc.dll",
			"gvplugin_core.dll",
			"gvplugin_dot_layout.dll",
			"pathplan.dll",
			"xdot.dll",
		};

		foreach (string Lib in DynamicLibs)
		{
			string LibPath = Path.Combine(GetDynamicLibPath(), Lib);
			string BinariesLibPath = CopyToProjectBinaries(LibPath);
			System.Console.WriteLine("Using Graphviz DLL: " + BinariesLibPath);
			RuntimeDependencies.Add(BinariesLibPath);
		}


		string ConfigPath = Path.Combine(GetDynamicLibPath(), "config6");
		string BinariesConfigPath = CopyToProjectBinaries(ConfigPath);
		System.Console.WriteLine("Using Graphviz config: " + BinariesConfigPath);
	}

	string GetGraphvizPath()
	{
		return Path.Combine(ModuleDirectory, "Graphviz");
	}

	string GetIncludePath()
	{
		return Path.Combine(GetGraphvizPath(), "include");
	}

	string GetStaticLibPath()
	{
		return Path.Combine(GetGraphvizPath(), "lib");
	}

	string GetDynamicLibPath()
	{
		return Path.Combine(GetGraphvizPath(), "bin");
	}

	string GetProjectPath()
	{
		return Path.Combine(ModuleDirectory, "../../../..");
	}

	string CopyToProjectBinaries(string FilePath)
	{
		string BinariesDir = Path.Combine(GetProjectPath(), "Binaries", Target.Platform.ToString());
		string Filename = Path.GetFileName(FilePath);
		string FullBinariesDir = Path.GetFullPath(BinariesDir);

		if (!Directory.Exists(FullBinariesDir))
		{
			Directory.CreateDirectory(FullBinariesDir);
		}

		string FullExistingPath = Path.Combine(FullBinariesDir, Filename);

		if (!File.Exists(FullExistingPath))
		{
			File.Copy(FilePath, Path.Combine(FullBinariesDir, Filename), true);
		}

		return FullExistingPath;
	}
}