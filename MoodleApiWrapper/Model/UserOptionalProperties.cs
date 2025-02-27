#nullable enable
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace MoodleApiWrapper.Model;

public abstract class UserOptionalProperties
{
    public string? auth { get; set; }
    public string? idnumber { get; set; }
    public string? lang { get; set; }
    public string? calendartye { get; set; }
    public string? theme { get; set; }
    public string? timezone { get; set; }
    public string? mailformat { get; set; }
    public string? description { get; set; }
    public string? city { get; set; }
    public string? country { get; set; }
    public string? firstnamephonetic { get; set; }
    public string? lastnamephonetic { get; set; }
    public string? middlename { get; set; }
    public string? alternatename { get; set; }
    public bool? suspended { get; set; }


    public List<string>? preferences_type { get; set; }
    public List<string>? preferences_value { get; set; }
    public List<string>? customfields_type { get; set; }
    public List<string>? customfields_value { get; set; }

    public string? institution { get; set; }
    public string? department { get; set; }
    public string? phone1 { get; set; }
    public string? phone2 { get; set; }
    public string? address { get; set; }
}
