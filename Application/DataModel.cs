/*
Sound Recorder
http://www.codeproject.com/Articles/814903/Practical-Sound-Recorder-with-Sound-Activation
Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Application {
    using System.Runtime.Serialization;
    using System.Xml;
    using System.IO;

    [DataContract(Name="SoundRecorder", Namespace=@"http://www.SAKryukov.org/Schema/SoundRecorder")]
    class ModelTop {

        [DataMember(Order = 1)]
        internal bool UseSoundActivation { get; set; }
        [DataMember(Order = 2)]
        internal int DelayBeforeActivationMs { get; set; }
        [DataMember(Order = 3)]
        internal double ActivationThreshold { get; set; }
        [DataMember(Order = 4)]
        internal bool AutoRestartOnSave { get; set; }
        [DataMember(Order = 5)]
        internal string BaseFileName { get; set; }
        [DataMember(Order = 6)]
        internal int NumberOfDigitsInFileNumber { get; set; }

        internal void Store(string fileName) {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineHandling = NewLineHandling.Entitize;
            settings.Indent = true;
            settings.IndentChars = "\t";
            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
                serializer.WriteObject(writer, this);
        } //Store

        internal static ModelTop Load(string fileName) {
            using (XmlReader reader = XmlReader.Create(fileName))
                return (ModelTop)serializer.ReadObject(reader);
        } //Load 

        static DataContractSerializer serializer = new DataContractSerializer(typeof(ModelTop));

    } //class Top

} //SoundRecorder.Application