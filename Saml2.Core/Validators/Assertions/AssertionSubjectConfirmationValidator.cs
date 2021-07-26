﻿using Saml2.Core.Errors;
using Saml2.Core.Models.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Saml2.Core.Validators.Assertions
{
    public interface IAssertionSubjectConfirmationValidator
    {
        Task ValidateList(List<SubjectConfirmation> subjectConfirmations);
        Task Validate(SubjectConfirmation subjectConfirmation);
    }

    public class AssertionSubjectConfirmationValidator: IAssertionSubjectConfirmationValidator
    {
        public async Task ValidateList(List<SubjectConfirmation> subjectConfirmations)
        {
            if (subjectConfirmations != null)
            {
                List<string> errorMessages = new List<string>();

                foreach (SubjectConfirmation confirmation in subjectConfirmations)
                {
                    try
                    {
                        await this.Validate(confirmation);
                    }
                    catch (SamlValidationException error)
                    {
                        errorMessages.Add(error.Message);
                    }
                }

                if (errorMessages.Count == subjectConfirmations.Count)
                {
                    throw new SamlValidationException("Subject should have at least one valid subject confirmation.");
                }
            }
        }

        public async Task Validate(SubjectConfirmation subjectConfirmation)
        {

        }
    }
}
