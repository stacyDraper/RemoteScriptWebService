﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IService")]
public interface IService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ExecuteJobs", ReplyAction="http://tempuri.org/IService/ExecuteJobsResponse")]
    string ExecuteJobs(string[] ids, string[] siteUrls);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ExecuteJobs", ReplyAction="http://tempuri.org/IService/ExecuteJobsResponse")]
    System.Threading.Tasks.Task<string> ExecuteJobsAsync(string[] ids, string[] siteUrls);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ExecuteJob", ReplyAction="http://tempuri.org/IService/ExecuteJobResponse")]
    string ExecuteJob(string id, string siteUrl);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ExecuteJob", ReplyAction="http://tempuri.org/IService/ExecuteJobResponse")]
    System.Threading.Tasks.Task<string> ExecuteJobAsync(string id, string siteUrl);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/setup", ReplyAction="http://tempuri.org/IService/setupResponse")]
    string setup(string hostUrl);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/setup", ReplyAction="http://tempuri.org/IService/setupResponse")]
    System.Threading.Tasks.Task<string> setupAsync(string hostUrl);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IServiceChannel : IService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class ServiceClient : System.ServiceModel.ClientBase<IService>, IService
{
    
    public ServiceClient()
    {
    }
    
    public ServiceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string ExecuteJobs(string[] ids, string[] siteUrls)
    {
        return base.Channel.ExecuteJobs(ids, siteUrls);
    }
    
    public System.Threading.Tasks.Task<string> ExecuteJobsAsync(string[] ids, string[] siteUrls)
    {
        return base.Channel.ExecuteJobsAsync(ids, siteUrls);
    }
    
    public string ExecuteJob(string id, string siteUrl)
    {
        return base.Channel.ExecuteJob(id, siteUrl);
    }
    
    public System.Threading.Tasks.Task<string> ExecuteJobAsync(string id, string siteUrl)
    {
        return base.Channel.ExecuteJobAsync(id, siteUrl);
    }
    
    public string setup(string hostUrl)
    {
        return base.Channel.setup(hostUrl);
    }
    
    public System.Threading.Tasks.Task<string> setupAsync(string hostUrl)
    {
        return base.Channel.setupAsync(hostUrl);
    }
}
