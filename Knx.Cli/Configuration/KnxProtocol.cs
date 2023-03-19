using System.ComponentModel.DataAnnotations;

namespace Knx.Cli.Configuration;

public enum KnxProtocol
{
    [Display(Name = "Tunneling Protocol")]
    Tunneling,

    [Display(Name = "Routing Protocol")]
    Routing
}
