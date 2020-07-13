using Knx.Common;

namespace Knx.DatapointTypes.Dpt8BitUnsignedValue
{
    /// <summary>
    /// This DPT shall be used for reading and setting tariff information.
    /// A large number of different tariffs are defined and these are specific to the country and
    /// even to the supplier. Therefore, the mapping between a tariff and this DPT is not
    /// standardised. For usability and interpretability of the tariff information by the end user,
    /// the product description should give clear information about this mapping
    /// </summary>
    [DatapointType(5, 6, Unit.CounterPulses, Usage.General)]
    public class DptTariff : Dpt8BitUnsignedValue
    {
        public DptTariff(byte[] payload)
            : base(payload)
        {
        }

        public DptTariff(int value)
            : base(value)
        {
        }

        [DatapointProperty]
        [Range(0, 254, ErrorMessage = "Tariff information must be within 0 and 254")]
        public override int Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}