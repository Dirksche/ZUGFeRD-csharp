using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using s2industries.ZUGFeRD.Test.InvoiceProviders;

namespace s2industries.ZUGFeRD.Test.CreateAndCompareAgainstReferenceFile
{

    [TestClass]
    public class SEPAPrenotificationTest : TestBase
    {
        private SEPAPrenotification _InvoiceProvider = new SEPAPrenotification();

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
