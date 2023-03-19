using System.ComponentModel.DataAnnotations;

namespace Knx.Cli.Configuration;

public class KnxOptions
{
    [Required]
    public KnxProtocol Protocol { get; set; }
}
