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
    /// Class for creating an example invoice with bank transfer as payment method.
    /// </summary>
    internal class PaymentMethodBankTransfer : InvoiceProviderBase
    {
        /// <summary>
        /// Creates an example invoice with bank transfer as payment method.
        ///
        /// The created invoice is identical to the invoice created by "InvoiceProviders.PaymentMethodDirectDebit.cs",
        /// but the payment method is not by direct debit, but by bank transfer.
        /// </summary>
        /// <returns>InvoiceDescriptor which contains the generated invoice</returns>
        internal override InvoiceDescriptor CreateInvoice()
        {
            // Wir verwenden als Rechnungsdatum nicht 2018, sondern ein aktuelleres Jahr, um folgende Validierungs-Warnung zu vermeiden:
            //    [VD-Valitool-96]-Es existiert kein Prüfprofil, bei dem Standard, Guideline und Gültigkeitsdatum zum Dokument passen.
            //                     Jedoch wurde das Profil XRechnung3p0p0UI.vdc zur Prüfung herangezogen, das zwischen 01.02.2024 und
            //                     31.12.2099 gültig ist. Das Dokumentendatum lautet: 05.03.2018.
            // Es ist auch unwahrscheinlich, dass jemand heute noch Rechnungen mit dem Datum 2018 erstellt.
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
                           id: String.Empty,
                           globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"),
                           legalOrganization: new LegalOrganization(GlobalIDSchemeIdentifiers.GLN, "4000001123452", "Lieferant GmbH")
                           );
            desc.SetSellerElectronicAddress("DE123456789", ElectronicAddressSchemeIdentifiers.GermanyVatNumber);
            desc.SetSellerContact(name: "Max Mustermann",
                                  orgunit: "Muster-Einkauf",
                                  emailAddress: "Max@Mustermann.de",
                                  phoneno: "+49891234567",
                                  faxno: "+49891234568"
                                 );
            desc.AddSellerTaxRegistration("201/113/40209", TaxRegistrationSchemeID.FC);
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

            desc.AddTradeLineItem(name: "Trennblätter A4",
                                  unitCode: QuantityCodes.H87,
                                  sellerAssignedID: "TB100A4",
                                  id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4012345001235"),
                                  grossUnitPrice: 9.9m,
                                  netUnitPrice: 9.9m,
                                  billedQuantity: 20m,
                                  lineTotalAmount: 198.0m,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.S,
                                  taxPercent: 19m
                                 );

            desc.AddTradeLineItem(name: "Joghurt Banane",
                unitCode: QuantityCodes.H87,
                sellerAssignedID: "ARNR2",
                id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4000050986428"),
                grossUnitPrice: 5.5m,
                netUnitPrice: 5.5m,
                billedQuantity: 50,
                lineTotalAmount: 275.0m,
                taxType: TaxTypes.VAT,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 7
                );


            desc.AddApplicableTradeTax(basisAmount: 275.0m,
                                       percent: 7m,
                                       taxAmount: 275.0m / 100m * 7m,
                                       typeCode: TaxTypes.VAT,
                                       categoryCode: TaxCategoryCodes.S
                                       );

            desc.AddApplicableTradeTax(basisAmount: 198.0m,
                                       percent: 19m,
                                       taxAmount: 198.0m / 100m * 19m,
                                       typeCode: TaxTypes.VAT,
                                       categoryCode: TaxCategoryCodes.S
                                       );

            desc.SetTotals(lineTotalAmount: 473.0m,
                           taxBasisAmount: 473.0m,
                           taxTotalAmount: 56.87m,
                           grandTotalAmount: 529.87m,
                           duePayableAmount: 529.87m
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
