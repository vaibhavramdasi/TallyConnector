﻿using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TallyConnector.Core.Models;
using TallyConnector.Core.Models.Masters;

namespace Tests.CustomReports;
internal class GroupTest
{
    TallyConnector.Tally Tally = new();
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void CheckGetReportforGroup()
    {
        ReportField reportField = Tally.CrateTDLReport(typeof(Group));

        RequestEnvelope requestEnvelope = new(reportField);
        var xml = requestEnvelope.GetXML();

        Assert.IsNotNull(xml);
    }
    [Test]
    public async Task CheckGetVoucherStats()
    {
        var Stat = await Tally.GetVoucherStatistics();

        Assert.IsNotNull(Stat);
    } 
    [Test]
    public async Task CheckGetMasterStats()
    {
        var Stat = await Tally.GetMasterStatistics();

        Assert.IsNotNull(Stat);
    }
}

public class TGroup : Group, IXmlSerializable
{
    public TGroup()
    {
    }

    public TGroup(string name, string parent) : base(name, parent)
    {
    }

    [XmlElement(ElementName = "CALCULABLE")]
    public TallyYesNo Calculable { get; set; }

    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        while (reader.Read())
        {

        }
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotImplementedException();
    }
}
