
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Address")]
public partial class Address
{
    /// <summary>
    /// Gets or Sets AddressType
    /// </summary>
    [DataMember(Name = "addressType", EmitDefaultValue = false)]
    public AddressTypeEnum AddressType { get; set; }
    
    /// <summary>
    /// __Optional__. The address line of the address
    /// </summary>
    /// <value>__Optional__. The address line of the address</value>
    [DataMember(Name = "addressLines", EmitDefaultValue = false)]
    public List<string> AddressLines { get; set; } = new List<string>();

    /// <summary>
    /// __Optional__. The street name of the address
    /// </summary>
    /// <value>__Optional__. The street name of the address</value>
    [DataMember(Name = "streetName", EmitDefaultValue = false)]
    public string StreetName { get; set; } = string.Empty;

    /// <summary>
    /// __Optional__. The building number of the address
    /// </summary>
    /// <value>__Optional__. The building number of the address</value>
    [DataMember(Name = "buildingNumber", EmitDefaultValue = false)]
    public string BuildingNumber { get; set; } = string.Empty;

    /// <summary>
    /// __Optional__. The post code of the address
    /// </summary>
    /// <value>__Optional__. The post code of the address</value>
    [DataMember(Name = "postCode", EmitDefaultValue = false)]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// __Optional__. The town name of the address
    /// </summary>
    /// <value>__Optional__. The town name of the address</value>
    [DataMember(Name = "townName", EmitDefaultValue = false)]
    public string TownName { get; set; } = string.Empty;

    /// <summary>
    /// __Optional__. The list of counties for the address
    /// </summary>
    /// <value>__Optional__. The list of counties for the address</value>
    [DataMember(Name = "county", EmitDefaultValue = false)]
    public List<string> County { get; set; } = new List<string>();

    /// <summary>
    /// __Conditional__. The 2-letter country code for the address. &lt;br&gt;&lt;br&gt;An &#x60;Institution&#x60; may require you to specify the &#x60;country&#x60; when used in the context of the &#x60;Payee&#x60; to be able to make a payment
    /// </summary>
    /// <value>__Conditional__. The 2-letter country code for the address. &lt;br&gt;&lt;br&gt;An &#x60;Institution&#x60; may require you to specify the &#x60;country&#x60; when used in the context of the &#x60;Payee&#x60; to be able to make a payment</value>
    [DataMember(Name = "country", EmitDefaultValue = false)]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// __Optional__. The department for the address
    /// </summary>
    /// <value>__Optional__. The department for the address</value>
    [DataMember(Name = "department", EmitDefaultValue = false)]
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// __Optional__. The sub-department for the address
    /// </summary>
    /// <value>__Optional__. The sub-department for the address</value>
    [DataMember(Name = "subDepartment", EmitDefaultValue = false)]
    public string SubDepartment { get; set; } = string.Empty;
}