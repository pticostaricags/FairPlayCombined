using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.PayPal
{

    public class GetOrderDetailsResponse
    {
        public string? id { get; set; }
        public string? status { get; set; }
        public Gross_Total_Amount? gross_total_amount { get; set; }
        public Application_Context? application_context { get; set; }
        public Purchase_Units[]? purchase_units { get; set; }
        public Redirect_Urls? redirect_urls { get; set; }
        public Link[]? links { get; set; }
        public DateTime create_time { get; set; }
    }

    public class Gross_Total_Amount
    {
        public string? value { get; set; }
        public string? currency { get; set; }
    }

    public class Application_Context
    {
    }

    public class Redirect_Urls
    {
        public string? return_url { get; set; }
        public string? cancel_url { get; set; }
    }

    public class Purchase_Units
    {
        public string? reference_id { get; set; }
        public string? description { get; set; }
        public Amount? amount { get; set; }
        public Payee? payee { get; set; }
        public Item[]? items { get; set; }
        public Shipping_Address? shipping_address { get; set; }
        public string? shipping_method { get; set; }
        public Partner_Fee_Details? partner_fee_details { get; set; }
        public int payment_linked_group { get; set; }
        public string? custom { get; set; }
        public string? invoice_number { get; set; }
        public string? payment_descriptor { get; set; }
        public string? status { get; set; }
    }

    public class Amount
    {
        public string? currency { get; set; }
        public Details? details { get; set; }
        public string? total { get; set; }
    }

    public class Details
    {
        public string? subtotal { get; set; }
        public string? shipping { get; set; }
        public string? tax { get; set; }
    }

    public class Payee
    {
        public string? email { get; set; }
    }

    public class Shipping_Address
    {
        public string? line1 { get; set; }
        public string? line2 { get; set; }
        public string? city { get; set; }
        public string? country_code { get; set; }
        public string? postal_code { get; set; }
        public string? state { get; set; }
        public string? phone { get; set; }
    }

    public class Partner_Fee_Details
    {
        public Receiver? receiver { get; set; }
        public Amount1? amount { get; set; }
    }

    public class Receiver
    {
        public string? email { get; set; }
    }

    public class Amount1
    {
        public string? value { get; set; }
        public string? currency { get; set; }
    }

    public class Item
    {
        public string? name { get; set; }
        public string? sku { get; set; }
        public string? price { get; set; }
        public string? currency { get; set; }
        public int quantity { get; set; }
    }

    public class Link
    {
        public string? href { get; set; }
        public string? rel { get; set; }
        public string? method { get; set; }
    }
}
