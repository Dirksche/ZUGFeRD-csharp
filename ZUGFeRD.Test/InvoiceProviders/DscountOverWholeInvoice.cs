/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace s2industries.ZUGFeRD.Test.InvoiceProviders
{
    /// <summary>
    /// Class for creating an example invoice with a discount over the whole invoice.
    /// </summary>
    internal class DscountOverWholeInvoice : InvoiceProviderBase
    {
        /// <summary>
        /// Creates an example invoice with a discount over the whole invoice.
        /// </summary>
        /// <returns>InvoiceDescriptor which contains the generated invoice</returns>
        internal override InvoiceDescriptor CreateInvoice()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2024, 03, 05), CurrencyCodes.EUR);

            // The BusinessProcess is required, when creating E-Invoices with the XRechnung profile in CII format.
            desc.BusinessProcess = "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0";

            desc.Name = "WARENRECHNUNG";
            desc.ActualDeliveryDate = new DateTime(2024, 03, 05);

            desc.ReferenceOrderNo = "04011000-12345-34";

            desc.SetSeller(name: "Lieferant GmbH",
                           postcode: "80333",
                           city: "München",
                           street: "Lieferantenstraße 20",
                           country: CountryCodes.DE,
                           id: "Kennung des Verkäufers"
                           );
            desc.SetSellerElectronicAddress("DE123456789", ElectronicAddressSchemeIdentifiers.GermanyVatNumber);
            desc.SetSellerContact(name: "Max Mustermann",
                                  orgunit: "Muster-Einkauf",
                                  emailAddress: "Max@Mustermann.de",
                                  phoneno: "+49891234567",
                                  faxno: "+49891234568"
                                 );
            desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);

            desc.SetBuyer(name: "Kunden AG Mitte",
                          postcode: "69876",
                          city: "Frankfurt",
                          street: "Kundenstraße 15",
                          country: CountryCodes.DE,
                          id: "GE2020211"
                          );
            desc.SetBuyerElectronicAddress("DE123123123", ElectronicAddressSchemeIdentifiers.GermanyVatNumber);

            desc.AddNote("Rechnung gemäß Bestellung Nr. 2018-471331 vom 01.03.2018.");
            desc.AddNote("Es bestehen Rabatt- und Bonusvereinbarungen.", SubjectCodes.AAK);
            desc.AddNote("Lieferant GmbH\r\nLieferantenstraße 20\r\n80333 München\r\nDeutschland\r\nGeschäftsführer: Hans Muster\r\nHandelsregisternummer: H A 123\r\n",
                         SubjectCodes.REG);

            // add three articles
            Decimal lineTotalAmount19 = 0;
            Decimal lineTotalAmount7 = 0;
            {
                TradeLineItem tradeLineItem = desc.AddTradeLineItem("001",                  // BT-126 Kennung der Rechnungsposition (normalerweise die Rechnungszeilennummer). Wenn nicht angegeben, dann wird diese automatisch vergeben.
                                                                    "BERLTHYROX 100UG",     // BT-153 Artikelname
                                                                    14.0439m,               // BT-146 der Preis eines Artikels ohne Umsatzsteuer nach Abzug des Nachlass auf den Artikelpreis
                                                                    QuantityCodes.XPP,      // BT-130 Code der Maßeinheit der in Rechnung gestellten Menge
                                                                    "My description",       // BT-154 Artikelbeschreibung
                                                                    null,                   // BT-149 die Anzahl von Artikeleinheiten, für die der Preis gilt
                                                                    null,                   // BT-148 der Einheitspreis **ohne Umsatzsteuer** vor Abzug des Nachlass auf den Artikelpreis
                                                                    3,                      // BT-129 in Rechnung gestellte Menge
                                                                    null,                   // BT-131 Nettobetrag der Rechnungsposition
                                                                    TaxTypes.VAT,           // BT-151-0
                                                                    TaxCategoryCodes.S,     // BT-151 Code der Umsatzsteuerkategorie des in Rechnung gestellten Artikels
                                                                    19,                     // BT-152 Umsatzsteuersatz für den in Rechnung gestellten Artikel
                                                                    "My comment",           // BT-127 Freitext zur Rechnungsposition
                                                                    null,                   // BT-157 Kennung eines Artikels nach registriertem Schema
                                                                    "04169807",             // BT-155 eine dem Artikel vom Verkäufer zugewiesene Kennung
                                                                    null,                   // BT-156 eine dem Artikel vom Käufer zugewiesene Kennung
                                                                    null, // DeliveryNoteID		// BT-X-92
                                                                    null, // DeliveryNoteDate	// BT-X-94
                                                                    null, // BuyerOrderLineID	// BT-132 Referenz zur Bestellposition
                                                                    null, // BuyerOrderID		// BT-132 Referenz zur Bestellposition
                                                                    null, // Bestelldatum		// BT-132 Referenz zur Bestellposition
                                                                    null, // Start of billing period
                                                                    null); // End of billing period

                tradeLineItem.AddApplicableProductCharacteristic("Package size", "100 St");
                tradeLineItem.AddApplicableProductCharacteristic("Dosage form", "Tablets");
                lineTotalAmount19 += Math.Round(14.0439m * 3, 2);
            }
            {
                TradeLineItem tradeLineItem = desc.AddTradeLineItem("002",                  // BT-126 Kennung der Rechnungsposition (normalerweise die Rechnungszeilennummer). Wenn nicht angegeben, dann wird diese automatisch vergeben.
                                                                    "INFECTOTRIMET 100MG",  // BT-153 Artikelname
                                                                    20.7357m,               // BT-146 der Preis eines Artikels ohne Umsatzsteuer nach Abzug des Nachlass auf den Artikelpreis
                                                                    QuantityCodes.XPP,      // BT-130 Code der Maßeinheit der in Rechnung gestellten Menge
                                                                    "My 2. description",    // BT-154 Artikelbeschreibung
                                                                    null,                   // BT-149 die Anzahl von Artikeleinheiten, für die der Preis gilt
                                                                    null,                   // BT-148 der Einheitspreis **ohne Umsatzsteuer** vor Abzug des Nachlass auf den Artikelpreis
                                                                    10,                     // BT-129 in Rechnung gestellte Menge
                                                                    null,                   // BT-131 Nettobetrag der Rechnungsposition
                                                                    TaxTypes.VAT,           // BT-151-0
                                                                    TaxCategoryCodes.S,     // BT-151 Code der Umsatzsteuerkategorie des in Rechnung gestellten Artikels
                                                                    7,                      // BT-152 Umsatzsteuersatz für den in Rechnung gestellten Artikel
                                                                    "My 2. comment",        // BT-127 Freitext zur Rechnungsposition
                                                                    null,                   // BT-157 Kennung eines Artikels nach registriertem Schema
                                                                    "02736107",             // BT-155 eine dem Artikel vom Verkäufer zugewiesene Kennung
                                                                    null,                   // BT-156 eine dem Artikel vom Käufer zugewiesene Kennung
                                                                    null, // DeliveryNoteID		// BT-X-92
                                                                    null, // DeliveryNoteDate	// BT-X-94
                                                                    null, // BuyerOrderLineID	// BT-132 Referenz zur Bestellposition
                                                                    null, // BuyerOrderID		// BT-132 Referenz zur Bestellposition
                                                                    null, // Bestelldatum		// BT-132 Referenz zur Bestellposition
                                                                    null, // Start of billing period
                                                                    null); // End of billing period

                tradeLineItem.AddApplicableProductCharacteristic("Package size", "50 St");
                tradeLineItem.AddApplicableProductCharacteristic("Dosage form", "Tablets");
                lineTotalAmount7 += Math.Round(20.7357m * 10, 2);
            }
            {
                TradeLineItem tradeLineItem = desc.AddTradeLineItem("003",                  // BT-126 Kennung der Rechnungsposition (normalerweise die Rechnungszeilennummer). Wenn nicht angegeben, dann wird diese automatisch vergeben.
                                                                    "METOHEXAL-SUCC 23.75MG",   // BT-153 Artikelname
                                                                    12.6666m,               // BT-146 der Preis eines Artikels ohne Umsatzsteuer nach Abzug des Nachlass auf den Artikelpreis
                                                                    QuantityCodes.XPP,      // BT-130 Code der Maßeinheit der in Rechnung gestellten Menge
                                                                    "My 3. description",    // BT-154 Artikelbeschreibung
                                                                    null,                   // BT-149 die Anzahl von Artikeleinheiten, für die der Preis gilt
                                                                    null,                   // BT-148 der Einheitspreis **ohne Umsatzsteuer** vor Abzug des Nachlass auf den Artikelpreis
                                                                    2,                      // BT-129 in Rechnung gestellte Menge
                                                                    null,                   // BT-131 Nettobetrag der Rechnungsposition
                                                                    TaxTypes.VAT,           // BT-151-0
                                                                    TaxCategoryCodes.S,     // BT-151 Code der Umsatzsteuerkategorie des in Rechnung gestellten Artikels
                                                                    19,                     // BT-152 Umsatzsteuersatz für den in Rechnung gestellten Artikel
                                                                    "My 3. comment",        // BT-127 Freitext zur Rechnungsposition
                                                                    null,                   // BT-157 Kennung eines Artikels nach registriertem Schema
                                                                    "00850419",             // BT-155 eine dem Artikel vom Verkäufer zugewiesene Kennung
                                                                    null,                   // BT-156 eine dem Artikel vom Käufer zugewiesene Kennung
                                                                    null, // DeliveryNoteID		// BT-X-92
                                                                    null, // DeliveryNoteDate	// BT-X-94
                                                                    null, // BuyerOrderLineID	// BT-132 Referenz zur Bestellposition
                                                                    null, // BuyerOrderID		// BT-132 Referenz zur Bestellposition
                                                                    null, // Bestelldatum		// BT-132 Referenz zur Bestellposition
                                                                    null, // Start of billing period
                                                                    null); // End of billing period

                tradeLineItem.AddApplicableProductCharacteristic("Package size", "100 St");
                tradeLineItem.AddApplicableProductCharacteristic("Dosage form", "Retard tablets");
                lineTotalAmount19 += Math.Round(12.6666m * 2, 2);
            }

            // add a discount of 2 percent
            // NOTE: If you have items with different VAT rates in your invoice and you want to give a discount
            //       on the whole invoice, you need to specify the discount per VAT rate.
            Decimal discount19 = Math.Round(lineTotalAmount19 * 0.02m, 2); // 2% discount on all articles with 19% VAT
            desc.AddTradeAllowance(null,
                                   CurrencyCodes.EUR,
                                   discount19,
                                   null,
                                   "Auftragsrabatt",
                                   TaxTypes.VAT,
                                   TaxCategoryCodes.S,
                                   19,
                                   null);

            Decimal discount7 = Math.Round(lineTotalAmount7 * 0.02m, 2); // 2% discount on all articles with 7% VAT
            desc.AddTradeAllowance(null,
                                   CurrencyCodes.EUR,
                                   discount7,
                                   null,
                                   "Auftragsrabatt",
                                   TaxTypes.VAT,
                                   TaxCategoryCodes.S,
                                   7,
                                   null);

            // add VAT information
            Decimal taxAmount7 = Math.Round((lineTotalAmount7 - discount7) * 0.07m, 2, MidpointRounding.AwayFromZero);
            desc.AddApplicableTradeTax(basisAmount: lineTotalAmount7 - discount7,
                                       percent: 7m,
                                       taxAmount: taxAmount7,
                                       typeCode: TaxTypes.VAT,
                                       categoryCode: TaxCategoryCodes.S
                                       );

            Decimal taxAmount19 = Math.Round((lineTotalAmount19 - discount19) * 0.19m, 2, MidpointRounding.AwayFromZero);
            desc.AddApplicableTradeTax(basisAmount: lineTotalAmount19 - discount19,
                                       percent: 19m,
                                       taxAmount: taxAmount19,
                                       typeCode: TaxTypes.VAT,
                                       categoryCode: TaxCategoryCodes.S
                                       );

            // add totals
            Decimal lineTotal = lineTotalAmount7 + lineTotalAmount19;
            Decimal allowanceTotal = discount7 + discount19;
            Decimal taxTotal = taxAmount7 + taxAmount19;
            desc.SetTotals(lineTotalAmount: lineTotal,
                           allowanceTotalAmount: allowanceTotal,
                           taxBasisAmount: lineTotal - allowanceTotal,
                           taxTotalAmount: taxTotal,
                           grandTotalAmount: lineTotal - allowanceTotal + taxTotal,
                           duePayableAmount: lineTotal - allowanceTotal + taxTotal
                          );

            desc.AddTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018, 3% Skonto innerhalb 10 Tagen bis 15.03.2018");
            desc.SetPaymentMeans(PaymentMeansTypeCodes.SEPACreditTransfer); // BG-17 Überweisung

            // Bei Überweisung (BG-17) müssen wir die Kontodaten des Verkäufers angeben.
            desc.AddCreditorFinancialAccount(iban: "DE43123456789012345678", // BT-84 - IBAN des Zahlungsempfängers
                                             bic: "", // BT-86 - BIC des Zahlungsempfängers (bei deutschen Banken nicht zwingend erforderlich)
                                             name: "Lieferant GmbH");

            return desc;
        }
    }
}
