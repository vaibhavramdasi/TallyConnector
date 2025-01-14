﻿using System.Globalization;
using System.Xml.Schema;

namespace TallyConnector.Core.Converters.XMLConverterHelpers;
[JsonConverter(typeof(TallyDueDateJsonConverter))]
public class TallyDueDate : IXmlSerializable
{
    public TallyDueDate()
    {
    }

    
    public TallyDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
    }
    public TallyDueDate(int value, DueDateFormat suffix)
    {
        Value = value;
        Suffix = suffix;
    }

    private DateTime? _billDate;
    public int Value { get; private set; }
    public DueDateFormat? Suffix { get; private set; }
    public DateTime? DueDate { get; private set; }

    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        var JD = reader.GetAttribute("JD");
        var tValue = reader.ReadElementContentAsString();

        if (!string.IsNullOrEmpty(tValue))
        {
            if (tValue.Contains('-'))
            {
                Suffix = DueDateFormat.Date;
                bool v = DateTime.TryParseExact(tValue, "d-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                bool sdate = DateTime.TryParseExact(tValue, "d-MMM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ShrtDate);
                if (v)
                {
                    DueDate = date;
                }
                if (sdate)
                {
                    DueDate = ShrtDate;
                }
            }
            else
            {
                if (JD != null)
                {
                    _billDate = new DateTime(1900, 1, 1).AddDays(int.Parse(JD) - 1);
                }
                var splittedvalues = tValue.Split(' ');
                var suffix = splittedvalues.Last().Trim();
                Value = int.Parse(splittedvalues.First());
                if (suffix.Contains("Days"))
                {
                    Suffix = DueDateFormat.Day;
                }
                else if (suffix.Contains("Weeks"))
                {
                    Suffix = DueDateFormat.Week;
                }
                else if (suffix.Contains("Months"))
                {
                    Suffix = DueDateFormat.Month;
                }
                else if (suffix.Contains("Years"))
                {
                    Suffix = DueDateFormat.Year;
                }
                if (_billDate != null)
                {
                    DueDate = Suffix == DueDateFormat.Month ?
                    _billDate?.AddMonths(Value) : Suffix == DueDateFormat.Year ?
                    _billDate?.AddYears(Value) : Suffix == DueDateFormat.Week ? _billDate?.AddDays(Value * 7) : _billDate?.AddDays(Value);
                }

            }

        }

    }

    public void WriteXml(XmlWriter writer)
    {
        if (this != null)
        {
            writer.WriteAttributeString("TYPE", "Due Date");
            // writer.WriteAttributeString("", "");
            if (Value != 0 && Suffix != null)
            {
                writer.WriteString($"{Value} {Suffix}s");
            }
            else if (DueDate != null)
            {
                writer.WriteString(DueDate?.ToString("dd-MMM-yyyy"));
            }

        }
    }

    public static implicit operator TallyDueDate(DateTime dueDate)
    {
        return new(dueDate);
    }
}

public enum DueDateFormat
{
    Day,
    Week,
    Month,
    Year,
    Date,
}
