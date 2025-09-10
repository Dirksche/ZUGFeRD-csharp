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

namespace s2industries.ZUGFeRD.Test
{
    public class TestBase
    {
        protected string _makeSurePathIsCrossPlatformCompatible(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return path.Replace('\\', System.IO.Path.DirectorySeparatorChar);
        } // !_makeSurePathIsCrossPlatformCompatible()


        /// <summary>
        /// Asserts that two strings are equal, using an ordinal comparison, and suppresses detailed failure output.
        /// </summary>
        /// <remarks>This method performs a case-sensitive, culture-invariant comparison of the two
        /// strings. If the strings are not equal, the provided <paramref name="message"/> is displayed in the assertion
        /// failure.</remarks>
        /// <param name="expected">The expected string value.</param>
        /// <param name="actual">The actual string value to compare against the expected value.</param>
        /// <param name="message">The message to display if the assertion fails.</param>
        protected void AssertEqualSuppressDetails(string expected, string actual, string message)
        {
            if (!string.Equals(expected, actual, StringComparison.Ordinal))
            {
                Assert.Fail(message);
            }
        }


        /// <summary>
        /// Creates an invoice using the provided invoice provider, saves it to a temporary file,
        /// and compares it against a reference file.
        /// </summary>
        /// <param name="_InvoiceProvider">The invoice provider to create the invoice.</param>
        /// <param name="version">The version to be used.</param>
        /// <param name="profile">The profile to be used.</param>
        /// <param name="format">The format to be used.</param>
        internal void CreateAndCompare(InvoiceProviders.InvoiceProviderBase _InvoiceProvider, ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format)
        {
            var desc = _InvoiceProvider.CreateInvoice();
            string className = GetType().Name;
            string filename = $"{className}_{version}_{profile}_{format}.xml";

            string fullFilePathOfGeneratedInvoice = _makeSurePathIsCrossPlatformCompatible(System.IO.Path.GetTempPath() + filename);
            using (var fs = new FileStream(fullFilePathOfGeneratedInvoice, FileMode.Create))
            {
                desc.Save(fs, version, profile, format);
            }

            string fullFilePathOfReferenceInvoice = _makeSurePathIsCrossPlatformCompatible(@"..\\..\\..\\ReferenceFiles\\" + filename);

            string expected = File.ReadAllText(fullFilePathOfReferenceInvoice);
            string actual = File.ReadAllText(fullFilePathOfGeneratedInvoice);

            // If the generated file does not match the reference file, please check whether the changes are valid.
            // - Compare the generated file with the reference file (e.g., using BeyondCompare) and analyse the differences.
            // - Check the validity of the generated file (e.g. with https://www.portinvoice.com/).
            // - If the changes are valid, replace the reference file with the generated file.
            // The following validation warnings are acceptable:
            //   [VD-Valitool-71]-Es konnten keine Angaben wie z.B. Geschäftsführer, Handelsregistereintrag oder ähnliche Angaben
            //                    gefunden werden. (SubjectCode=REG). Bitte prüfen Sie, ob Sie diese Daten ggf. im Freitext angegeben
            //                    haben oder diese Angaben für Ihre Rechnungsstellung nicht relevant sind.
            //      --> This is acceptable because the creator of the e-invoice can fix this themselves by specifying the #-syntax (#REG#) in the text.
            //   [VD-Valitool-148]-Die Rechnung liegt im Format XRechnung vor. Sie enthält Skontobedingungen. Die XRechnung verlangt
            //                     die Angabe von Skontobedingungen in strukturierter Form. Die Skontobedingungen liegen jedoch
            //                     ausschließlich als Freitext vor.
            //      --> This is acceptable because the creator of the e-invoice can fix this themselves by specifying the #-syntax (#SKONTO#...) in the text.
            //   [VD-Valitool-160]-Die Umsatzsteuer-Identifikationsnummer entspricht nicht dem national vorgegebenen Format: Prüfsumme oder Prüfziffer ist falsch.
            //      --> This is acceptable for this test, as the creator of a correct e-invoice should provide a valid VAT ID.
            AssertEqualSuppressDetails(expected, actual, $"The generated e-invoice '{fullFilePathOfGeneratedInvoice}' does not match the reference file." + Environment.NewLine +
                $"Please validate the changes and update the reference files.");
        }
    }
}
