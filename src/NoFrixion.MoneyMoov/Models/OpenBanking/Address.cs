
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Address")]
public partial class Address
{
    /// <summary>
    /// The type of the address.
    /// </summary>
    [DataMember(Name = "addressType", EmitDefaultValue = true)]
    public AddressTypeEnum AddressType { get; set; }
    
    /// <summary>
    /// The address line of the address.
    /// </summary>
    [DataMember(Name = "addressLines", EmitDefaultValue = false)]
    public List<string>? AddressLines { get; set; }

    /// <summary>
    /// The street name of the address.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "streetName", EmitDefaultValue = false)]
    public string StreetName { get; set; } = string.Empty;

    /// <summary>
    /// The building number of the address.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "buildingNumber", EmitDefaultValue = false)]
    public string BuildingNumber { get; set; } = string.Empty;

    /// <summary>
    /// The post code of the address.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "postCode", EmitDefaultValue = false)]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// The town name of the address.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "townName", EmitDefaultValue = false)]
    public string TownName { get; set; } = string.Empty;

    /// <summary>
    /// The list of counties for the address.
    /// </summary>
    [DataMember(Name = "county", EmitDefaultValue = false)]
    public List<string>? County { get; set; }

    /// <summary>
    /// The 2-letter country code for the address. Institution's; may require you to specify the country when used in the context of the Payee to be able to make a payment.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "country", EmitDefaultValue = false)]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// The department for the address.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "department", EmitDefaultValue = false)]
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// The sub-department for the address.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "subDepartment", EmitDefaultValue = false)]
    public string SubDepartment { get; set; } = string.Empty;
}