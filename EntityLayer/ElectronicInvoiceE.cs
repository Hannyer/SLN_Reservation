using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("FacturaElectronica", Namespace = "https://cdn.comprobanteselectronicos.go.cr/xml-schemas/v4.3/facturaElectronica")]
public class ElectronicInvoiceE
{
    [XmlElement("Clave")]
    public string Key { get; set; }

    [XmlElement("CodigoActividad")]
    public string ActivityCode { get; set; }

    [XmlElement("NumeroConsecutivo")]
    public string ConsecutiveNumber { get; set; }

    [XmlElement("FechaEmision")]
    public string EmissionDate { get; set; }

    [XmlElement("Emisor")]
    public IssuerE Issuer { get; set; }

    [XmlElement("Receptor")]
    public ReceiverE Receiver { get; set; }
    [XmlElement("CondicionVenta")]
    public string SaleCondition { get; set; }
    [XmlElement("MedioPago")]
    public string Halfpayment { get; set; }

    [XmlElement("DetalleServicio")]
    public ServiceDetailE ServiceDetail { get; set; }

    [XmlElement("ResumenFactura")]
    public InvoiceSummaryE InvoiceSummary { get; set; }

    [XmlElement("Otros")]
    public OthersE Others { get; set; }
    
}

public class IssuerE
{
    [XmlElement("Nombre")]
    public string Name { get; set; }

    [XmlElement("Identificacion")]
    public IdentificationE Identification { get; set; }

    [XmlElement("NombreComercial")]
    public string CommercialName { get; set; }

    [XmlElement("Ubicacion")]
    public LocationE Location { get; set; }

    [XmlElement("Telefono")]
    public PhoneE Phone { get; set; }

    [XmlElement("CorreoElectronico")]
    public string Email { get; set; }
}

public class ReceiverE
{
    [XmlElement("Nombre")]
    public string Name { get; set; }

    [XmlElement("Identificacion")]
    public IdentificationE Identification { get; set; }

    [XmlElement("Ubicacion")]
    public LocationE Location { get; set; }

    [XmlElement("Telefono")]
    public PhoneE Phone { get; set; }

    [XmlElement("CorreoElectronico")]
    public string Email { get; set; }
}

public class IdentificationE
{
    [XmlElement("Tipo")]
    public string Type { get; set; }

    [XmlElement("Numero")]
    public string Number { get; set; }
}

public class LocationE
{
    [XmlElement("Provincia")]
    public string Province { get; set; }

    [XmlElement("Canton")]
    public string Canton { get; set; }

    [XmlElement("Distrito")]
    public string District { get; set; }

    [XmlElement("OtrasSenas")]
    public string OtherSigns { get; set; }
}

public class PhoneE
{
    [XmlElement("CodigoPais")]
    public string CountryCode { get; set; }

    [XmlElement("NumTelefono")]
    public string PhoneNumber { get; set; }
}

public class ServiceDetailE
{
    [XmlElement("LineaDetalle")]
    public List<LineDetailE> LineDetails { get; set; } = new List<LineDetailE>();
}

public class LineDetailE
{
    [XmlElement("NumeroLinea")]
    public string LineNumber { get; set; }

    [XmlElement("Codigo")]
    public string Code { get; set; }

    //[XmlElement("CodigoComercial")]
    //public CommercialCodeE CommercialCode { get; set; }

    [XmlElement("Cantidad")]
    public decimal Quantity { get; set; }

    [XmlElement("UnidadMedida")]
    public string UnitOfMeasure { get; set; }

    [XmlElement("Detalle")]
    public string Detail { get; set; }

    [XmlElement("PrecioUnitario")]
    public decimal UnitPrice { get; set; }

    [XmlElement("MontoTotal")]
    public decimal TotalAmount { get; set; }

    [XmlElement("SubTotal")]
    public decimal SubTotal { get; set; }

    [XmlElement("Impuesto")]
    public TaxE Tax { get; set; }

    [XmlElement("ImpuestoNeto")]
    public decimal NetTax { get; set; }

    [XmlElement("MontoTotalLinea")]
    public decimal TotalLineAmount { get; set; }
}

public class CommercialCodeE
{
    [XmlElement("Tipo")]
    public string Type { get; set; }

    [XmlElement("Codigo")]
    public string Code { get; set; }
}

public class TaxE
{
    [XmlElement("Codigo")]
    public string Code { get; set; }

    [XmlElement("CodigoTarifa")]
    public string TariffCode { get; set; }

    [XmlElement("Tarifa")]
    public decimal Rate { get; set; }

    [XmlElement("Monto")]
    public decimal Amount { get; set; }
}

public class InvoiceSummaryE
{
    [XmlElement("CodigoTipoMoneda")]
    public CurrencyCodeE CurrencyCode { get; set; }

    [XmlElement("TotalVenta")]
    public decimal TotalSale { get; set; }

    [XmlElement("TotalVentaNeta")]
    public decimal NetTotalSale { get; set; }

    [XmlElement("TotalImpuesto")]
    public decimal TotalTax { get; set; }

    [XmlElement("TotalComprobante")]
    public decimal TotalVoucher { get; set; }
    [XmlIgnore]
    public decimal Price { get; set; }
}

public class CurrencyCodeE
{
    [XmlElement("CodigoMoneda")]
    public string Currency { get; set; }

    [XmlElement("TipoCambio")]
    public decimal ExchangeRate { get; set; }
}

public class OthersE
{
    [XmlElement("OtroTexto")]
    public string OtherText { get; set; }
}
