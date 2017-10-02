using MagicMirror.Business.Models.Cognitive;
using MagicMirror.Business.Services;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MagicMirror.Business.Cognitive
{
    public class FaceService
    { 
        const string API_KEY = "";
        const string API_ENDPOINT = "";
        private readonly FaceServiceClient _faceApiClient;

        public FaceService()
        {
            _faceApiClient = new FaceServiceClient(API_KEY, API_ENDPOINT);
        }

        async Task<IEnumerable<FaceInfoModel>> GetFaceInfoAsync(Stream imageStream)
        {
            var attributes = new List<FaceAttributeType>
            {
                FaceAttributeType.Age,
                FaceAttributeType.Emotion,
                FaceAttributeType.FacialHair,
                FaceAttributeType.Gender,
                FaceAttributeType.Glasses,
                FaceAttributeType.HeadPose,
                FaceAttributeType.Smile
            };
            return await _faceApiClient.DetectAsync(imageStream, true, true, attributes).ContinueWith(ProcessFaces);
        }

        private IEnumerable<FaceInfoModel> ProcessFaces(Task<Face[]> faces)
        {
            foreach (var face in faces.Result)
            {
                yield return new FaceInfoModel
                {
                    FaceId = face.FaceId,
                    Age = face.FaceAttributes.Age,
                    Gender = face.FaceAttributes.Gender
                };
            }
        }
    }
}
