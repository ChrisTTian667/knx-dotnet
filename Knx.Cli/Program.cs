﻿using Knx.Cli.Commands;
using Knx.Cli.Configuration;
using Knx.DatapointTypes;
using Knx.DatapointTypes.Dpt1Bit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Knx.Cli;

internal sealed class Program
{
    private static readonly string UserProfileFolder =
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    private static string? _settingsJsonPath;

    public static string DefaultSettingsPath
    {
        get
        {
            if (_settingsJsonPath is null)
            {
                var configurationDirectory = Path.Combine(UserProfileFolder);
                _settingsJsonPath = Path.Combine(configurationDirectory, ".knx");
            }

            return _settingsJsonPath;
        }
    }

    private static IConfiguration BuildConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        foreach (var configFile in GetConfigurationFiles())
        {
            var configFilePath = Path.GetDirectoryName(configFile)!;
            var configFileName = Path.GetFileName(configFile);

            configurationBuilder.AddJsonFile(
                new PhysicalFileProvider(configFilePath, ExclusionFilters.None),
                configFileName,
                optional: true,
                reloadOnChange: true);
        }

        return configurationBuilder.Build();
    }

    private static IEnumerable<string> GetConfigurationFiles()
    {
        var configFiles = new List<string> { DefaultSettingsPath };
        var currentDirectory = Directory.GetCurrentDirectory();

        do
        {
            var configFilename = Path.Combine(currentDirectory!, ".knx");
            if (File.Exists(configFilename) && currentDirectory != UserProfileFolder)
            {
                configFiles.Add(configFilename);
            }

            currentDirectory = Directory.GetParent(currentDirectory!)?.FullName;
        } while (currentDirectory == null);

        return configFiles;
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KnxOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations();

        //services.AddSingleton<ReleaseService>();
    }

    private static CommandApp CreateApp(IServiceCollection services)
    {
        var app = new CommandApp(new TypeRegistrar(services));

        app.Configure(config =>
        {
            // TODO add commands for each datatype here...

            // config.AddBranch<WriteCommandSettings>(
            //     "write",
            //     write =>
            //     {
            //         foreach (var dpt in DatapointTypeFactory.GetAllTypes())
            //         {
            //             typeof(WriteDatapointType<>).MakeGenericType(dpt);
            //
            //             write.AddCommand<WriteDatapointType<DptBoolean>>("boolean");
            //         }
            //     });

            config.AddCommand<List>("list")
                .WithDescription($"Lists all the [green]objects of the specified type[/]. Run [grey]list --help[/] for details.")
                .WithExample(new[] { "list DatapointTypes" });

            config.AddCommand<Write>("write")
                .WithDescription($"Sends a KnxNetIP [green]write[/] message. [grey]show --help[/] for details.")
                .WithExample(new[] { "write 1/1/28 false" });

            config.AddCommand<Read>("read")
                .WithDescription($"Sends a KnxNetIP [green]read[/] message. [grey]show --help[/] for details.")
                .WithExample(new[] { "read 1/1/28" });

            config.AddCommand<Reply>("reply")
                .WithDescription($"Sends a KnxNetIP [green]reply[/] message. [grey]show --help[/] for details.")
                .WithExample(new[] { "reply 1/1/28 false" });;

            config.AddCommand<Discover>("discover")
                .WithDescription($"Sends a KnxNetIP [green]discovery[/] request to find all the KnxNetIp devices in your network. Run [grey]show --help[/] for details.");

            config.SetExceptionHandler(ex =>
            {
                if (ex.InnerException is OptionsValidationException validationException)
                {
                    AnsiConsole.MarkupLine("[red]Invalid Configuration[/]");
                    foreach (var failure in validationException.Failures)
                    {
                        AnsiConsole.MarkupLine($"[grey]- {failure}[/]");
                    }
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine($"Run \"init\" command to build new configuration.");

                    return -2;
                }

                AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
                return -1;
            });
        });

        return app;
    }

    private static int Main(string[] args)
    {
        var configuration = BuildConfiguration();
        var services = new ServiceCollection();

        ConfigureServices(services, configuration);

        var app = CreateApp(services);
        return app.Run(args);
    }
}
