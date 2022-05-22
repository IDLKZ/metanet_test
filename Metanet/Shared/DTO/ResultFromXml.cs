using System.Xml.Serialization;

namespace Metanet.Shared.DTO;

public class ResultFromXml
{
    // using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(Result));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Result)serializer.Deserialize(reader);
// }

[XmlRoot(ElementName="rec")]
public class Rec { 

	[XmlElement(ElementName="order_id")] 
	public string OrderId { get; set; } 

	[XmlElement(ElementName="mpi_order")] 
	public string MpiOrder { get; set; } 

	[XmlElement(ElementName="status")] 
	public string Status { get; set; } 

	[XmlElement(ElementName="status_desc")] 
	public string StatusDesc { get; set; } 

	[XmlElement(ElementName="amount")] 
	public string Amount { get; set; } 

	[XmlElement(ElementName="currency")] 
	public string Currency { get; set; } 

	[XmlElement(ElementName="terminal")] 
	public string Terminal { get; set; } 

	[XmlElement(ElementName="client_id")] 
	public string ClientId { get; set; } 

	[XmlElement(ElementName="card_masked")] 
	public string CardMasked { get; set; } 

	[XmlElement(ElementName="card_name")] 
	public string CardName { get; set; } 

	[XmlElement(ElementName="card_expdt")] 
	public string CardExpdt { get; set; } 

	[XmlElement(ElementName="card_token")] 
	public string CardToken { get; set; } 

	[XmlElement(ElementName="description")] 
	public string Description { get; set; } 

	[XmlElement(ElementName="desc_order")] 
	public string DescOrder { get; set; } 

	[XmlElement(ElementName="email")] 
	public string Email { get; set; } 

	[XmlElement(ElementName="lang")] 
	public string Lang { get; set; } 

	[XmlElement(ElementName="phone")] 
	public string Phone { get; set; } 

	[XmlElement(ElementName="create_date")] 
	public string CreateDate { get; set; } 

	[XmlElement(ElementName="result")] 
	public string Result { get; set; } 

	[XmlElement(ElementName="result_desc")] 
	public string ResultDesc { get; set; } 

	[XmlElement(ElementName="rc")] 
	public string Rc { get; set; } 

	[XmlElement(ElementName="rrn")] 
	public string Rrn { get; set; } 

	[XmlElement(ElementName="int_ref")] 
	public string IntRef { get; set; } 

	[XmlElement(ElementName="auth_code")] 
	public string AuthCode { get; set; } 

	[XmlElement(ElementName="inv_id")] 
	public string InvId { get; set; } 

	[XmlElement(ElementName="inv_exp_date")] 
	public string InvExpDate { get; set; } 

	[XmlElement(ElementName="rev_max_amount")] 
	public string RevMaxAmount { get; set; } 

	[XmlElement(ElementName="recur_freq")] 
	public string RecurFreq { get; set; } 

	[XmlElement(ElementName="requr_exp")] 
	public string RequrExp { get; set; } 

	[XmlElement(ElementName="recur_ref")] 
	public string RecurRef { get; set; } 

	[XmlElement(ElementName="recur_int_ref")] 
	public string RecurIntRef { get; set; } 

	[XmlElement(ElementName="card_to_masked")] 
	public string CardToMasked { get; set; } 

	[XmlElement(ElementName="cart_to_token")] 
	public string CartToToken { get; set; } 
}

[XmlRoot(ElementName="operations")]
public class Operations { 

	[XmlElement(ElementName="rec")] 
	public List<Rec> Rec { get; set; } 
}

[XmlRoot(ElementName="result")]
public class Result { 

	[XmlElement(ElementName="code")] 
	public int Code { get; set; } 

	[XmlElement(ElementName="description")] 
	public string Description { get; set; } 

	[XmlElement(ElementName="operations")] 
	public Operations Operations { get; set; } 
}


}