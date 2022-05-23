﻿namespace TallyConnector.Core.Models.Masters.CostCenter;


[XmlRoot(ElementName = "COSTCATEGORY")]
[XmlType(AnonymousType = true)]
public class CostCategory : BasicTallyObject, ITallyObject
{
    public CostCategory()
    {
        LanguageNameList = new();
    }
    public CostCategory(string name)
    {
        LanguageNameList = new();
        Name = name;
    }

    [XmlAttribute(AttributeName = "NAME")]
    [JsonIgnore]
    public string? OldName { get; set; }

    private string? name;

    [XmlElement(ElementName = "NAME")]
    [Required]
    public string Name
    {
        get
        {
            name = name == null || name == string.Empty ? OldName : name;
            return name!;
        }
        set => name = value;
    }

    [XmlElement(ElementName = "ALLOCATEREVENUE")]
    public YesNo AllocateRevenue { get; set; }

    [XmlElement(ElementName = "ALLOCATENONREVENUE")]
    public YesNo AllocateNonRevenue { get; set; }

    [XmlIgnore]
    public string? Alias { get; set; }

    [JsonIgnore]
    [XmlElement(ElementName = "LANGUAGENAME.LIST")]
    [TDLCollection(CollectionName = "LanguageName")]
    public List<LanguageNameList> LanguageNameList { get; set; }
    

    public void CreateNamesList()
    {
        if (LanguageNameList.Count == 0)
        {
            LanguageNameList.Add(new LanguageNameList());
            LanguageNameList?[0]?.NameList?.NAMES?.Add(Name);

        }
        if (Alias != null && Alias != string.Empty)
        {
            LanguageNameList![0].LanguageAlias = Alias;
        }
    }
    public new string GetXML(XmlAttributeOverrides? attrOverrides = null)
    {
        CreateNamesList();
        return base.GetXML(attrOverrides);
    }

    public new void PrepareForExport()
    {
        CreateNamesList();
    }

    public override string ToString()
    {
        return $"Cost Category - {Name}";
    }
}