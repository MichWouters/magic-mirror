using MagicMirror.Business.Models.Cognitive;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Business.Cognitive
{
    public class FaceService
    {
        // TODO: app settings (https://docs.microsoft.com/en-us/windows/uwp/app-settings/app-settings-and-data)
        private const string API_KEY = "2857aeacf78c475c80fa2467fa917397";

        private const string API_ENDPOINT = "https://westeurope.api.cognitive.microsoft.com/face/v1.0";
        private const string MAGIC_MIRROR_GROUP = "magic-mirror-group";

        private readonly FaceServiceClient _faceApiClient;

        public FaceService()
        {
            _faceApiClient = new FaceServiceClient(API_KEY, API_ENDPOINT);
            var t = Task.Run(() => CreateGroupIfNotExists());
            t.Wait();
        }

        /// <summary>
        /// Creates the default PersonGroup
        /// </summary>
        /// <returns></returns>
        public async Task CreateGroupIfNotExists()
        {
            try
            {
                var groups = await _faceApiClient.ListPersonGroupsAsync();
                if (!groups.Any(g => g.PersonGroupId == MAGIC_MIRROR_GROUP))
                {
                    await _faceApiClient.CreatePersonGroupAsync(MAGIC_MIRROR_GROUP, MAGIC_MIRROR_GROUP);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // TODO: logging
            }
        }

        /// <summary>
        /// Creates a person with a given name
        /// </summary>
        /// <param name="name">first name + last name</param>
        /// <returns>Guid personId</returns>
        public async Task<Guid> CreatePersonAsync(string name)
        {
            var result = await _faceApiClient.CreatePersonAsync(MAGIC_MIRROR_GROUP, name);
            return result.PersonId;
        }

        /// <summary>
        /// Adds a face to an existing user
        /// </summary>
        /// <param name="personId">Guid personId</param>
        /// <param name="imageStream">Stream imageStream</param>
        /// <returns></returns>
        public async Task<Guid> AddFaceAsync(Guid personId, Stream imageStream)
        {
            imageStream.Position = 0;
            var result = await _faceApiClient.AddPersonFaceAsync(MAGIC_MIRROR_GROUP, personId, imageStream);
            StartTrainingAsync();
            return result.PersistedFaceId;
        }

        private async Task StartTrainingAsync()
        {
            Console.WriteLine("Training Group {0}", MAGIC_MIRROR_GROUP);
            await _faceApiClient.TrainPersonGroupAsync(MAGIC_MIRROR_GROUP);
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await _faceApiClient.GetPersonGroupTrainingStatusAsync(MAGIC_MIRROR_GROUP);

                if (trainingStatus.Status != Status.Running)
                {
                    if (trainingStatus.Status == Status.Failed)
                    {
                        Console.WriteLine("Trainig Failed!");
                        Console.WriteLine(trainingStatus.Message);
                    }
                    break;
                }

                Console.Write(".");
                await Task.Delay(1000);
            }

            Console.WriteLine("Group Trained");
        }

        /// <summary>
        /// Removes a person by personId
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="faceId"></param>
        /// <returns></returns>
        public async Task RemovePersonAsync(Guid personId, Guid faceId)
        {
            await _faceApiClient.DeletePersonAsync(MAGIC_MIRROR_GROUP, personId);
        }

        /// <summary>
        /// Removes a face that is linked to a user
        /// </summary>
        /// <param name="personId">Guid personId</param>
        /// <param name="faceId">Guid faceId</param>
        /// <returns></returns>
        public async Task RemoveFaceAsync(Guid personId, Guid faceId)
        {
            await _faceApiClient.DeletePersonFaceAsync(MAGIC_MIRROR_GROUP, personId, faceId);
        }

        /// <summary>
        /// Detects a face from a Stream
        /// </summary>
        /// <param name="imageStream">Stream imageStream</param>
        /// <returns>Guid personId</returns>
        public async Task<FaceInfoModel> DetectFace(Stream imageStream)
        {
            var faces = await _faceApiClient.DetectAsync(imageStream, true, true, new List<FaceAttributeType>
            {
                FaceAttributeType.Age,
                FaceAttributeType.Emotion,
                FaceAttributeType.FacialHair,
                FaceAttributeType.Gender,
                FaceAttributeType.Glasses,
                FaceAttributeType.HeadPose,
                FaceAttributeType.Smile
            });

            var identification = await _faceApiClient.IdentifyAsync(MAGIC_MIRROR_GROUP, faces.Select(f => f.FaceId).ToArray());

            if (identification == null || identification.Length != 1 || !identification.First().Candidates.Any())
            {
                return new FaceInfoModel
                {
                    PersonId = null,
                    Age = faces.FirstOrDefault()?.FaceAttributes?.Age,
                    Gender = faces.FirstOrDefault()?.FaceAttributes?.Gender
                };
            }

            var candidate = identification.First().Candidates.OrderByDescending(x => x.Confidence).FirstOrDefault();

            if (candidate == null)
            {
                return new FaceInfoModel
                {
                    PersonId = null,
                    Age = faces.FirstOrDefault()?.FaceAttributes?.Age,
                    Gender = faces.FirstOrDefault()?.FaceAttributes?.Gender
                };
            }

            return new FaceInfoModel
            {
                PersonId = candidate.PersonId,
                Age = faces.FirstOrDefault()?.FaceAttributes?.Age,
                Gender = faces.FirstOrDefault()?.FaceAttributes?.Gender
            };
        }
    }
}