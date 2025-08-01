using System;

namespace FreeEnterprise.Api.Models;

#pragma warning disable IDE1006 // Naming Styles - ignoring for pure database models, these names reflect what is in the database
public class RolledSeed
{
    public int id { get; set; }
    public string user_id { get; set; } = string.Empty;
    public string flagset { get; set; } = string.Empty;
    public string binary_flags { get; set; } = string.Empty;
    public string link { get; set; } = string.Empty;
    public string fe_version { get; set; } = string.Empty;
    public string seed { get; set; } = string.Empty;
    public string verification { get; set; } = string.Empty;
    public string flagset_search { get; set; } = string.Empty;
    public int? race_id { get; set; }

}
