﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Psps.Web.Core.Mvc
{
    public static class ModelStateExtensions
    {
        public static object SerializeErrors(this ModelStateDictionary modelState)
        {
            return modelState
                .Where<KeyValuePair<string, ModelState>>(entry => entry.Value.Errors.Any<ModelError>())
                .ToDictionary<KeyValuePair<string, ModelState>, string, Dictionary<string, object>>(entry => entry.Key, entry => SerializeModelState(entry.Value));
        }

        public static Dictionary<string,string> SerializeErrorsToStringDictionary(this ModelStateDictionary modelState)
        {
            return modelState
                .Where<KeyValuePair<string, ModelState>>(entry => entry.Value.Errors.Any<ModelError>())
                .ToDictionary(entry=>entry.Key,entry => SerializeErrorsToStringDictionary(entry.Value)["errors"]);
        }

        public static object ToDataSourceResult(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.SerializeErrors();
            }
            return null;
        }

        private static string GetErrorMessage(ModelError error, ModelState modelState)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
            {
                return error.ErrorMessage;
            }
            if (modelState.Value == null)
            {
                return error.ErrorMessage;
            }
            object[] args = new object[] { modelState.Value.AttemptedValue };
            return string.Format("ValueNotValidForProperty=The value '{0}' is invalid", args);
        }

        private static Dictionary<string, object> SerializeModelState(ModelState modelState)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary["errors"] = modelState.Errors.Select<ModelError, string>(x => GetErrorMessage(x, modelState)).ToArray<string>();
            return dictionary;
        }


        public static Dictionary<string, string> SerializeErrorsToStringDictionary(this ModelState modelState)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary["errors"] = string.Join(",",modelState.Errors.Select<ModelError, string>(x => GetErrorMessage(x, modelState)).ToArray<string>());
            return dictionary;
        }
    }
}