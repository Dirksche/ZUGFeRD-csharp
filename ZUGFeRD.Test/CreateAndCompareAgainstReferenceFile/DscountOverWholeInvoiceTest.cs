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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace s2industries.ZUGFeRD.Test.CreateAndCompareAgainstReferenceFile
{
    [TestClass]
    public class DscountOverWholeInvoiceTest : TestBase
    {
        private InvoiceProviders.DscountOverWholeInvoice _InvoiceProvider = new InvoiceProviders.DscountOverWholeInvoice();

        [DataTestMethod]

        [DataRow(ZUGFeRDVersion.Version20, Profile.Comfort, ZUGFeRDFormats.CII)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended, ZUGFeRDFormats.CII)]

        [DataRow(ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended, ZUGFeRDFormats.CII)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.CII)]

        // https://www.portinvoice.com/ reports on the UBL version a warning
        // "Arithmetical issue:Payable total in XML is 296.10, but calculated total
        //  is 302.16 with tax basis 274.82 and with positions 274.82 = 42.13 + 207.36 + 25.33"
        // This is only reported by the Mustang validation and not by the Valitool validation,
        // although the Valitool validation is usually more strict. There's also no such
        // warning, when validating the CII version. When visualising the CII and UBL versions,
        // both show the same values for the invoice positions, disounts, totals, tax bases
        // and tax amounts.
        // I believe this is a bug in the Mustang validation and have reported it on 10/09/2025 to
        // office@obwyse.com (the people behind https://www.portinvoice.com/).
        [DataRow(ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL)]

        public void CreateAndCompare(ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format)
        {
            CreateAndCompare(_InvoiceProvider, version, profile, format);
        }
    }
}
