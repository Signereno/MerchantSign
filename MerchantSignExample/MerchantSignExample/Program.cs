//######################################################################//
//            This example uses the following nuget packages:           //
//                                                                      //
//            - IdentiSign.MerchantSign.Client                          //
//            - IdentiSign.MerchantSign.Models                          //
//                                                                      //              
//######################################################################//
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IdentiSign.MerchantSign.Client;
using IdentiSign.MerchantSign.Client.MerchantSignClient;
using IdentiSign.MerchantSign.Models.Request;
using IdentiSign.MerchantSign.Models.Response;
using IdentiSign.MerchantSign.Models.Shared;

namespace MerchantSignExample
{
    class Program
    {
        private static IMerchantSignClient _merchantSignClient;
        private const bool IsProduction = false;

        static void Main(string[] args)
        {
            //Insert your credentials here
            var accountId = Guid.Parse("Enter your account Id here");
            var oauthClientId = "Enter your oauth2 client id here";
            var oauthSecret = "Enter your oauth2 secret here";
            var outPutFilePath1 = Path.Combine(Directory.GetCurrentDirectory(), "App_Data\\SignedTxtFile.sdo");
            var outPutFilePath2 = Path.Combine(Directory.GetCurrentDirectory(), "App_Data\\SignedXmlFile.sdo");
            
            
            _merchantSignClient = new MerchantSignClient(accountId, oauthClientId, oauthSecret);

            //################################################################################################################//
            //                                        Sign text file                                                          //      
            //                                      --------------------                                                      //          
            //################################################################################################################//
            
             var fileData = File.ReadAllBytes("App_Data/FileToSign.txt");

             Console.WriteLine("Signing txt file, please wait...");
             var signRequest = new SignRequest()
             {
                 DataFormat = DataFormat.txt,
                 DataToSign = Convert.ToBase64String(fileData),
                 ExternalReference = "Some external reference",
                 SigningFormat = SigningFormat.no_bankid_seid_sdo
             };

             Sign(signRequest, outPutFilePath1);




            //################################################################################################################//
            //                                        Sign xml file                                                           //      
            //                                      --------------------                                                      //      
            //               Comment out code below to try, xslt file has to be included during xml signing                   //    
            //################################################################################################################//

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

            Sign(xmlSignRequest, outPutFilePath2);
            */
            Console.ReadKey();
        }


        private static void Sign(SignRequest request, string outputFilePath)
        { 
            var result = _merchantSignClient.Sign(request, IsProduction);
            Console.WriteLine("Sign Result");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(Extensions.Serialize(result));
            Console.WriteLine("------------------------------------------------------------");

            File.WriteAllBytes(outputFilePath, Convert.FromBase64String(result.SignedData));
            Console.WriteLine($"Signed data saved to: {outputFilePath}");
            //You can validate the sdo file here: https://www.bankid.no/privat/los-mitt-bankid-problem/les-signerte-dokumenter/
        }



        //################################################################################################################//
        //                  The following methods shows how to retrieve transaction log items                             //      
        //                  -----------------------------------------------------------------                             //          
        //################################################################################################################//
        private static MerchantSignTransaction GetTransaction(Guid transactionId)
        {
            return _merchantSignClient.GetTransaction(transactionId, IsProduction);
        }

        private static List<MerchantSignTransaction> GetAllTransactionsForThisOauthClient()
        {
            return _merchantSignClient.GetTransactions(IsProduction);
        }

        private static List<MerchantSignTransaction> GetAllTransactionsForThisOauthClientWithTimeFilter()
        {
            long from = DateTime.UtcNow.AddDays(-14).Ticks;
            long to = DateTime.UtcNow.AddDays(-10).Ticks;

            return _merchantSignClient.GetTransactions(IsProduction, from, to);
        }
    }
}
