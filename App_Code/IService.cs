using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{


    //[OperationContract]
    //CompositeType GetDataUsingDataContract(CompositeType composite);

    [OperationContract]
    string ExecuteJobs(string[] ids, string[] jobSiteUrls);

    [OperationContract]
    [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json)]
    string ExecuteJob(string id, string jobSiteUrl);

    [OperationContract]
    string setup(string hostUrl);

    // TODO: Add your service operations here
}

// Use a data contract as illustrated in the sample below to add composite types to service operations.
//[DataContract]
//public class CompositeType
//{
//	bool boolValue = true;
//	string stringValue = "Hello ";

//	[DataMember]
//	public bool BoolValue
//	{
//		get { return boolValue; }
//		set { boolValue = value; }
//	}

//	[DataMember]
//	public string StringValue
//	{
//		get { return stringValue; }
//		set { stringValue = value; }
//	}
//}
