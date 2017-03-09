/*######################################################################################################
            This example uses the following nuget packages:

            - IdentiSign.MerchantSign.Client
            - IdentiSign.MerchantSign.Models 

  ###################################################################################################### */
using System;
using System.IO;
using System.Threading.Tasks;
using IdentiSign.MerchantSign.Client;
using IdentiSign.MerchantSign.Client.MerchantSignClient;
using IdentiSign.MerchantSign.Models.Request;
using IdentiSign.MerchantSign.Models.Shared;

namespace MerchantSignExample
{
    class Program
    {
        private static IMerchantSignClient _merchantSignClient;

        static void Main(string[] args)
        {
            //Insert your credentials here
            var oauthClientId = "Your oauth2 client id";
            var oauthSecret = "Your oauth2 client secret";
            var accountId = Guid.Parse("Your Signere accountId");
            var outPutFilePath1 = Path.Combine(Directory.GetCurrentDirectory(), "App_Data\\SignedTxtFile.sdo");
            var outPutFilePath2 = Path.Combine(Directory.GetCurrentDirectory(), "App_Data\\SignedXmlFile.sdo");
            
            
            _merchantSignClient = new MerchantSignClient(accountId, oauthClientId, oauthSecret);

            /*################################################################################################################
                                                    Sign text file
                                                  --------------------
              ################################################################################################################*/
            var fileData = File.ReadAllBytes("App_Data/FileToSign.txt");

            Console.WriteLine("Signing txt file, please wait...");
            var signRequest = new SignRequest()
            {
                DataFormat = DataFormat.txt,
                DataToSign = Convert.ToBase64String(fileData),
                ExternalReference = "Some external reference",
                SigningFormat = SigningFormat.no_bankid_seid_sdo
            };
            //Signing txt file
            Task.Run(async () => await Sign(signRequest, outPutFilePath1));


            /*################################################################################################################
                                                  Sign xml file
                                             -----------------------
                        Comment out code below to try, xslt file has to be included during xml signing
              ################################################################################################################*/
        /*
            var xml = File.ReadAllBytes("App_Data/SignereDummy.xml");
            var xslt = File.ReadAllBytes("App_Data/SignereDummy.xslt");

            Console.WriteLine("Signing xml file, please wait...");
            var xmlSignRequest = new SignRequest()
            {
                DataFormat = DataFormat.xml,
                DataToSign = Convert.ToBase64String(xml),
                Xslt = Convert.ToBase64String(xslt),
                ExternalReference = "Some external reference",
                SigningFormat = SigningFormat.no_bankid_seid_sdo
            };
            //Signing txt file
            Task.Run(async () => await Sign(xmlSignRequest, outPutFilePath2));
        */

            Console.ReadKey();
        }


        private static async Task Sign(SignRequest request, string outputFilePath)
        { 
            var result = await _merchantSignClient.Sign(request, false);
            Console.WriteLine("Sign Result");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(Extensions.Serialize(result));
            Console.WriteLine("------------------------------------------------------------");

            File.WriteAllBytes(outputFilePath, Convert.FromBase64String(result.SignedData));
            Console.WriteLine($"Signed data saved to: {outputFilePath}");
            //You can validate the sdo file here: https://www.bankid.no/privat/los-mitt-bankid-problem/les-signerte-dokumenter/
        }
    }
}
