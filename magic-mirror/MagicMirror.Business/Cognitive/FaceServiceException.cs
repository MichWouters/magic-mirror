//using System;

//namespace MagicMirror.Business.Cognitive
//{
//    [Serializable]
//    public class FaceServiceException : Exception
//    {
//        public FaceServiceException()
//        {
//        }

//        public FaceServiceException(FaceServiceExceptionType exceptionType) : base(exceptionType.ToString())
//        {
//            ExceptionType = exceptionType;
//        }

//        public FaceServiceExceptionType ExceptionType { get; private set; }
//    }

//    public enum FaceServiceExceptionType
//    {
//        NoFaceDetected,
//        MultipleFacesDetected
//    }
//}