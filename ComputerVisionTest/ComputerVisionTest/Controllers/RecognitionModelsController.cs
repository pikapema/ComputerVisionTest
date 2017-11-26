using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Cognitive.CustomVision;
using System.IO;
using System.Diagnostics;
using ComputerVisionTest.Models;

namespace ComputerVisionTest.Controllers
{
    public class RecognitionModelsController : Controller
    {
        // GET: RecognitionModels
        public async Task<ActionResult> Index([Bind(Include = "Id,Key,ImageUpload")] RecognitionModel recognitionModel)
        {
            ViewBag.Message = "Result will show here.";
            Trace.WriteLine("Starting Prediction...");

            if (recognitionModel.ImageUpload != null)
            {
                // Create image.                
                Guid projectid = new Guid("57471653-6e79-455f-b874-ee00d1014c37");

                // Create a prediction endpoint, passing in a prediction credentials object that contains the obtained prediction key

                PredictionEndpointCredentials predictionEndpointCredentials = new PredictionEndpointCredentials(recognitionModel.Key);
                PredictionEndpoint endpoint = new PredictionEndpoint(predictionEndpointCredentials);

                if (recognitionModel.ImageUpload != null && recognitionModel.ImageUpload.ContentLength > 0)
                {
                    //var uploadDir = "~/uploads";
                    //var imagePath = Path.Combine(Server.MapPath(uploadDir), recognitionModel.ImageUpload.FileName);
                    //var imageUrl = Path.Combine(uploadDir, recognitionModel.ImageUpload.FileName);
                    //Trace.WriteLine("ImageUrl:" + imageUrl);                    
                    //Trace.WriteLine("Image path:" + imagePath);
                    //recognitionModel.ImageUpload.SaveAs(imagePath);

                    Trace.WriteLine("createing memory stream");
                    //MemoryStream memStream = new MemoryStream(System.IO.File.ReadAllBytes(imagePath));

                    MemoryStream memStream = new MemoryStream();
                    recognitionModel.ImageUpload.InputStream.Position = 0;
                    recognitionModel.ImageUpload.InputStream.CopyTo(memStream);
                    memStream.Position = 0;

                    // Make a prediction against the new project
                    Trace.WriteLine("Making a prediction:");
                    try
                    {
                        var result = endpoint.PredictImage(projectid, memStream);

                        // Loop over each prediction and write out the results
                        var predicition = result.Predictions.FirstOrDefault(P => P.Probability >= 0.8);
                        if (predicition == null)
                            ViewBag.Message = "Could not find a good match for the uploaded image. Please try again.";
                        else
                        {
                            ViewBag.Message = "Your image is of type " + predicition.Tag + " with the probability of " + predicition.Probability;
                        }
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "Could not make a predicition of image " + recognitionModel.ImageUpload.FileName + "! Error message: " + e.Message;
                    }
                }
            }
            return View(recognitionModel);
        }
    }
}