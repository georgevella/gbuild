var target = Argument("target", "Default");

Task("Clean")
  .Does(() =>
  {
    CleanDirectories("./artifacts/");
    // CleanDirectories("./src/**/bin");
    // CleanDirectories("./src/**/obj");

    var settings = new DotNetCoreCleanSettings
    {
      Configuration = "Release",
      OutputDirectory = "./artifacts/"
    };

    DotNetCoreClean("./src/GBuild.sln", settings);
  });

Task("Build")
  .Does(() =>
  {
    var settings = new DotNetCorePublishSettings
    {        
        Configuration = "Release",
        OutputDirectory = "./artifacts/win-x64",
        SelfContained = false,
        Runtime = "win7-x64"
    };

    DotNetCorePublish("./src/gbuild.console/", settings);

    settings = new DotNetCorePublishSettings
    {        
        Configuration = "Release",
        OutputDirectory = "./artifacts/linux-x64",
        SelfContained = false,
        Runtime = "linux-x64"
    };

    DotNetCorePublish("./src/gbuild.console/", settings);

        settings = new DotNetCorePublishSettings
    {        
        Configuration = "Release",
        OutputDirectory = "./artifacts/ubuntu-x64",
        SelfContained = false,
        Runtime = "ubuntu-x64"
    };

    DotNetCorePublish("./src/gbuild.console/", settings);
  });

Task("Default")
  .IsDependentOn("Clean")
  .IsDependentOn("Build");

RunTarget(target);