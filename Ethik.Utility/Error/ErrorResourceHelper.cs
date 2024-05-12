using System.Reflection;
using System.Resources;

namespace Ethik.Utility.Error;

public class ErrorResourceHelper
{
    private static ResourceManager _resourceManager;
        
    public ErrorResourceHelper(string resourcePath)
    {
        string callingAssembly = Assembly.GetCallingAssembly().GetName().Name!;
        _resourceManager = new ResourceManager(callingAssembly+"."+resourcePath, Assembly.GetCallingAssembly());
        try
        {
            _resourceManager.GetString("check");
        }
        catch (MissingManifestResourceException ex)
        {
            throw new ErrorResourceException($"Resource not found [{resourcePath}], please verify the provided resource path", ex);
        }
    }

    public ErrorResourceHelper(Assembly callingAssembly, string resourcePath)
    {
        _resourceManager = new ResourceManager(callingAssembly.GetName().Name! + "." + resourcePath, callingAssembly);
        try
        {
            _resourceManager.GetString("check");
        }
        catch (MissingManifestResourceException ex)
        {
            throw new ErrorResourceException($"Resource not found [{resourcePath}], please verify the provided resource path", ex);
        }
    }

    public ErrorInfo GetResourceInfo(string key)
    {
        try
        {
            var errValue = _resourceManager.GetString(key) ?? throw new ErrorResourceException($"Key not found with the name [{key}]");
            var value = errValue.Split(":");
            if(value.Length!=2) throw new ErrorResourceException($"Error value string is not formatted correctly, please ensure the value matches the correct format i.e. ErrorCode:ErrorMessage");
            var resourceInfo = new ErrorInfo
            {
                Error = key.ToString(),
                ErrorCode = Convert.ToInt32(value[0]),
                ErrorMessage = value[1],
                ErrorDescription = _resourceManager.GetString($"{value[0]}_DESCRIPTION") ?? throw new ErrorResourceException($"Error description not found in resource i.e. {value[0]}_DESCRIPTION"),
                ErrorSolution = _resourceManager.GetString($"{value[0]}_SOLUTION") ?? throw new ErrorResourceException($"Error solution not found in resource i.e. {value[0]}_SOLUTION"),
            };
            return resourceInfo;
        }
        catch (MissingManifestResourceException ex)
        {
            throw new ErrorResourceException($"Resource not found, please verify the provided resource path", ex);
        }
    }


}
