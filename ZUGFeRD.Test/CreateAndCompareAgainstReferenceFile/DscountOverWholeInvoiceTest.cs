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

        // I'm not testing UBL as much as CII, because I believe that most people will be using CII.
        // UBL is not supported in ZUGFeRD-PDF files. Therefore, if you plan to embed your e-invoice
        // in a PDF file to make a ZUGFeRD file, you will have to use CII.
        // Furthermore, UBL has other limitations. E.g. it doesn't support credit notes. You will
        // need to use negative invoices instead, when using UBL.
        [DataRow(ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL)]

        public void CreateAndCompare(ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format)
        {
            CreateAndCompare(_InvoiceProvider, version, profile, format);
        }
    }
}
