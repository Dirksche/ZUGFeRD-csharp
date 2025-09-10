using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD.Test.InvoiceProviders
{
    abstract internal class InvoiceProviderBase
    {
        abstract internal InvoiceDescriptor CreateInvoice();
    }
}
