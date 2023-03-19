using System.ComponentModel.DataAnnotations;

namespace Knx.Cli.Configuration;

public class KnxOptions
{
    [Required]
    public KnxProtocol Protocol { get; set; } = KnxProtocol.Routing;

    [Required]
    public KnxDeviceAddress DeviceAddress { get; set; } = "0/0/0";
}
