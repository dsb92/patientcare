using System;

namespace PatientCare.Shared.Model.Extensions.TextBlob
{
    public interface ITextBlobSerializer
    {
        string Serialize(object element);
        object Deserialize(string text, Type type);
    }
}
